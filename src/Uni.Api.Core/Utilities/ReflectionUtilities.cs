using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Uni.Api.Core.Utilities
{
    public static class ReflectionUtilities
    {
        /// <summary>
        ///     Return all the values of constants of the specified type
        /// </summary>
        /// <typeparam name="TField">What type of constants to return</typeparam>
        /// <param name="type">Type to examine</param>
        /// <returns>List of constant values</returns>
        [NotNull]
        [ItemNotNull]
        public static FieldInfo[] GetConstantFields<TField>([NotNull] Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            return fieldInfos
                .Where(x => x.IsLiteral && !x.IsInitOnly && x.FieldType == typeof(TField))
                .ToArray();
        }
    }
}
