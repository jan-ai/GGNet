using GGNet.Scales;

namespace GGNet.Shapes
{
    public class MultiPolygon : Shape
    {
        public Geospacial.Polygon[] Polygons { get; set; }

        public Elements.Rectangle Aesthetic { get; set; }

        public override void Scale<TX, TY>(Position<TX> ScaleX, Position<TY> ScaleY)
        {
            foreach (var poly in Polygons)
            {
                for (var i = 0; i < poly.Longitude.Length; i++)
                {
                    poly.Longitude[i] = ScaleX.Coord(poly.Longitude[i]);
                    poly.Latitude[i] = ScaleY.Coord(poly.Latitude[i]);
                }
            }
        }
    }
}
