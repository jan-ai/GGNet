using System.Globalization;
using System.Text;

using Microsoft.AspNetCore.Components;

namespace GGNet.Components
{
    public partial class Area<T, TX, TY> : ComponentBase
       where TX : struct
       where TY : struct
    {
        [Parameter]
        public Data<T, TX, TY>.Panel Data { get; set; }

        [Parameter]
        public RenderChildPolicyBase RenderPolicy { get; set; }

        [Parameter]
        public ICoord Coord { get; set; }

        [Parameter]
        public Zone Zone { get; set; }

        [Parameter]
        public string Clip { get; set; }

        private readonly StringBuilder sb = new StringBuilder();

        protected override bool ShouldRender() => RenderPolicy.ShouldRender();

        private string Path(Shapes.Path path)
        {
            sb.Clear();

            var op = "M ";
            for (var j = 0; j < path.Points.Count; j++)
            {
                var (x, y) = path.Points[j];

                if (double.IsNaN(y))
                    continue;

                sb.Append(op);
                sb.Append(Coord.CoordX(x).ToString(CultureInfo.InvariantCulture));
                sb.Append(" ");
                sb.Append(Coord.CoordY(y).ToString(CultureInfo.InvariantCulture));
                op = " L ";
            }

            return sb.ToString();
        }

        private string Path(Shapes.Area area)
        {
            sb.Clear();

            for (var j = 0; j < area.Points.Count; j++)
            {
                var (x, _, ymax) = area.Points[j];

                sb.Append(j == 0 ? "M " : " L ");
                sb.Append(Coord.CoordX(x).ToString(CultureInfo.InvariantCulture));
                sb.Append(" ");
                sb.Append(Coord.CoordY(ymax).ToString(CultureInfo.InvariantCulture));
            }

            for (var j = 0; j < area.Points.Count; j++)
            {
                var (x, ymin, _) = area.Points[area.Points.Count - j - 1];

                sb.Append(" L ");
                sb.Append(Coord.CoordX(x).ToString(CultureInfo.InvariantCulture));
                sb.Append(" ");
                sb.Append(Coord.CoordY(ymin).ToString(CultureInfo.InvariantCulture));
            }

            sb.Append(" Z");

            return sb.ToString();
        }

        private void AppendPolygon(Geospacial.Polygon poly)
        {
            for (var j = 0; j < poly.Longitude.Length; j++)
            {
                sb.Append(j == 0 ? "M " : " L ");
                sb.Append(Coord.CoordX(poly.Longitude[j]).ToString(CultureInfo.InvariantCulture));
                sb.Append(" ");
                sb.Append(Coord.CoordY(poly.Latitude[j]).ToString(CultureInfo.InvariantCulture));
            }

            sb.Append(" Z");
        }

        private string Path(Geospacial.Polygon poly)
        {
            sb.Clear();

            AppendPolygon(poly);

            return sb.ToString();
        }

        private string Path(Geospacial.Polygon[] polygons)
        {
            sb.Clear();

            for (var i = 0; i < polygons.Length; i++)
            {
                if (i > 0)
                    sb.Append(" ");

                AppendPolygon(polygons[i]);
            }

            return sb.ToString();
        }
    }
}
