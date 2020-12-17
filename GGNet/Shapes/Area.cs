using System.Collections.Generic;
using GGNet.Scales;

namespace GGNet.Shapes
{
    public class Area : Shape
    {
        public Area() => Points = new SortedBuffer<(double x, double ymin, double ymax)>(comparer: comparer);

        public SortedBuffer<(double x, double ymin, double ymax)> Points { get; }

        private class Comparer : Comparer<(double x, double ymin, double ymax)>
        {
            public override int Compare((double x, double ymin, double ymax) a, (double x, double ymin, double ymax) b) => a.x.CompareTo(b.x);
        }

        private static readonly Comparer comparer = new Comparer();

        public Elements.Rectangle Aesthetic { get; set; }

        public override void Scale<TX, TY>(Position<TX> ScaleX, Position<TY> ScaleY)
        {
            for (var i = 0; i < Points.Count; i++)
            {
                var (x, ymin, ymax) = Points[i];
                Points[i] = (ScaleX.Coord(x), ScaleY.Coord(ymin), ScaleY.Coord(ymax));
            }
        }
    }
}
