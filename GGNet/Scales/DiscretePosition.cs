using GGNet.Transformations;
using GGNet.Formats;
using System;

namespace GGNet.Scales
{
    public class DiscretePosition<T> : Position<T>
        where T : struct
    {
        protected readonly SortedBuffer<T> values = new SortedBuffer<T>(16, 1);

        protected readonly IFormatter<T> formatter;

        public DiscretePosition(ITransformation<T> transformation = null,
            (T? min, T? max)? limits = null,
            (double minMult, double minAdd, double maxMult, double maxAdd)? expand = null,
            IFormatter<T> formatter = null)
            : base(transformation, expand ?? (0.0, 0.6, 0, 0.6), limits)
        {
            this.formatter = formatter ?? Standard<T>.Instance;
        }

        public override Guide Guide => Guide.None;

        public override void Train(T key)
        {
            (T? min, T? max) limits = (null, null);

            if (Limits != null)
                limits = Limits.Invoke();

            if ((!limits.min.HasValue || !(limits.min is IComparable) || (limits.min is IComparable min && min.CompareTo(key) <= 0))
                && (!limits.max.HasValue || !(limits.max is IComparable) || (limits.max is IComparable max && max.CompareTo(key) >= 0)))
            {
                values.Add(key);
            }
        }

        protected override void Labeling()
        {
            var breaks = new double[values.Count];
            var labels = new (double value, string label)[values.Count];

            for (var i = 0; i < values.Count; i++)
            {
                breaks[i] = i;
                labels[i] = (i, formatter.Format(values[i]));
            }

            Breaks = breaks;
            Labels = labels;
        }

        public override double Map(T key, bool ignoreLimits = false)
        {
            var index = values.IndexOf(key);
            if (index < 0)
            {
                return double.NaN;
            }

            return index;
        }

        public override void Clear()
        {
            base.Clear();

            values.Clear();
        }
    }
}
