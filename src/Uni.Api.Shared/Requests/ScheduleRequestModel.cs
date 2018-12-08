using System;
using JetBrains.Annotations;

namespace Uni.Api.Shared.Requests
{
    [PublicAPI]
    public class ScheduleRequestModel
    {
        public int SubjectId { get; set; }

        public int TeacherId { get; set; }

        public DateTime StartTime { get; set; }

        public TimeSpan Duration { get; set; }

        public string LessonType { get; set; }

        public string AudienceNumber { get; set; }
    }
}
