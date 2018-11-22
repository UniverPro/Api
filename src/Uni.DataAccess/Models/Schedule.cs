using System;

namespace Uni.DataAccess.Models
{
    public class Schedule : ITableObject
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }

        public Subject IdNavigation { get; set; }
    }
}
