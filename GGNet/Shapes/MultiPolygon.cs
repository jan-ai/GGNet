using GGNet.Scales;

namespace GGNet.Shapes
{
    public class MultiPolygon : Shape
    {
        public Geospacial.Polygon[] Polygons { get; set; }

        public Elements.Rectangle Aesthetic { get; set; }

        public override void Scale<TX, TY>(Position<TX> ScaleX, Position<TY> ScaleY)
        {
            var oldPolygons = Polygons;
            Polygons = new Geospacial.Polygon[oldPolygons.Length];
            for (var p = 0; p < oldPolygons.Length; p++)
            {
                var oldPoly = oldPolygons[p];
                var poly = new Geospacial.Polygon()
                {
                    Hole = oldPoly.Hole,
                    Latitude = new double[oldPoly.Latitude.Length],
                    Longitude = new double[oldPoly.Longitude.Length]
                };

                for (var i = 0; i < oldPoly.Longitude.Length; i++)
                {
                    poly.Longitude[i] = ScaleX.Coord(oldPoly.Longitude[i]);
                    poly.Latitude[i] = ScaleY.Coord(oldPoly.Latitude[i]);
                }

                Polygons[p] = poly;
            }
        }
    }
}
