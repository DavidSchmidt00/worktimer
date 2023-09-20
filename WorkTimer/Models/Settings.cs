using SQLite;

namespace WorkTimer.Models
{
    [Table("Settings")]
    public class Settings
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public float WorktimeWeeklyHours { get; set; }

        public int VacationDays { get; set; }

        public bool StandingDesk { get; set; }
    }
}
