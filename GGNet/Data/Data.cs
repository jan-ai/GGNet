using System;

using NodaTime;

using static System.Math;

namespace GGNet
{
    using Scales;
    using Facets;
    using Coords;

    public partial class Data<T, TX, TY> : IData
        where TX : struct
        where TY : struct
    {
        public Data()
        {
            Id = SVGUtils.Id(this);
        }

        public string Id { get; }

        internal string Title { get; set; }

        internal string SubTitle { get; set; }

        internal string XLab { get; set; }

        internal string Caption { get; set; }

        public Source<T> Source { get; set; }

        public class _Selectors
        {
            public Func<T, TX> X { get; set; }

            public Func<T, TY> Y { get; set; }
        }

        public _Selectors Selectors { get; } = new _Selectors();

        public class _Positions
        {
            public class _Position<TKey>
                where TKey : struct
            {
                public Buffer<Position<TKey>> Scales { get; } = new Buffer<Position<TKey>>(16, 1);

                public Func<Position<TKey>> Factory { get; set; }

                public void Register(Position<TKey> scale) => Scales.Add(scale);

                public Position<TKey> Instance()
                {
                    var instance = Factory();

                    Register(instance);

                    return instance;
                }
            }

            public _Position<TX> X { get; } = new _Position<TX>();

            public _Position<TY> Y { get; } = new _Position<TY>();
        }

        public _Positions Positions { get; } = new _Positions();

        public class _Aesthetics
        {
            public Buffer<IScale> Scales { get; } = new Buffer<IScale>(16, 1);

            public IAestheticMapping<T, string> Color { get; internal set; }

            public IAestheticMapping<T, string> Fill { get; internal set; }

            public IAestheticMapping<T, double> Size { get; internal set; }

            public IAestheticMapping<T, LineType> LineType { get; internal set; }
        }

        public _Aesthetics Aesthetics { get; } = new _Aesthetics();

        public Faceting<T> Faceting { get; set; }

        public ICoord Coord { get; set; } = new Cartesian();

        internal Theme Theme { get; set; }

        public PanelFactory DefaultFactory { get; set; }

        public Buffer<PanelFactory> PanelFactories { get; } = new Buffer<PanelFactory>(4, 1);

        public Panel DefaultPanel { get; set; }

        public Buffer<Panel> Panels { get; } = new Buffer<Panel>(16, 1);

        internal Legends Legends { get; set; }

        internal (int rows, int cols) N { get; set; }

        internal double Strip { get; set; }

        private bool grid = true;

        public void Init(bool grid = true)
        {
            this.grid = grid;

            if (Positions.X.Factory == null)
            {
                if (typeof(TX) == typeof(LocalDate))
                {
                    (this as Data<T, LocalDate, TY>).Scale_X_Discrete_Date();
                }
                else if (typeof(TX) == typeof(LocalDateTime))
                {
                    (this as Data<T, LocalDateTime, TY>).Scale_X_Discrete_DateTime();
                }
                else if (typeof(TX) == typeof(double))
                {
                    (this as Data<T, double, TY>).Scale_X_Continuous();
                }
                else if (typeof(TX) == typeof(string) || typeof(TX).IsEnum)
                {
                    this.Scale_X_Discrete();
                }
                else
                {
                    //TODO: throw
                }
            }

            if (Positions.Y.Factory == null)
            {
                if (typeof(TY) == typeof(double))
                {
                    (this as Data<T, TX, double>).Scale_Y_Continuous();
                }
                else if (typeof(TX) == typeof(string) || typeof(TX).IsEnum)
                {
                    this.Scale_Y_Discrete();
                }
                else
                {
                    //TODO: throw
                }
            }

            Theme ??= Theme.Default();

            Legends = new Legends(Theme);
        }

        protected void RunDefaultPanel(bool first)
        {
            if (first)
            {
                var n = PanelFactories.Count;

                if (n > 0)
                {
                    N = (n, 1);

                    Positions.X.Instance();

                    var ylab = 0.0;

                    for (var i = 0; i < n; i++)
                    {
                        var lab = PanelFactories[i].YLab;
                        if (!string.IsNullOrEmpty(lab))
                        {
                            ylab = lab.Height(Theme.Axis.Title.Y.Size);

                            break;
                        }
                    }

                    for (var i = 0; i < n; i++)
                    {
                        var factory = PanelFactories[i];

                        if (factory.Y == null)
                        {
                            Positions.Y.Instance();
                        }
                        else
                        {
                            Positions.Y.Register(factory.Y());
                        }

                        var panel = factory.Build(Coord.PanelSize((i, 0)));

                        Coord.SetAxisVisibility(panel, Theme, false, false);

                        if (i == (n - 1))
                        {
                            if (!string.IsNullOrEmpty(XLab))
                            {
                                var xAxis = Coord.XAxis(panel);
                                xAxis.AxisLabel = XLab;
                                xAxis.AxisLabelSize = XLab.Height(Theme.Axis.Title.X.Size);
                            }
                        }

                        var yAxis = Coord.YAxis(panel);
                        yAxis.AxisLabel = factory.YLab;
                        yAxis.AxisLabelSize = ylab;

                        Panels.Add(panel);
                    }
                }
                else if (DefaultFactory != null)
                {
                    N = (1, 1);

                    Positions.X.Instance();
                    Positions.Y.Instance();

                    var panel = DefaultFactory.Build(Coord.PanelSize((0, 0)));

                    Coord.SetAxisVisibility(panel, Theme, false, false);

                    if (!string.IsNullOrEmpty(XLab))
                    {
                        var xAxis = Coord.XAxis(panel);
                        xAxis.AxisLabel = XLab;
                        xAxis.AxisLabelSize =XLab.Height(Theme.Axis.Title.X.Size);
                    }

                    if (!string.IsNullOrEmpty(DefaultFactory.YLab))
                    {
                        var yAxis = Coord.YAxis(panel);
                        yAxis.AxisLabel = DefaultFactory.YLab;
                        yAxis.AxisLabelSize = DefaultFactory.YLab.Height(Theme.Axis.Title.Y.Size);
                    }

                    Panels.Add(panel);
                }
            }
            else
            {
                for (var i = 0; i < Positions.X.Scales.Count; i++)
                {
                    Positions.X.Scales[i].Clear();
                }

                for (var i = 0; i < Positions.Y.Scales.Count; i++)
                {
                    Positions.Y.Scales[i].Clear();
                }
            }
        }

        protected void RunFaceting(bool first)
        {
            if (!first)
            {
                Faceting.Clear();

                Panels.Clear();

                Positions.X.Scales.Clear();
                Positions.Y.Scales.Clear();
            }

            for (var i = 0; i < Source?.Count; i++)
            {
                Faceting.Train(Source[i]);
            }

            Faceting.Set();

            var facets = Faceting.Facets();

            N = (Faceting.NRows, Faceting.NColumns);
            
            var width = 1.0 / Coord.PanelColumns(this);
            var height = 1.0 / Coord.PanelRows(this);

            if (Faceting.Strip)
            {
                Strip = Theme.Strip.Text.X.Size.Height();
            }

            if (!Faceting.FreeX)
            {
                Positions.X.Instance();
            }

            if (!Faceting.FreeY)
            {
                Positions.Y.Instance();
            }

            var xlab = 0.0;
            if (!string.IsNullOrEmpty(XLab))
            {
                xlab = XLab.Height(Theme.Axis.Title.X.Size);
            }

            var ylab = 0.0;
            if (!string.IsNullOrEmpty(DefaultFactory.YLab))
            {
                ylab = DefaultFactory.YLab.Height(Theme.Axis.Title.Y.Size);
            }

            for (var i = 0; i < facets.Length; i++)
            {
                var facet = facets[i];

                if (Faceting.FreeX)
                {
                    Positions.X.Instance();
                }

                if (Faceting.FreeY)
                {
                    Positions.Y.Instance();
                }

                var panel = DefaultFactory.Build(Coord.PanelSize(facet.Coord), facet, width, height);

                panel.XStrip.Label = facet.XStrip;
                panel.YStrip.Label = facet.YStrip;

                Coord.SetAxisVisibility(panel, Theme, Faceting.FreeX, Faceting.FreeY);

                if (xlab > 0.0 && panel.Position.row == (Coord.PanelRows(this) - 1))
                {
                    var xAxis = Coord.XAxis(panel);
                    xAxis.AxisLabel = (panel.Position.col == (Coord.PanelColumns(this) - 1)) ? XLab : null;
                    xAxis.AxisLabelSize = xlab;
                }

                if (ylab > 0)
                {
                    if ((Theme.Axis.Y == Position.Left && panel.Position.col == 0) 
                        || Theme.Axis.Y == Position.Right && panel.Position.col == (N.cols - 1))
                    {
                        var yAxis = Coord.YAxis(panel);
                        yAxis.AxisLabel = (panel.Position.row == 0) ? DefaultFactory.YLab : null;
                        yAxis.AxisLabelSize = ylab;
                    }
                }

                Panels.Add(panel);
            }
        }

        protected void ClearAesthetics(bool first)
        {
            if (first)
            {
                return;
            }

            for (var i = 0; i < Aesthetics.Scales.Count; i++)
            {
                Aesthetics.Scales[i].Clear();
            }
        }

        protected void RunLegend(bool first)
        {
            for (int p = 0; p < Panels.Count; p++)
            {
                var panel = Panels[p];

                for (int g = 0; g < panel.Geoms.Count; g++)
                {
                    panel.Geoms[g].Legend();
                }

                if (Faceting != null)
                    return;
            }
        }

        public void Render(bool first)
        {
            if (Faceting == null)
            {
                RunDefaultPanel(first);
            }
            else
            {
                RunFaceting(first);
            }

            ClearAesthetics(first);

            for (int p = 0; p < Panels.Count; p++)
            {
                var panel = Panels[p];

                for (int g = 0; g < panel.Geoms.Count; g++)
                {
                    panel.Geoms[g].Train();
                }
            }

            for (int i = 0; i < Aesthetics.Scales.Count; i++)
            {
                Aesthetics.Scales[i].Set(grid);
            }

            if (grid)
            {
                RunLegend(first);
            }

            for (var p = 0; p < Panels.Count; p++)
            {
                var panel = Panels[p];

                for (var g = 0; g < panel.Geoms.Count; g++)
                {
                    var geom = panel.Geoms[g];
                    if (!first)
                    {
                        geom.Clear();
                    }

                    geom.Shape();
                }
            }

            for (var i = 0; i < Positions.X.Scales.Count; i++)
            {
                var scale = Positions.X.Scales[i];

                scale.Set(grid);
            }

            for (var i = 0; i < Positions.Y.Scales.Count; i++)
            {
                var scale = Positions.Y.Scales[i];

                scale.Set(grid);
            }

            for (var p = 0; p < Panels.Count; p++)
            {
                var panel = Panels[p];

                for (var g = 0; g < panel.Geoms.Count; g++)
                {
                    var geom = panel.Geoms[g];

                    for (var s = 0; s < geom.Layer.Count; s++)
                        geom.Layer[s].Scale(panel.X, panel.Y);

                }

                panel.Render(first);
            }
        }

        #region IData

        public Type PlotType => typeof(Components.Plot<T, TX, TY>);

        #endregion
    }
}
