using System;
using System.Collections.Generic;

namespace GGNet
{
    using Scales;
    using Facets;
    using Geoms;
    using GGNet.Data;
    using System.Linq;

    public partial class Data<T, TX, TY>
    {
        public class Panel
        {
            public Panel((int row, int col) pos, Data<T, TX, TY> data, double width, double height)
            {
                Data = data;
                Position = pos;
                Width = width;
                Height = height;

                Id = $"{Position.row}_{Position.col}";
            }

            public (int row, int col) Position { get; }

            public Data<T, TX, TY> Data { get; }

            public double Width { get; }

            public double Height { get; }

            public string Id { get; }

            public Buffer<IGeom> Geoms { get; } = new Buffer<IGeom>(8, 1);

            public Strip XStrip { get; set; } = new Strip(GGNet.Data.Strip.TextAlignment.BottomLeft);
            public Strip YStrip { get; set; } = new Strip(GGNet.Data.Strip.TextAlignment.TopRight);

            internal Components.IPanel Component { get; set; }

            internal void Register(Components.IPanel component)
            {
                Component = component;
            }

            internal Axis xAxis = new Axis ();

            internal Axis yAxis = new Axis ();

            internal Position<TX> X
            {
                get
                {
                    if (Data.Positions.X.Scales.Count == 1)
                    {
                        return Data.Positions.X.Scales[0];
                    }
                    else
                    {
                        return Data.Positions.X.Scales[Position.row * Data.Coord.PanelColumns(Data) + Position.col];
                    }
                }
            }

            internal Position<TY> Y
            {
                get
                {
                    if (Data.Positions.Y.Scales.Count == 1)
                    {
                        return Data.Positions.Y.Scales[0];
                    }
                    else
                    {
                        return Data.Positions.Y.Scales[Position.row * Data.Coord.PanelColumns(Data) + Position.col];
                    }
                }
            }

            public void Render (bool first)
            {
                if (X != null)
                {
                    var _xAxis = Data.Coord.XAxis(this);

                    _xAxis.Breaks = X.Breaks.Select(b => X.Coord(b));
                    _xAxis.MinorBreaks = X.MinorBreaks.Select(b => X.Coord(b));
                    _xAxis.Labels = X.Labels.Select(l => (X.Coord(l.value), l.label));
                    _xAxis.Titles = X.Titles.Select(t => (X.Coord(t.value), t.title));
                }

                if (Y != null)
                {
                    var _yAxis = Data.Coord.YAxis(this);

                    _yAxis.Breaks = Y.Breaks.Select(b => Y.Coord(b));
                    _yAxis.MinorBreaks = Y.MinorBreaks.Select(b => Y.Coord(b));
                    _yAxis.Labels = Y.Labels.Select(l => (Y.Coord(l.value), l.label));
                    _yAxis.Titles = Y.Titles.Select(t => (Y.Coord(t.value), t.title));
                }

                xAxis.Height = 0.0;
                xAxis.TitlesSize = 0.0;

                if (Data.grid)
                {
                    foreach (var (_, label) in xAxis.Labels)
                        xAxis.Height = Math.Max(xAxis.Height, label.Height(Data.Theme.Axis.Text.X.Size));

                    foreach (var (_, title) in xAxis.Titles)
                        xAxis.TitlesSize = Math.Max(xAxis.TitlesSize, title.Height(Data.Theme.Axis.Title.X.Size));
                }

                yAxis.Width = 0.0;
                yAxis.TitlesSize = 0.0;

                if (Data.grid)
                {
                    foreach (var (_, label) in yAxis.Labels)
                        yAxis.Width = Math.Max(yAxis.Width, label.Width(Data.Theme.Axis.Text.Y.Size));

                    foreach (var (_, title) in yAxis.Titles)
                        yAxis.TitlesSize = Math.Max(yAxis.TitlesSize, title.Height(Data.Theme.Axis.Title.Y.Size));
                }
            }
        }

        public class PanelFactory
        {
            private readonly List<Func<Panel, Facet<T>, IGeom>> geoms = new List<Func<Panel, Facet<T>, IGeom>>();

            public PanelFactory(Data<T, TX, TY> data, double width = 1, double height = 1)
            {
                Data = data;
                Width = width;
                Height = height;
            }

            public Data<T, TX, TY> Data { get; }

            public double Width { get; }

            public double Height { get; }

            public Func<Position<TY>> Y { get; set; }

            internal string YLab { get; set; }

            public void Add(Func<Panel, Facet<T>, IGeom> geom) => geoms.Add(geom);

            public Panel Build((int, int) pos, Facet<T> facet = null, double? width = null, double? height = null)
            {
                var panel = new Panel(pos, Data, width ?? Width, height ?? Height);

                foreach (var geom in geoms)
                {
                    panel.Geoms.Add(geom(panel, facet));
                }

                return panel;
            }
        }
    }
}
