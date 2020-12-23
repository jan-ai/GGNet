using System;
using GGNet.Scales;

namespace GGNet.Shapes
{
    public class Polycurve : Shape
    {
        public Polycurve() => Points = new Buffer<Arc>();

        public Buffer<Arc> Points { get; }

        public Elements.Rectangle Aesthetic { get; set; }

        public override void Scale<TX, TY>(Position<TX> ScaleX, Position<TY> ScaleY)
        {
            for (var i = 0; i < Points.Count; i++)
            {
                var arc = Points[i];
                Points[i] = new Arc()
                {
                    X = ScaleX.Coord(arc.X),
                    Y = ScaleY.Coord(arc.Y),
                    Radius = arc.Radius,
                    SweepFlag = arc.SweepFlag,
                    LargeArcFlag = arc.LargeArcFlag,
                };
            }
        }

        public class Arc
        {
            public Arc ()
            {

            }

            public Arc(double x, double y, double radius = double.NaN, double startAngle = double.NaN, double endAngle = double.NaN)
            {
                X = x;
                Y = y;
                Radius = radius;
                LargeArcFlag = Math.Abs(endAngle - startAngle) > 180;
                SweepFlag = startAngle > endAngle;
            }

            public double X { get; set; }
            public double Y { get; set; }
            public double Radius { get; set; }
            public bool LargeArcFlag { get; set; }
            public bool SweepFlag { get; set; }
        }
    }
}
