using SQLite;

namespace WorkTimer.Models
{
    public class WorktimeAggregatedByDay
    {
        public DateTime Date { get; set; }
        public TimeSpan TotalWorkTime { get; set; }
        public TimeSpan TotalPauseTime { get; set; }
        public bool Absent { get; set; }
    }
}
