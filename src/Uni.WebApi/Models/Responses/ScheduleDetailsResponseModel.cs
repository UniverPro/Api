using System;
using Newtonsoft.Json;

namespace Uni.WebApi.Models.Responses
{
    [JsonObject]
    public class ScheduleDetailsResponseModel
    {
        public int Id { get; set; }

        public SubjectResponseModel Subject { get; set; }

        public TeacherResponseModel Teacher { get; set; }

        public DateTime StartTime { get; set; }

        public TimeSpan Duration { get; set; }

        public string LessonType { get; set; }

        public string AudienceNumber { get; set; }
    }
}