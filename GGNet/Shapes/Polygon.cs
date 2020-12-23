using GGNet.Scales;

namespace GGNet.Shapes
{
    public class Polygon : Shape
    {
        public Geospacial.Polygon Path { get; set; }

        public Elements.Rectangle Aesthetic { get; set; }

        public override void Scale<TX, TY>(Position<TX> ScaleX, Position<TY> ScaleY)
        {
            var oldPath = Path;
            Path = new Geospacial.Polygon()
            {
                Hole = oldPath.Hole,
                Latitude = new double[oldPath.Latitude.Length],
                Longitude = new double[oldPath.Longitude.Length]
            };

            for (var i = 0; i < oldPath.Longitude.Length; i++)
            {
                Path.Longitude[i] = ScaleX.Coord(oldPath.Longitude[i]);
                Path.Latitude[i] = ScaleY.Coord(oldPath.Latitude[i]);
            }
        }
    }
}
