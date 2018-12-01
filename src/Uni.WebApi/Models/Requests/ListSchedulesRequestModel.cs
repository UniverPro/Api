using System;

namespace Uni.WebApi.Models.Requests
{
    public class ListSchedulesRequestModel
    {
        /// <summary>
        ///     Filters results by start time from date if value set
        /// </summary>
        public DateTime? DateFrom { get; set; }

        /// <summary>
        ///     Filters results by start time to date if value set
        /// </summary>
        public DateTime? DateTo { get; set; }

        /// <summary>
        ///     Filters results by duration if value set
        /// </summary>
        public TimeSpan? Duration { get; set; }

        /// <summary>
        ///     Filters results by audience number if value set
        /// </summary>
        public string AudienceNumber { get; set; }

        /// <summary>
        ///     Filters results by lesson type if value set
        /// </summary>
        public string LessonType { get; set; }

        /// <summary>
        ///     Filters results by subject if value set
        /// </summary>
        public int? SubjectId { get; set; }

        /// <summary>
        ///     Filters results by teacher if value set
        /// </summary>
        public int? TeacherId { get; set; }
    }
}
