using System.Collections.Generic;

using NodaTime;
using NodaTime.Text;

using GGNet.Transformations;
using GGNet.Formats;

namespace GGNet.Scales
{
    public class DateTimeDiscretePosition : DiscretePosition<LocalDateTime>
    {
        private static readonly LocalTimePattern timePattern = LocalTimePattern.CreateWithInvariantCulture("HH:mm");
        private static readonly LocalDatePattern datePattern = LocalDatePattern.CreateWithInvariantCulture("MM/dd");

        public DateTimeDiscretePosition(ITransformation<LocalDateTime> transformation = null,
            (LocalDateTime? min, LocalDateTime? max)? limits = null,
            (double minMult, double minAdd, double maxMult, double maxAdd)? expand = null,
            IFormatter<LocalDateTime> formatter = null)
            : base(transformation, limits, expand, formatter)
        {
        }

        public override Guide Guide => Guide.None;

        protected override void Labeling()
        {
            var breaks = new List<double>();
            var minor = new List<double>();
            var labels = new List<(double x, string label)>();
            var titles = new List<(double x, string title)>();

            var dfirst = -1;
            var dlast = -1;

            var day = new LocalDate();

            for (int i = 0; i < values.Count; i++)
            {
                var date = values[i];

                if (date.Date != day)
                {
                    if (dlast >= 0 && dlast != dfirst)
                    {
                        titles.Add(((dlast + dfirst) / 2.0, datePattern.Format(day)));
                    }

                    day = date.Date;

                    dfirst = i;
                }

                if (date.Minute % 30 == 0)
                {
                    breaks.Add(i);
                    labels.Add((i, timePattern.Format(date.TimeOfDay)));
                }
                else if (date.Minute % 15 == 0)
                {
                    minor.Add(i);
                }

                values.Add(date);

                dlast = i;
            }

            if (dlast >= 0 && dlast != dfirst)
            {
                titles.Add(((dlast + dfirst) / 2.0, datePattern.Format(day)));
            }

            Breaks = breaks;
            MinorBreaks = minor;
            Labels = labels;
            Titles = titles;
        }
    }
}
