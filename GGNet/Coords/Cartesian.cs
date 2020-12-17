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
    public class Cartesian : ICoord 
    {
        public Cartesian ((double min, double max)? limits = null, bool expand = true, bool clip = true)
        {
        }

        public virtual Axis XAxis<T, TX, TY>(Data<T, TX, TY>.Panel panel) where TX : struct where TY : struct => panel.xAxis;
        public virtual (double min, double max) XRange<T, TX, TY> (Data<T, TX, TY>.Panel panel) where TX: struct where TY : struct => panel.X.Range;
        public virtual ITransformation<double> XTransformation<T, TX, TY>(Data<T, TX, TY>.Panel panel) where TX : struct where TY : struct => panel.X.RangeTransformation;

        public virtual Axis YAxis<T, TX, TY>(Data<T, TX, TY>.Panel panel) where TX : struct where TY : struct => panel.yAxis;
        public virtual (double min, double max) YRange<T, TX, TY>(Data<T, TX, TY>.Panel panel) where TX : struct where TY : struct => panel.Y.Range;
        public virtual ITransformation<double> YTransformation<T, TX, TY>(Data<T, TX, TY>.Panel panel) where TX : struct where TY : struct => panel.Y.RangeTransformation;

        public virtual int PanelRows<T, TX, TY>(Data<T, TX, TY> data) where TX : struct where TY : struct => data.N.rows;
        public virtual int PanelColumns<T, TX, TY>(Data<T, TX, TY> data) where TX : struct where TY : struct => data.N.cols;
        public virtual (int row, int col) PanelSize((int row, int col) size) => size; 

        public virtual void SetAxisVisibility<T, TX, TY>(Data<T, TX, TY>.Panel panel, Theme theme, bool showX, bool showY)
            where TX : struct 
            where TY : struct
        {
            panel.xAxis.Show = showX || panel.Position.row == (panel.Data.N.rows - 1);
            panel.yAxis.Show = showY || theme.Axis.Y == Position.Left ? panel.Position.col == 0 : panel.Position.col == (panel.Data.N.cols - 1); ;
        }

        public virtual (double, double) Transform(Zone zone, double x, double y) => (zone.CoordX(x), zone.CoordY(y));

        public virtual Shape Transform (Shape shape)
        {
            return shape;
        }
    }
}
