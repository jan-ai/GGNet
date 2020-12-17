using GGNet.Scales;

namespace GGNet.Shapes
{
    public class Polygon : Shape
    {
        public Geospacial.Polygon Path { get; set; }

        public Elements.Rectangle Aesthetic { get; set; }

        public override void Scale<TX, TY>(Position<TX> ScaleX, Position<TY> ScaleY)
        {
            for (var i = 0; i < Path.Longitude.Length; i++)
            {
                Path.Longitude[i] = ScaleX.Coord(Path.Longitude[i]);
                Path.Latitude[i] = ScaleY.Coord(Path.Latitude[i]);
            }
        }
    }
}
