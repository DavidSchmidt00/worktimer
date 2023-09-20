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
            await conn.CreateTableAsync<Settings>();
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

        public async Task UpdateWorktimeDay(WorktimeDay worktime_day)
        {
            int result = 0;
            try
            {
                await Init();

                result = await conn.UpdateAsync(worktime_day);

                StatusMessage = string.Format("{0} record updated", result);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to update worktime. Error: {1}", ex.Message);
            }

        }

        public async Task DeleteWorktimeDay(WorktimeDay worktime_day)
        {
            int result = 0;
            try
            {
                await Init();

                result = await conn.DeleteAsync(worktime_day);

                StatusMessage = string.Format("{0} record deleted", result);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to deleted worktime. Error: {1}", ex.Message);
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

        public async Task UpdateSettings(Settings settings)
        {
            int result = 0;
            try
            {
                await Init();
                if (await GetSettings() is not null)
                    result = await conn.UpdateAsync(settings);
                else
                    result = await conn.InsertAsync(settings);

                StatusMessage = string.Format("{0} record(s) added/updated", result);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add worktime. Error: {1}", ex.Message);
            }

        }

        public async Task<Settings> GetSettings()
        {
            try
            {
                await Init();
                return await conn.Table<Settings>().FirstAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
            }

            return null;
        }
    }
}
