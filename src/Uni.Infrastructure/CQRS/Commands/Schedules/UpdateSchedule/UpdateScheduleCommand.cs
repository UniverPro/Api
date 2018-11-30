using System;
using Uni.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Infrastructure.CQRS.Commands.Schedules.UpdateSchedule
{
    public class UpdateScheduleCommand : ICommand
    {
        public UpdateScheduleCommand(
            int id,
            int subjectId,
            int teacherId,
            DateTime startTime,
            TimeSpan duration,
            string lessonType,
            string audienceNumber
            )
        {
            Id = id;
            SubjectId = subjectId;
            TeacherId = teacherId;
            StartTime = startTime;
            Duration = duration;
            LessonType = lessonType;
            AudienceNumber = audienceNumber;
        }

        public int Id { get; }

        public int SubjectId { get; }

        public int TeacherId { get; }

        public DateTime StartTime { get; }

        public TimeSpan Duration { get; }

        public string LessonType { get; }

        public string AudienceNumber { get; }
    }
}
