using System.Collections.Generic;

using GGNet.Transformations;
using System;

namespace GGNet.Scales
{
    public class DateTimePosition : ContinuousPosition<DateTime>
    {
        public DateTimePosition(ITransformation<DateTime> transformation = null,
            (DateTime? min, DateTime? max)? limits = null,
            (DateTime? min, DateTime? max)? expandLimits = null,
            (double minMult, double minAdd, double maxMult, double maxAdd)? expand = null)
            : base(transformation, expand ?? (0.0, 0.0, 0, 0.0), limits, expandLimits)
        {
        }

        public override Guide Guide => Guide.None;

        public override void Set(bool grid)
        {
            base.Set(grid);

            if (!grid)
                return;

            var labels = new List<(double x, string label)>();
            var titles = new List<(double x, string title)>();
            var breaks = new List<double>();
            var minor = new List<double>();

            var start = Range.min.ConvertFromDouble();
            var end = Range.max.ConvertFromDouble();
            var diff = end - start;
            var titleStart = TimeSpan.Zero;
            var lastTitleDate = start;
            var span = Math.Floor((end - start).TotalSeconds);
            span = span switch
            {
                _ when span > 4 * 24 * 60 * 60 => span - span % (4 * 24 * 60 * 60),
                _ when span > 24 * 60 * 60 => span - span % (24 * 60 * 60),
                _ when span > 60 * 60 => span - span % (60 * 60),
                _ when span > 60 => span - span % 60,
                _ => span
            };

            double numBreaks = 4;

            if (span > 24 * 60 * 60)
                numBreaks = Math.Min(4, Math.Floor((end - start).TotalSeconds / (24 * 60 * 60)));

            var step = span / (end - start).TotalSeconds / numBreaks;
            var year = new DateTime(start.Year, 1, 1, 0, 0, 0, start.Kind);
            var firstDate = year.AddSeconds(span * Math.Floor((start - year).TotalSeconds / span));
            var firstBreak = (double) (firstDate - start).Ticks / (end - start).Ticks;
            var startWithMinor = false;

            while (firstBreak < 0)
            {
                firstBreak += step / 2;
                startWithMinor = !startWithMinor;
            }

            for (double i = firstBreak; i <= 1; i += step / 2)
            {
                var offset = i * diff;
                var dt = start + offset;
                var bp = dt.ConvertToDouble();

                if ((!startWithMinor && breaks.Count == minor.Count)
                    || (startWithMinor && breaks.Count != minor.Count))
                {
                    breaks.Add(bp);
                    if (diff.TotalDays < 3)
                    {
                        labels.Add((bp, dt.ToLongTimeString()));
                        if (lastTitleDate.Date != dt.Date)
                        {
                            titles.Add(((start + (offset + titleStart) / 2).ConvertToDouble(), lastTitleDate.ToShortDateString()));
                            titleStart = offset;
                            lastTitleDate = dt;
                        }
                    }
                    else
                    {
                        labels.Add((bp, dt.Day.ToString()));

                        if (lastTitleDate.Year != dt.Year || lastTitleDate.Month != dt.Month)
                        {
                            titles.Add(((start + (offset + titleStart) / 2).ConvertToDouble(), lastTitleDate.ToString("Y")));
                            titleStart = offset;
                            lastTitleDate = dt;
                        }
                    }
                }
                else
                {
                    minor.Add(bp);
                }
            }

            titles.Add(((start + (diff + titleStart) / 2).ConvertToDouble(),
                diff.TotalDays < 3 ? lastTitleDate.ToShortDateString() : lastTitleDate.ToString("Y")));

            Breaks = breaks;
            MinorBreaks = minor;
            Labels = labels;
            Titles = titles;
        }

        public override double Map(DateTime key, bool ignoreLimits = false)
        {
            if (transformation != null)
                key = transformation.Apply(key);

            if (!ignoreLimits && Limits != null)
            {
                var (min, max) = Limits.Invoke();

                if (min.HasValue && min.Value > key)
                    return double.NaN;

                if (max.HasValue && max.Value < key)
                    return double.NaN;
            }

            return key.ConvertToDouble();
        }
    }

    public static class DateTimeExtensions 
    {
        public static double ConvertToDouble(this DateTime dt) => dt.ToBinary() / 10000;

        public static DateTime ConvertFromDouble(this double ddt) => DateTime.FromBinary(((long) ddt) * 10000);
    }
}
