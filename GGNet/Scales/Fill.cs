using GGNet.Transformations;

using static System.Math;

namespace GGNet.Scales
{
    public class FillDiscrete<TKey> : Discrete<TKey, string>
    {
        public FillDiscrete(
           Palettes.Discrete<TKey, string> palette,
            ITransformation<TKey> transformation = null)
           : base(palette, default, transformation)
        {
        }

        public FillDiscrete(
            string[] palette,
            int direction = 1,
            ITransformation<TKey> transformation = null)
            : base(palette, direction, default, transformation)
        {
        }

        public override Guide Guide => Guide.Items;
    }

    public class FillContinuous : Scale<double, string>
    {
        private readonly int m;
        private readonly string format;

        protected (double min, double max) limits = (0.0, 0.0);

        public FillContinuous(
            string[] colors,
            int m = 5,
            string format = "0.##",
            ITransformation<double> transformation = null)
            : base(transformation)
        {
            this.Colors = colors;
            this.m = m;
            this.format = format;
        }

        public string[] Colors { get; }
        public override Guide Guide => Guide.ColorBar;

        public override void Train(double key)
        {
            if (limits.min == 0 && limits.max == 0)
            {
                limits = (key, key);
            }
            else
            {
                limits = (Min(limits.min, key), Max(limits.max, key));
            }
        }

        public override void Set(bool grid)
        {
            if (!grid)
            {
                return;
            }

            var extended = Wilkinson.extended(limits.min, limits.max, m, true);

            var labels = new (string value, string label)[extended.Length];
            var breaks = new string[extended.Length];

            for (var i = 0; i < extended.Length; i++)
            {
                var value = extended[extended.Length - i - 1];
                var mapped = Map(value);

                breaks[i] = mapped;
                labels[i] = (mapped, value.ToString(format));
            }

            Breaks = breaks;
            Labels = labels;
        }

        public override string Map(double key, bool ignoreLimits = false)
        {
            if (limits.max > limits.min)
            {
                double transOffset, transRange;

                if (transformation != null)
                {
                    transOffset = transformation.Apply(key - limits.min);
                    transRange = transformation.Apply(limits.max - limits.min);
                }
                else
                {
                    transOffset = key - limits.min;
                    transRange = limits.max - limits.min;
                }

                var offset = Max(Min(transOffset / transRange, 1), 0) * (Colors.Length - 1);
                var lowOffset = (int) Floor(offset);
                var highOffset = (int) Ceiling(offset);
                var lowColor = Colors[lowOffset];
                var highColor = Colors[highOffset];

                var (lowRed, lowGreen, lowBlue) = ParseColor(lowColor);
                var (highRed, highGreen, highBlue) = ParseColor(highColor);

                if (highOffset == lowOffset)
                    return $"#{lowRed:X2}{lowGreen:X2}{lowBlue:X2}";

                var colorOffset = (offset - lowOffset) / (highOffset - lowOffset);
                var red = (uint) (lowRed + ((double) highRed - lowRed) * colorOffset);
                var green = (uint) (lowGreen + ((double) highGreen - lowGreen) * colorOffset);
                var blue = (uint) (lowBlue + ((double) highBlue - lowBlue) * colorOffset);

                return $"#{red:X2}{green:X2}{blue:X2}";
            }
            else
            {
                return string.Empty;
            }
        }

        public override void Clear()
        {
            limits = (0.0, 0.0);
        }

        private static (uint red, uint green, uint blue) ParseColor(string color)
        {
            if (color.StartsWith("#"))
            {
                return (uint.Parse(color.Substring(1, 2), System.Globalization.NumberStyles.AllowHexSpecifier),
                    uint.Parse(color.Substring(3, 2), System.Globalization.NumberStyles.AllowHexSpecifier),
                    uint.Parse(color.Substring(5, 2), System.Globalization.NumberStyles.AllowHexSpecifier));
            }
            else if (color.StartsWith("rgb"))
            {
                var colorParts = color.Split(new char[] { '(', ',', ')' });
                if (colorParts.Length >= 4)
                    return (uint.Parse(colorParts[1]), uint.Parse(colorParts[2]), uint.Parse(colorParts[3]));
            }

            return (0, 0, 0);
        }
    }
}

