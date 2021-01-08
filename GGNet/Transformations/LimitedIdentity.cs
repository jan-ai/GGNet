using System;

namespace GGNet.Transformations
{
    public class LimitedIdentity<T> : ITransformation<T>
        where T: struct, IComparable
    {
        private T? _min, _max;

        public LimitedIdentity (T? min, T? max)
        {
            _min = min;
            _max = max;
        }

        public T Apply(T value)
        {
            if (_min.HasValue && _min.Value.CompareTo(value) > 0)
                value = _min.Value;

            if (_max.HasValue && _max.Value.CompareTo(value) < 0)
                value = _max.Value;

            return value;
        }

        public T Inverse(T value) => value;
    }
}
