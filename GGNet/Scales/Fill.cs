using System;
using System.Collections.Generic;
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
        public IEnumerable<(double offset, string color)> ColorGuide { get; set; } = Array.Empty<(double offset, string color)>();

        public override void Train(double key)
        {
            if (double.IsNaN(key))
                return;

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

            var colorGuide = new (double offset, string color)[Colors.Length];
            for (int i = 0; i < Colors.Length; i++)
            {
                var min = transformation?.Apply(limits.min) ?? limits.min;
                var max = transformation?.Apply(limits.max) ?? limits.max;
                var key = min + ((double) i) / (Colors.Length - 1) * (max - min);
                key = transformation?.Inverse(key) ?? key;
                var offset = Max(Min((key - limits.min) / (limits.max - limits.min), 1), 0);

                colorGuide[i] = (offset, Colors[i]);
            }

            ColorGuide = colorGuide;
        }

        public override string Map(double key, bool ignoreLimits = false)
        {
            if (double.IsNaN(key))
                return "none";

            if (limits.max > limits.min)
            {
                key = transformation?.Apply(key) ?? key;
                var min = transformation?.Apply(limits.min) ?? limits.min;
                var max = transformation?.Apply(limits.max) ?? limits.max;

                var offset = Max(Min((key - min) / (max - min), 1), 0) * (Colors.Length - 1);
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
                return Colors[0];
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

