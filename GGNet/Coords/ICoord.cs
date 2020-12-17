using GGNet.Data;
using GGNet.Facets;
using GGNet.Scales;
using GGNet.Shapes;
using GGNet.Transformations;

namespace GGNet.Coords
{
    public interface ICoord
    {
        Axis XAxis<T, TX, TY>(Data<T, TX, TY>.Panel panel) where TX : struct where TY : struct;
        (double min, double max) XRange<T, TX, TY>(Data<T, TX, TY>.Panel panel) where TX : struct where TY : struct;
        ITransformation<double> XTransformation<T, TX, TY>(Data<T, TX, TY>.Panel panel) where TX : struct where TY : struct;

        Axis YAxis<T, TX, TY>(Data<T, TX, TY>.Panel panel) where TX : struct where TY : struct;
        (double min, double max) YRange<T, TX, TY>(Data<T, TX, TY>.Panel panel) where TX : struct where TY : struct;
        ITransformation<double> YTransformation<T, TX, TY> (Data<T, TX, TY>.Panel panel) where TX: struct where TY : struct;

        int PanelRows<T, TX, TY>(Data<T, TX, TY> data) where TX : struct where TY : struct;
        int PanelColumns<T, TX, TY>(Data<T, TX, TY> data) where TX : struct where TY : struct;
        (int row, int col) PanelSize((int row, int col) size);
        void SetAxisVisibility<T, TX, TY>(Data<T, TX, TY>.Panel panel, Theme theme, bool showX, bool showY) where TX : struct where TY : struct;

        (double x, double y) Transform(Zone zone, double x, double y);
        Shape Transform(Shape shape);
    }
}
