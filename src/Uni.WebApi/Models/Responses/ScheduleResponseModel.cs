using System;
using Newtonsoft.Json;

namespace Uni.WebApi.Models.Responses
{
    [JsonObject]
    public class ScheduleResponseModel
    {
        public int Id { get; set; }

        public int SubjectId { get; set; }

        public int TeacherId { get; set; }

        public DateTime StartTime { get; set; }

        public TimeSpan Duration { get; set; }

        public int AudienceNumber { get; set; }
    }
}
