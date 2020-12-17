using System;
using System.Collections.Generic;
using System.Text;
using GGNet.Data;
using GGNet.Facets;
using GGNet.Scales;
using GGNet.Shapes;
using GGNet.Transformations;

namespace GGNet.Coords
{
    public class Flip : Cartesian
    {
        public Flip((double min, double max)? limits = null, bool expand = true, bool clip = true) : base (limits, expand, clip)
        { }

        public override Axis XAxis<T, TX, TY>(Data<T, TX, TY>.Panel panel) where TX : struct where TY : struct => panel.yAxis;
        public override (double min, double max) XRange<T, TX, TY>(Data<T, TX, TY>.Panel panel) where TX : struct where TY : struct => panel.X.Range;
        public override ITransformation<double> XTransformation<T, TX, TY>(Data<T, TX, TY>.Panel panel) where TX : struct where TY : struct => panel.X.RangeTransformation;

        public override Axis YAxis<T, TX, TY>(Data<T, TX, TY>.Panel panel) where TX : struct where TY : struct => panel.xAxis;
        public override (double min, double max) YRange<T, TX, TY>(Data<T, TX, TY>.Panel panel) where TX : struct where TY : struct => panel.Y.Range;
        public override ITransformation<double> YTransformation<T, TX, TY>(Data<T, TX, TY>.Panel panel) where TX : struct where TY : struct => panel.Y.RangeTransformation;

        public override int PanelRows<T, TX, TY>(Data<T, TX, TY> data) where TX : struct where TY : struct => data.N.cols;
        public override int PanelColumns<T, TX, TY>(Data<T, TX, TY> data) where TX : struct where TY : struct => data.N.rows;
        public override (int row, int col) PanelSize((int row, int col) size) => (size.col, size.row);

        public override void SetAxisVisibility<T, TX, TY>(Data<T, TX, TY>.Panel panel, Theme theme, bool showX, bool showY)
            where TX : struct
            where TY : struct
        {
            panel.xAxis.Show = showY || panel.Position.row == (panel.Data.N.cols - 1);
            panel.yAxis.Show = showX || theme.Axis.Y == Position.Left ? panel.Position.col == 0 : panel.Position.col == (panel.Data.N.rows - 1); ;
        }

        public override (double, double) Transform(Zone zone, double x, double y) => base.Transform(zone, y, x);

        public override Shape Transform(Shape shape)
        {
            switch (shape)
            {
                case Rectangle r:
                    shape = new Rectangle()
                    {
                        Aesthetic = r.Aesthetic,
                        Classes = r.Classes,
                        Height = r.Width,
                        Width = r.Height,
                        X = r.Y,
                        Y = r.X,
                        OnClick = r.OnClick,
                        OnMouseOut = r.OnMouseOut,
                        OnMouseOver = r.OnMouseOver,
                    };

                    break;
            }

            return base.Transform(shape);
        }
    }
}
