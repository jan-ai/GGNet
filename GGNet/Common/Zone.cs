using GGNet.Coords;

namespace GGNet
{
    public struct Zone
    {
        public double X { get; set; }

        public double Y { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public double CoordX(double x) => X + x * Width;
        public double CoordY(double y) => Y + (1 - y) * Height;
    }
}
