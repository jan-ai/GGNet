﻿using GGNet.Transformations;

namespace GGNet.Scales
{
    public abstract class Discrete<TKey, TValue> : Scale<TKey, TValue>
    {
        private readonly bool defined;
        private readonly Palettes.Discrete<TKey, TValue> palette;
        private readonly TValue na;

        private Discrete(
            TValue na,
            ITransformation<TKey> transformation)
            : base(transformation)
        {
            this.na = na;
        }

        public Discrete(
            Palettes.Discrete<TKey, TValue> palette,
            TValue na,
            ITransformation<TKey> transformation)
            : this(na, transformation)
        {
            defined = true;
            this.palette = palette;
        }

        public Discrete(
            TValue[] palette,
            int direction,
            TValue na,
            ITransformation<TKey> transformation)
            : this(na, transformation)
        {
            this.palette = new Palettes.Discrete<TKey, TValue>(palette, direction);
        }

        public override void Train(TKey key)
        {
            if (defined)
            {
                return;
            }

            palette.Add(key);
        }

        public override void Set()
        {
            if (!defined)
            {
                palette.Set();
            }

            var values = palette.Values();

            var breaks = new TValue[values.Length];
            var labels = new (TValue value, string label)[values.Length];

            for (int i = 0; i < values.Length; i++)
            {
                breaks[i] = values[i].value;
                labels[i] = (values[i].value, values[i].key.ToString());
            }

            Breaks = breaks;
            Labels = labels;
        }

        public override TValue Map(TKey key)
        {
            if (!palette.TryGetValue(key, out var result))
            {
                return na;
            }

            return result;
        }

        public override void Clear()
        {
            if (!defined)
            {
                palette.Clear();
            }
        }
    }
}
