using System;
using GGNet.Common;
using GGNet.Transformations;

namespace GGNet.Scales
{
    public abstract class ContinuousPosition<TKey> : Position<TKey>
        where TKey : struct, IComparable
    {

        public ContinuousPosition(ITransformation<TKey> transformation, (double minMult, double minAdd, double maxMult, double maxAdd) expand,
            (TKey? min, TKey? max)? limits = null,
            (TKey? min, TKey? max)? expandLimits = null)
            : base(transformation, expand, limits)
        {
            ExpandLimits = () => expandLimits ?? (null, null);
        }

        public Func<(TKey? min, TKey? max)> ExpandLimits { get; set; }

        public override void Train(TKey key)
        {
        }

        public override void Set(bool grid)
        {
            double? min = _min;
            double? max = _max;

            if (ExpandLimits != null)
            {
                var expandLimits = ExpandLimits.Invoke();
                if (expandLimits.min.HasValue)
                {
                    var expand = Map(expandLimits.min.Value, true);
                    min = min.HasValue ? Math.Min(expand, min.Value) : expand;
                }

                if (expandLimits.max.HasValue)
                {
                    var expand = Map(expandLimits.max.Value, true);
                    max = max.HasValue ? Math.Max(expand, max.Value) : expand;
                }
            }

            if (min.HasValue && max.HasValue)
                SetRange(min.Value, max.Value);

            if (grid)
                Labeling();
        }
    }
}