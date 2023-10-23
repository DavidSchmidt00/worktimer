using SQLite;

namespace WorkTimer.Models
{
    public class WorktimeAggregatedByWeek
    {
        public int CalendarWeek { get; set; }
        public TimeSpan TotalWorkTime { get; set; }
        public TimeSpan TotalPauseTime { get; set; }
        public int TotalAbsentCount { get; set; }
        public TimeSpan TotalAbsentHours { get; set; }
        public TimeSpan Overtime { get; set; }
    }
}
