using GGNet.Scales;

namespace GGNet.Shapes
{
    public class Text : Shape
    {
        public double X { get; set; }

        public double Y { get; set; }

        public double Height { get; set; }

        public double Width { get; set; }

        public string Value { get; set; }

        public Elements.Text Aesthetic { get; set; }

        public override void Scale<TX, TY>(Position<TX> ScaleX, Position<TY> ScaleY)
        {
            X = ScaleX.Coord(X);
            Y = ScaleY.Coord(Y);
        }
    }
}
