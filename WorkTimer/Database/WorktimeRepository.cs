using SQLite;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

        public async Task<List<WorktimeDay>> ListAllWorktime()
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

        public async Task<List<WorktimeAggregatedByWeek>> GetAggregatedWorktimeByWeek()
        {
            try
            {
                await Init();

                Settings settings = await App.WorktimeRepo.GetSettings();

                var worktimeEntries = await conn.Table<WorktimeDay>().ToListAsync();

                var groupedData = worktimeEntries
                    .GroupBy(x => GetWeekStart(x.Date))
                    .Select(group => new WorktimeAggregatedByWeek
                    {
                        WeekStartDate = group.Key,
                        WeekEndDate = group.Key.AddDays(6),
                        CalendarWeek = GetCalendarWeek(group.Key),
                        TotalWorkTime = TimeSpan.FromTicks(group.Sum(x => x.WorkTime.Ticks)),
                        TotalPauseTime = TimeSpan.FromTicks(group.Sum(x => x.PauseTime.Ticks)),
                        TotalAbsentCount = group.Count(x => x.Absent),
                        Overtime = CalculateOvertime(TimeSpan.FromTicks(group.Sum(x => x.WorkTime.Ticks)), settings.WorktimeWeeklyHours)
                    })
                    .OrderBy(x => x.WeekStartDate)
                    .ToList();

                return groupedData;
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
            }

            return new List<WorktimeAggregatedByWeek>();
        }

        private DateTime GetWeekStart(DateTime date)
        {
            int daysUntilSunday = (int)date.DayOfWeek - (int)DayOfWeek.Sunday;
            if (daysUntilSunday < 0)
                daysUntilSunday += 7;
            return date.Date.AddDays(-daysUntilSunday);
        }

        private int GetCalendarWeek(DateTime date)
        {
            // Calculate the calendar week based on the provided date
            var calendar = CultureInfo.InvariantCulture.Calendar;
            return calendar.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
        }

        private TimeSpan CalculateOvertime(TimeSpan totalWorkTime, float standardWorkWeekHours)
        {
            return totalWorkTime - TimeSpan.FromHours(standardWorkWeekHours);
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
