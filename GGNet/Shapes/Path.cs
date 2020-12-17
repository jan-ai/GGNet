using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using GGNet.Scales;

namespace GGNet.Shapes
{
    public class Path : Shape
    {
        public Path() => Points = new SortedBuffer<(double x, double y)>(comparer: comparer);

        public SortedBuffer<(double x, double y)> Points { get; }

        private class Comparer : Comparer<(double x, double y)>
        {
            public override int Compare([AllowNull] (double x, double y) a, [AllowNull] (double x, double y) b) => a.x.CompareTo(b.x);
        }

        private static readonly Comparer comparer = new Comparer();

        public Elements.Line Aesthetic { get; set; }

        public override void Scale<TX, TY>(Position<TX> ScaleX, Position<TY> ScaleY)
        {
            for (var i = 0; i < Points.Count; i++)
            {
                var (x, y)= Points[i];
                Points[i] = (ScaleX.Coord(x), ScaleY.Coord(y));
            }
        }
    }
}
