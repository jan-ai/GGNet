using GGNet.Scales;

namespace GGNet.Shapes
{
    public class Rectangle : Shape
    {
        public double X { get; set; }

        public double Y { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public Elements.Rectangle Aesthetic { get; set; }

        public override void Scale<TX, TY>(Position<TX> ScaleX, Position<TY> ScaleY)
        {
            Width = ScaleX.Coord(X + Width);
            X = ScaleX.Coord(X);
            Width -= X;

            Height = ScaleY.Coord(Y + Height);
            Y = ScaleY.Coord(Y);
            Height -= Y;
        }
    }
}
