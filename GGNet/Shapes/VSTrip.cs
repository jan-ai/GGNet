using GGNet.Scales;

namespace GGNet.Shapes
{
    public class VStrip : Shape
    {
        public double X { get; set; }

        public double Y { get; set; }

        public double Width { get; set; }

        public override void Scale<TX, TY>(Position<TX> ScaleX, Position<TY> ScaleY)
        {
            Width = ScaleX.Coord(X + Width);
            X = ScaleX.Coord(X);
            Width -= X;

            Y = ScaleY.Coord(Y);
            
        }
    }
}
