namespace Uni.Identity.Web.ViewModels.Error
{
    /// <summary>
    ///     Модель представления ошибки приложения.
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        ///     Конструирует модель представления ошибки приложения.
        /// </summary>
        /// <param name="message">Текст сообщения об ошибке.</param>
        /// <param name="description">Подробное описание ошибки.</param>
        /// <param name="requestId">Уникальный идентификатор запроса, вызвавшего ошибку.</param>
        public ErrorViewModel(
            string message,
            string description = null,
            string requestId = null
            )
        {
            Message = message;
            Description = description;
            RequestId = requestId;
        }

        /// <summary>
        ///     Текст сообщения об ошибке.
        /// </summary>
        public string Message { get; }

        /// <summary>
        ///     Подробное описание ошибки.
        /// </summary>
        public string Description { get; }

        /// <summary>
        ///     Уникальный идентификатор запроса, вызвавшего ошибку.
        /// </summary>
        public string RequestId { get; }
    }
}
