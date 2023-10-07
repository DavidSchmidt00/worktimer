using SQLite;

namespace WorkTimer.Models
{
    public class WorktimeAggregatedByWeek
    {
        public DateTime WeekStartDate { get; set; }
        public DateTime WeekEndDate { get; set; }
        public int CalendarWeek { get; set; }
        public TimeSpan TotalWorkTime { get; set; }
        public TimeSpan TotalPauseTime { get; set; }
        public int TotalAbsentCount { get; set; }
        public TimeSpan Overtime { get; set; }
    }
}
