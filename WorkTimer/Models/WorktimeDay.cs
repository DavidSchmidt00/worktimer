using SQLite;

namespace WorkTimer.Models
{
    [Table("Worktime")]
    public class WorktimeDay
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime Date { get; set; }
        
        public TimeSpan WorkTime { get; set; }

        public TimeSpan PauseTime { get; set; }

        public bool Absent { get; set; }
    }
}
