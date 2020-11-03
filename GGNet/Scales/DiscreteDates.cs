using System.Collections.Generic;

using NodaTime;

using GGNet.Transformations;

namespace GGNet.Scales
{
    public class DiscretDates : DiscretePosition<LocalDate>
    {
        private static readonly string[] Abbreviations = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

        public DiscretDates(ITransformation<LocalDate> transformation = null,
            (LocalDate? min, LocalDate? max)? limits = null,
            (double minMult, double minAdd, double maxMult, double maxAdd)? expand = null)
            : base(transformation, limits, expand)
        {
        }

        protected void DayMonth(int start, int end)
        {
            /*
            var delta = (int)Pretty.delta(start, end);

            if (delta == 0)
            {
                return;
            }
            */

            var delta = (end - start + 1) switch
            {
                var _n when _n <= 7 => 1,
                var _n when _n <= 14 => 2,
                var _n when _n <= 35 => 5,
                var _n when _n < 70 => 10,
                _ => 15
            };

            var breaks = new List<double>();
            var labels = new List<(double x, string day)>();
            var titles = new List<(double x, string month)>();

            var mfirst = -1;
            int? month = null;

            var j = 1;
            for (int i = start; i < end; i++)
            {
                var date = values[i];

                if (date.Month != month)
                {
                    if (month.HasValue)
                    {
                        titles.Add(((i - 1 + mfirst) / 2.0, Abbreviations[month.Value - 1]));
                    }

                    month = date.Month;
                    mfirst = i;
                }

                if (j++ % delta == 0)
                {
                    labels.Add((i, date.Day.ToString()));
                    breaks.Add(i);
                }
            }

            if (month.HasValue)
                titles.Add(((end - 1 + mfirst) / 2.0, Abbreviations[month.Value - 1]));

            Breaks = breaks;
            Labels = labels;
            Titles = titles;
        }

        protected void MonthYear(int start, int end)
        {
            var yfirst = -1;
            var mfirst = -1;

            int? year = null;
            int? month = null;

            var labels = new List<(double x, string month)>();
            var breaks = new List<double>();
            var titles = new List<(double x, string year)>();

            for (int i = start; i < end; i++)
            {
                var date = values[i];

                if (date.Year != year)
                {
                    if (year.HasValue)
                        titles.Add(((i - 1 + yfirst) / 2.0, year.ToString()));

                    year = date.Year;
                    yfirst = i;
                }

                if (date.Month != month)
                {
                    if (month.HasValue)
                    {
                        var label = Abbreviations[month.Value - 1];

                        labels.Add(((i - 1 + mfirst) / 2.0, label));
                        breaks.Add(i - 1);
                    }

                    month = date.Month;
                    mfirst = i;
                }
            }

            titles.Add(((end - 1 + yfirst) / 2.0, year.ToString()));
            labels.Add(((end - 1 + mfirst) / 2.0, Abbreviations[month.Value - 1]));

            Breaks = breaks;
            Labels = labels;
            Titles = titles;
        }

        protected void QuarterYear(int start, int end)
        {
            var yfirst = -1;

            int? year = null;
            int? month = null;

            var breaks = new List<double>();
            var labels = new List<(double x, string quarter)>();
            var titles = new List<(double x, string year)>();

            for (int i = start; i < end; i++)
            {
                var date = values[i];

                if (date.Year != year)
                {
                    if (year.HasValue)
                        titles.Add(((i - 1 + yfirst) / 2.0, year.ToString()));

                    year = date.Year;
                    yfirst = i;
                }

                if (date.Month != month)
                {
                    if (month.HasValue)
                    {
                        var label = Abbreviations[month.Value - 1];

                        if (month % 3 == 0)
                        {
                            labels.Add((i - 1, label));
                            breaks.Add(i - 1);
                        }
                    }

                    month = date.Month;
                }
            }

            titles.Add(((end - 1 + yfirst) / 2.0, year.ToString()));

            Breaks = breaks;
            Labels = labels;
            Titles = titles;
        }

        protected override void Labeling(int start, int end)
        {
            var n = end - start;

            if (n <= 128)
            {
                DayMonth(start, end);
            }
            else if (n <= 384)
            {
                MonthYear(start, end);
            }
            else
            {
                QuarterYear(start, end);
            }
        }
    }
}
