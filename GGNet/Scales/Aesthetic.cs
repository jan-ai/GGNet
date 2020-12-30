﻿using System;
using System.Collections.Generic;

namespace GGNet.Scales
{
    public interface IAestheticMapping
    {
        bool Guide { get; }

        string Name { get; }

        Guide Type { get; }
    }

    public interface IAestheticMapping<T, TValue> : IAestheticMapping
    {
        void Train(T item);

        TValue Map(T item);

        IEnumerable<(TValue value, string label)> Labels { get; }
    }

    public class Aesthetic<T, TKey, TValue> : IAestheticMapping<T, TValue>
    {
        protected readonly Func<T, TKey> selector;
        protected readonly Scale<TKey, TValue> scale;

        public Aesthetic(Func<T, TKey> selector, Scale<TKey, TValue> scale, bool guide, string name)
        {
            this.selector = selector;
            this.scale = scale;

            Guide = guide;
            Name = name;
        }

        public bool Guide { get; }

        public string Name { get; }

        public Guide Type => scale.Guide;

        public void Train(T item) => scale.Train(selector(item));

        public TValue Map(T item) => scale.Map(selector(item));

        public IEnumerable<(TValue value, string label)> Labels => scale.Labels;

    }

    public class ColorBarAesthetic<T> : Aesthetic<T, double, string>
    {
        public ColorBarAesthetic(Func<T, double> selector, FillContinuous scale, bool guide, string name) : base (selector, scale, guide, name)
        {
        }

        public string[] Colors => (scale as FillContinuous).Colors;
    }
}
