using System;
using JetBrains.Annotations;

namespace Uni.Common.Utilities
{
    public static class ExceptionUtilities
    {
        /// <summary>
        ///     Возвращает сообщение об ошибке из последнего исключения в цепочке исключений.
        /// </summary>
        /// <param name="exception">"Корневое" исключение.</param>
        /// <returns></returns>
        public static string GetExceptionMessage([NotNull] Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            var lastException = UnrollException(exception);
            return lastException.Message;
        }

        /// <summary>
        ///     Метод для "размотки" исключений.
        /// </summary>
        /// <param name="exception">"Родительское" исключение.</param>
        /// <returns></returns>
        [NotNull]
        public static Exception UnrollException([NotNull] Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            var currentException = exception;
            while (currentException.InnerException != null)
            {
                currentException = currentException.InnerException;
            }

            return currentException;
        }
    }
}
