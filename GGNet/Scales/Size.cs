using System.Linq;
using GGNet.Transformations;

using static System.Math;

namespace GGNet.Scales
{
    public class SizeContinuous : Scale<double, double>
    {
        protected readonly bool defined;
        protected (double min, double max)? limits;
        protected readonly (double min, double max) range;
        protected readonly bool oob;
        private readonly string format;

        public SizeContinuous(
            (double min, double max)? limits = null,
            (double min, double max)? range = null,
            bool oob = false,
            string format = "0.##",
            ITransformation<double> transformation = null)
            : base(transformation)
        {
            if (limits.HasValue)
            {
                defined = true;
                this.limits = limits;
            }

            this.range = range ?? (0.0, 1.0);
            this.oob = oob;

            this.format = format;
        }

        public override Guide Guide => Guide.Items;

        public override void Train(double key)
        {
            if (defined)
            {
                return;
            }

            if (!limits.HasValue)
            {
                limits = (key, key);
            }
            else
            {
                limits = (Min(limits.Value.min, key), Max(limits.Value.max, key));
            }
        }

        public override void Set(bool grid)
        {
            if (!grid)
                return;

            var breaks = Wilkinson.extended(limits.Value.min, limits.Value.max);

            var labels = new (double value, string label)[breaks.Length];

            for (var i = 0; i < breaks.Length; i++)
            {
                var value = breaks[i];

                labels[i] = (Map(value), value.ToString(format));
            }

            Breaks = breaks;
            Labels = labels;
        }

        public override double Map(double key, bool ignoreLimits = false)
        {
            if (!oob && !ignoreLimits)
            {
                if (key < limits.Value.min || key > limits.Value.max)
                {
                    return double.NaN;
                }
            }

            if (!ignoreLimits && key < limits.Value.min)
            {
                return range.min;
            }
            else if (!ignoreLimits && key > limits.Value.max)
            {
                return range.max;
            }

            if (limits.Value.min >= 0)
            {
                return range.min + (Sqrt(key) - Sqrt(limits.Value.min)) / (Sqrt(limits.Value.max) - Sqrt(limits.Value.min)) * (range.max - range.min);
            }
            else
            {
                return range.min + Sqrt(key - limits.Value.min) / Sqrt(limits.Value.max - limits.Value.min) * (range.max - range.min);
            }
        }

        public override void Clear()
        {
            if (defined)
                return;

            limits = null;
        }
    }
}
