using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Uni.Infrastructure.Interfaces.Services;

namespace Uni.Infrastructure.Services
{
    public class PasswordHasher : IPasswordHasher, IPasswordValidator
    {
        private readonly int _iterationsCount;
        private readonly RandomNumberGenerator _randomNumberGenerator;

        public PasswordHasher(PasswordHasherOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _iterationsCount = options.IterationsCount;
            if (_iterationsCount < 1)
            {
                throw new InvalidOperationException("The iteration count must be a positive integer.");
            }

            _randomNumberGenerator = options.RandomNumberGenerator;
        }

        public string HashPassword(string password)
        {
            var passwordBytes = HashPasswordInternal(password, _randomNumberGenerator);
            var passwordHash = Convert.ToBase64String(passwordBytes);
            return passwordHash;
        }

        public bool Verify(string hashedPassword, string providedPassword)
        {
            var providedPasswordHashBytes = Convert.FromBase64String(hashedPassword);
            var isValid = VerifyHashedPasswordInternal(providedPasswordHashBytes, providedPassword);
            return isValid;
        }

        private byte[] HashPasswordInternal(
            string password,
            RandomNumberGenerator randomNumberGenerator
            )
        {
            return HashPasswordInternal(
                password,
                randomNumberGenerator,
                KeyDerivationPrf.HMACSHA256,
                _iterationsCount,
                128 / 8,
                256 / 8
            );
        }

        private static byte[] HashPasswordInternal(
            string password,
            RandomNumberGenerator randomNumberGenerator,
            KeyDerivationPrf pseudoRandomFunction,
            int iterationsCount,
            int saltSize,
            int requestedBytesCount
            )
        {
            // Produce a text hash.
            var salt = new byte[saltSize];
            randomNumberGenerator.GetBytes(salt);
            var derivedKey = KeyDerivation.Pbkdf2(
                password,
                salt,
                pseudoRandomFunction,
                iterationsCount,
                requestedBytesCount
            );

            var outputBytes = new byte[13 + salt.Length + derivedKey.Length];
            outputBytes[0] = 0x01; // format marker
            WriteNetworkByteOrder(outputBytes, 1, (uint) pseudoRandomFunction);
            WriteNetworkByteOrder(outputBytes, 5, (uint) iterationsCount);
            WriteNetworkByteOrder(outputBytes, 9, (uint) saltSize);
            Buffer.BlockCopy(
                salt,
                0,
                outputBytes,
                13,
                salt.Length
            );

            Buffer.BlockCopy(
                derivedKey,
                0,
                outputBytes,
                13 + saltSize,
                derivedKey.Length
            );
            return outputBytes;
        }

        private static bool VerifyHashedPasswordInternal(byte[] hashedPassword, string password)
        {
            try
            {
                // Read header information
                var pseudoRandomFunction = (KeyDerivationPrf) ReadNetworkByteOrder(hashedPassword, 1);
                var iterationsCount = (int) ReadNetworkByteOrder(hashedPassword, 5);
                var saltLength = (int) ReadNetworkByteOrder(hashedPassword, 9);

                // Read the salt: must be >= 128 bits
                if (saltLength < 128 / 8)
                {
                    return false;
                }

                var salt = new byte[saltLength];
                Buffer.BlockCopy(
                    hashedPassword,
                    13,
                    salt,
                    0,
                    salt.Length
                );

                // Read the PBKDF2 derived key (the rest of the payload): must be >= 128 bits
                var derivedKeyLength = hashedPassword.Length - 13 - salt.Length;
                if (derivedKeyLength < 128 / 8)
                {
                    return false;
                }

                var expectedDerivedKey = new byte[derivedKeyLength];
                Buffer.BlockCopy(
                    hashedPassword,
                    13 + salt.Length,
                    expectedDerivedKey,
                    0,
                    expectedDerivedKey.Length
                );

                // Hash the incoming password and verify it
                var actualDerivedKey = KeyDerivation.Pbkdf2(
                    password,
                    salt,
                    pseudoRandomFunction,
                    iterationsCount,
                    derivedKeyLength
                );
                return ByteArraysEqual(actualDerivedKey, expectedDerivedKey);
            }
            catch
            {
                // This should never occur except in the case of a malformed payload, where
                // we might go off the end of the array. Regardless, a malformed payload
                // implies verification failed.
                return false;
            }
        }

        /// <summary>
        ///     Compares two byte arrays for equality. The method is specifically written so that the loop is not optimized.
        /// </summary>
        /// <param name="a">First array.</param>
        /// <param name="b">Second array.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (a == null && b == null)
            {
                return true;
            }

            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }

            var areSame = true;
            for (var i = 0; i < a.Length; i++)
            {
                areSame &= a[i] == b[i];
            }

            return areSame;
        }


        [SuppressMessage("ReSharper", "RedundantCast")]
        [SuppressMessage("ReSharper", "ArrangeRedundantParentheses")]
        private static uint ReadNetworkByteOrder(byte[] buffer, int offset)
        {
            return ((uint) (buffer[offset + 0]) << 24)
                   | ((uint) (buffer[offset + 1]) << 16)
                   | ((uint) (buffer[offset + 2]) << 8)
                   | ((uint) (buffer[offset + 3]));
        }

        [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
        private static void WriteNetworkByteOrder(
            byte[] buffer,
            int offset,
            uint value
            )
        {
            buffer[offset + 0] = (byte) (value >> 24);
            buffer[offset + 1] = (byte) (value >> 16);
            buffer[offset + 2] = (byte) (value >> 8);
            buffer[offset + 3] = (byte) (value >> 0);
        }
    }
}