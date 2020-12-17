using GGNet.Scales;

namespace GGNet.Shapes
{
    public class Circle : Shape
    {
        public double X { get; set; }

        public double Y { get; set; }

        public Elements.Circle Aesthetic { get; set; }

        public override void Scale<TX, TY>(Position<TX> ScaleX, Position<TY> ScaleY)
        {
            X = ScaleX.Coord(X);
            Y = ScaleY.Coord(Y);
        }
    }
}
