using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System.Collections.ObjectModel;
using System.Diagnostics;
using WorkTimer.Models;
using WorkTimer.Util;
using WorkTimer.View;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using LiveChartsCore.SkiaSharpView.Painting.Effects;

namespace WorkTimer.ViewModel
{
    public partial class AnalyticsViewModel : ObservableObject
    {
        private readonly Task initTask;
        public AnalyticsViewModel()
        {
            this.initTask = LoadWorktime();

        }

        [ObservableProperty]
        ObservableCollection<WorktimeAggregatedByWeek> worktimeAggregatedList = new ObservableCollection<WorktimeAggregatedByWeek>();

        [ObservableProperty]
        ISeries[] series;

        public Axis[] XAxes { get; set; }
            = new Axis[]
            {
                new Axis
                {
                    Name = "KW",
                    MinStep = 1
                }
            };

        public Axis[] YAxes { get; set; }
            = new Axis[]
            {
                new Axis
                {
                    Name = "Stunden"
                }
            };

        [ObservableProperty]
        public RectangularSection[] sections;        

        async private Task LoadWorktime()
        {
            TimeSpan overtime = TimeSpan.Zero;
            List<WorktimeAggregatedByWeek> result = await App.WorktimeRepo.GetAggregatedWorktimeByWeek();
            Trace.WriteLine("Get finished: " + result.Count);
            if (result.Count > 0)
            {
                WorktimeAggregatedList.Clear();
                foreach (WorktimeAggregatedByWeek day in result)
                {
                    WorktimeAggregatedList.Add(day);
                    overtime = overtime.Add(day.Overtime);
                }

                Series = new ISeries[]
                {
                    new StackedColumnSeries<WorktimeAggregatedByWeek>
                    {
                        Values = result,
                        Mapping = (value, point) =>
                        {
                            point.Coordinate = new (value.CalendarWeek, value.TotalWorkTime.TotalHours);
                        },
                        Stroke = null,
                        Name = "Arbeitszeit",
                        MaxBarWidth = 40,
                        IgnoresBarPosition = true
                    },
                    new StackedColumnSeries<WorktimeAggregatedByWeek>
                    {
                        Values = result,
                        Mapping = (value, point) =>
                        {
                            point.Coordinate = new (value.CalendarWeek, value.TotalAbsentHours.TotalHours);
                        },
                        Stroke = null,
                        Name = "Abwesenheit",
                        MaxBarWidth = 40,
                        IgnoresBarPosition = true
                    }
                };

                Settings settings = await App.WorktimeRepo.GetSettings();

                Sections = new RectangularSection[]
                {
                    new RectangularSection
                    {
                        Yi = settings.WorktimeWeeklyHours,
                        Yj = settings.WorktimeWeeklyHours,
                        Stroke = new SolidColorPaint
                        {
                            Color = SKColors.Gray,
                            StrokeThickness = 3,
                            PathEffect = new DashEffect(new float[] { 6, 6 })
                        }
                    }
                };
            }
        }

        [RelayCommand]
        async Task Back()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
