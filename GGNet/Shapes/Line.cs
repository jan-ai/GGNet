using GGNet.Scales;

namespace GGNet.Shapes
{
    public class Line : Shape
    {
        public double X1 { get; set; }

        public double X2 { get; set; }

        public double Y1 { get; set; }

        public double Y2 { get; set; }

        public Elements.Line Aesthetic { get; set; }

        public override void Scale<TX, TY>(Position<TX> ScaleX, Position<TY> ScaleY)
        {
            X1 = ScaleX.Coord(X1);
            X2 = ScaleX.Coord(X2);
            Y1 = ScaleY.Coord(Y1);
            Y2 = ScaleY.Coord(Y2);
        }
    }

    public class VLine : Shape
    {
        public double X { get; set; }

        public string Label { get; set; }

        public Elements.Line Line { get; set; }

        public Elements.Text Text { get; set; }

        public override void Scale<TX, TY>(Position<TX> ScaleX, Position<TY> ScaleY)
        {
            X = ScaleX.Coord(X);
        }
    }

    public class HLine : Shape
    {
        public double Y { get; set; }

        public string Label { get; set; }

        public Elements.Line Line { get; set; }

        public Elements.Text Text { get; set; }

        public override void Scale<TX, TY>(Position<TX> ScaleX, Position<TY> ScaleY)
        {
            Y = ScaleY.Coord(Y);
        }
    }

    public class ABLine : Shape
    {
        public double A { get; set; }

        public double B { get; set; }

        public (bool x, bool y) Transformation { get; set; }

        public string Label { get; set; }

        public Elements.Line Line { get; set; }

        public Elements.Text Text { get; set; }
        public override void Scale<TX, TY>(Position<TX> ScaleX, Position<TY> ScaleY)
        {
        }
    }
}
