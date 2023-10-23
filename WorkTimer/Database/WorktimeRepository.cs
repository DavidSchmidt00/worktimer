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
        
        private async Task Init() // Bei Bedarf Verbindung zur Datenbank aufbauen und Tabellen erstellen, falls sie noch nicht existieren
        {
            // Wenn Verbindung bereits besteht, nichts tun
            if (conn != null)
                return;
            
            conn = new SQLiteAsyncConnection(_dbPath);
            await conn.CreateTableAsync<WorktimeDay>();
            await conn.CreateTableAsync<Settings>();
        }

        public WorktimeRepository(string dbPath) // Übergabeparameter kommt aus MauiProgramm.cs
        {
            _dbPath = dbPath;
        }

        public async Task AddNewWorktimeDay(WorktimeDay worktime_day) // Neuen Eintrag in Datenbank schreiben
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

        public async Task UpdateWorktimeDay(WorktimeDay worktime_day) // Eintrag in Datenbank updaten
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

        public async Task DeleteWorktimeDay(WorktimeDay worktime_day) // Eintrag aus Datenbank löschen
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

        public async Task<List<WorktimeDay>> ListAllWorktime() // Alle Einträge der Tabelle WorktimeDay zurückgeben
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

        public async Task<List<WorktimeAggregatedByWeek>> GetAggregatedWorktimeByWeek() // Alle Einträge der Tablle WorktimeDay nach Kalenderwochen aggregieren und zurückgeben
        {
            try
            {
                await Init();

                Settings settings = await App.WorktimeRepo.GetSettings();

                // Alle Einträge aus Tabelle laden
                var worktimeEntries = await conn.Table<WorktimeDay>().ToListAsync();

                // Einträge nach Kalenderwoche groupieren und neue Werte berechnen oder aggregieren
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
                        TotalAbsentHours = TimeSpan.FromHours(group.Count(x => x.Absent) * (settings.WorktimeWeeklyHours / 5)),
                        Overtime = CalculateOvertime(TimeSpan.FromTicks(group.Sum(x => x.WorkTime.Ticks)), settings.WorktimeWeeklyHours, group.Count(x => x.Absent))
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

        private int GetCalendarWeek(DateTime date) // Kalenderwoche zu Datum berechnen
        {
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
        }

        private TimeSpan CalculateOvertime(TimeSpan totalWorkTime, float workWeekHours, int totalAbsentCount) // Über-/Unterstunden unter Berücksichtigung der Abwesenheiten
        {
            TimeSpan correctedWorkWeekHours = TimeSpan.FromHours(Math.Round(workWeekHours - (totalAbsentCount * (workWeekHours / 5)),1));
            return totalWorkTime - correctedWorkWeekHours;
        }

        public async Task UpdateSettings(Settings settings)
        /*
         Settings in Datenbank schreiben oder wenn vorhanden updaten.
         Es soll immer nur genau einen Eintrag in dieser Tabelle geben.
        */
        {
            int result = 0;
            try
            {
                await Init();
                if (await GetSettings() is not null) // Wenn es schon einen Eintrag gibt, diesen updaten
                    result = await conn.UpdateAsync(settings);
                else // Wenn es keinen Eintrag gibt, neuen anlegen
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
