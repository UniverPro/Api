using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using LinqBuilder;
using LinqBuilder.Core;
using LinqBuilder.OrderBy;
using Microsoft.EntityFrameworkCore;
using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Infrastructure.CQRS.Queries.Schedules.FindSchedules
{
    public class FindSchedulesQuery : IQuery<IEnumerable<Schedule>>
    {
        public FindSchedulesQuery(
            DateTime? startTime,
            TimeSpan? duration,
            [CanBeNull] string audienceNumber,
            [CanBeNull] string lessonType,
            int? subjectId,
            int? teacherId
            )
        {
            StartTime = startTime;
            Duration = duration;
            AudienceNumber = audienceNumber;
            LessonType = lessonType;
            SubjectId = subjectId;
            TeacherId = teacherId;
        }

        public DateTime? StartTime { get; }

        public TimeSpan? Duration { get; }

        public string AudienceNumber { get; }

        public string LessonType { get; }

        public int? SubjectId { get; }

        public int? TeacherId { get; }


        [NotNull]
        public ISpecification<Schedule> ToSpecification()
        {
            var specification = Spec<Schedule>.New();

            if (SubjectId != null)
            {
                var subjectId = SubjectId.Value;
                specification = specification.And(Spec<Schedule>.New(x => x.SubjectId == subjectId));
            }

            if (TeacherId != null)
            {
                var teacherId = TeacherId.Value;
                specification = specification.And(Spec<Schedule>.New(x => x.TeacherId == teacherId));
            }

            if (!string.IsNullOrEmpty(LessonType))
            {
                specification = specification.And(
                    Spec<Schedule>.New(x => EF.Functions.Like(x.LessonType, $"%{LessonType}%"))
                );
            }

            if (!string.IsNullOrEmpty(AudienceNumber))
            {
                specification = specification.And(
                    Spec<Schedule>.New(x => EF.Functions.Like(x.AudienceNumber, $"%{AudienceNumber}%"))
                );
            }

            if (Duration != null)
            {
                var duration = Duration.Value;
                specification = specification.And(Spec<Schedule>.New(x => x.Duration == duration));
            }

            if (StartTime != null)
            {
                var startTime = StartTime.Value;
                specification = specification.And(Spec<Schedule>.New(x => x.StartTime == startTime));
            }

            specification = specification.OrderBy(OrderSpec<Schedule, DateTime>.New(x => x.StartTime));
            
            return specification;
        }
    }
}
