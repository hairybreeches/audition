using System;
using System.Linq;
using Model.Accounting;
using Model.Time;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NodaTime;
using Persistence;

namespace Searching.SearchWindows
{
    public class WorkingHoursParameters : ISearchParameters
    {
        public WorkingHoursParameters(DayOfWeek fromDay, DayOfWeek toDay, LocalTime fromTime, LocalTime toTime)
        {
            if (toTime < fromTime)
            {
                throw new InvalidTimeFrameException("The 'from' time must be before the 'to' time");
            }
            ToTime = toTime;
            FromTime = fromTime;
            ToDay = toDay;
            FromDay = fromDay;
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public DayOfWeek FromDay { get; private set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public DayOfWeek ToDay { get; private set; }
        public LocalTime FromTime { get; private set; }
        public LocalTime ToTime { get; private set; }


        public IQueryable<Transaction> ApplyFilter(Searcher searcher, IQueryable<Transaction> transactions)
        {
            return searcher.FindTransactionsWithin(this, transactions);
        }

        public bool Contains(DateTimeOffset ukCreationTime)
        {            
            return DayWithinRange(this, ukCreationTime) && TimeWithinRange(ukCreationTime);
        }

        private bool TimeWithinRange(DateTimeOffset ukCreationTime)
        {
            var journalCreationTime = new LocalTime(ukCreationTime.Hour, ukCreationTime.Minute, ukCreationTime.Second);

            return journalCreationTime <= ToTime
                   && journalCreationTime >= FromTime;
        }

        private static bool DayWithinRange(WorkingHoursParameters workingHours, DateTimeOffset date)
        {
            var creationDay = (int)date.DayOfWeek;

            var fromDay = (int)workingHours.FromDay;
            var toDay = (int)workingHours.ToDay;

            if (toDay <= fromDay)
            {
                toDay += 7;
            }

            if (creationDay < fromDay)
            {
                creationDay += 7;
            }


            return creationDay <= toDay;
        }


        public override string ToString()
        {
            return String.Format("posted outside {0} to {1}, {2} to {3}", FromDay, ToDay, LocalTimeExtensions.ToShortString(FromTime), LocalTimeExtensions.ToShortString(ToTime));
        }

        protected bool Equals(WorkingHoursParameters other)
        {
            return FromDay == other.FromDay && ToDay == other.ToDay && FromTime.Equals(other.FromTime) && ToTime.Equals(other.ToTime);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((WorkingHoursParameters) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) FromDay;
                hashCode = (hashCode*397) ^ (int) ToDay;
                hashCode = (hashCode*397) ^ FromTime.GetHashCode();
                hashCode = (hashCode*397) ^ ToTime.GetHashCode();
                return hashCode;
            }
        }
    }
}