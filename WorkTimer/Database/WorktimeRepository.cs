using SQLite;
using WorkTimer.Models;

namespace WorkTimer.Database
{
    public class WorktimeRepository
    {
        string _dbPath;

        private SQLiteAsyncConnection conn;
        
        public string StatusMessage { get; set; }
        
        private async Task Init()
        {
            if (conn != null)
                return;
            
            conn = new SQLiteAsyncConnection(_dbPath);
            await conn.CreateTableAsync<WorktimeDay>();
        }

        public WorktimeRepository(string dbPath)
        {
            _dbPath = dbPath;
        }

        public async Task AddNewWorktimeDay(WorktimeDay worktime_day)
        {
            int result = 0;
            try
            {
                await Init();

                result = await conn.InsertAsync(worktime_day);

                StatusMessage = string.Format("{0} record(s) added", result);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add worktime. Error: {1}", ex.Message);
            }

        }

        public async Task<List<WorktimeDay>> GetAllWorktime()
        {
            try
            {
                await Init();
                return await conn.Table<WorktimeDay>().ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
            }

            return new List<WorktimeDay>();
        }
    }
}
