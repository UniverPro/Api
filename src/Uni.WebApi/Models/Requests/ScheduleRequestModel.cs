using System;
using Newtonsoft.Json;

namespace Uni.WebApi.Models.Requests
{
    [JsonObject]
    public class ScheduleRequestModel
    {
        public int SubjectId { get; set; }

        public DateTime StartTime { get; set; }

        public TimeSpan Duration { get; set; }
    }
}
