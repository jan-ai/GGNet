﻿using System;
using System.Globalization;
using System.Text;
using GGNet.Transformations;
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
        public Zone Zone { get; set; }

        [Parameter]
        public string Clip { get; set; }

        private readonly StringBuilder sb = new StringBuilder();

        protected override bool ShouldRender() => RenderPolicy.ShouldRender();

        public (double min, double max) XRange => Data.Data.Coord.XRange(Data); 
        public ITransformation<double> XTransformation => Data.Data.Coord.XTransformation(Data);
        public (double min, double max) YRange => Data.Data.Coord.YRange(Data);
        public ITransformation<double> YTransformation => Data.Data.Coord.YTransformation(Data);

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
                sb.Append(Zone.CoordX(x).ToString(CultureInfo.InvariantCulture));
                sb.Append(" ");
                sb.Append(Zone.CoordY(y).ToString(CultureInfo.InvariantCulture));
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
                sb.Append(Zone.CoordX(x).ToString(CultureInfo.InvariantCulture));
                sb.Append(" ");
                sb.Append(Zone.CoordY(ymax).ToString(CultureInfo.InvariantCulture));
            }

            for (var j = 0; j < area.Points.Count; j++)
            {
                var (x, ymin, _) = area.Points[area.Points.Count - j - 1];

                sb.Append(" L ");
                sb.Append(Zone.CoordX(x).ToString(CultureInfo.InvariantCulture));
                sb.Append(" ");
                sb.Append(Zone.CoordY(ymin).ToString(CultureInfo.InvariantCulture));
            }

            sb.Append(" Z");

            return sb.ToString();
        }

        private void AppendPolygon(Geospacial.Polygon poly)
        {
            for (var j = 0; j < poly.Longitude.Length; j++)
            {
                sb.Append(j == 0 ? "M " : " L ");
                sb.Append(Zone.CoordX(poly.Longitude[j]).ToString(CultureInfo.InvariantCulture));
                sb.Append(" ");
                sb.Append(Zone.CoordY(poly.Latitude[j]).ToString(CultureInfo.InvariantCulture));
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

        private string Path(Shapes.Polycurve curve)
        {
            sb.Clear();

            for (var j = 0; j < curve.Points.Count; j++)
            {
                var arc = curve.Points[j];

                if (double.IsNaN(arc.Y))
                    continue;

                var x = Zone.CoordX(arc.X);
                var y = Zone.CoordY(arc.Y);

                if (double.IsNaN(arc.Radius))
                {
                    sb.Append(j == 0 ? "M " : " L ");
                }
                else
                {
                    var radius = Zone.Width * arc.Radius;

                    sb.Append(" A ");
                    sb.Append(radius.ToString(CultureInfo.InvariantCulture));
                    sb.Append(" ");
                    sb.Append(radius.ToString(CultureInfo.InvariantCulture));
                    sb.Append(" 0 ");
                    sb.Append(arc.LargeArcFlag ? " 1 " : " 0 ");
                    sb.Append(arc.SweepFlag ? " 1 " : " 0 ");
                }

                sb.Append(x.ToString(CultureInfo.InvariantCulture));
                sb.Append(" ");
                sb.Append(y.ToString(CultureInfo.InvariantCulture));
            }

            return sb.ToString();
        }
    }
}
