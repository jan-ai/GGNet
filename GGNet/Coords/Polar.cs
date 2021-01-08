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
    public class Polar : ICoord 
    {
        public enum AngelMapping
        {
            X,
            Y
        }

        public enum Direction
        {
            Clockwise,
            Anticlockwise
        }

        private AngelMapping _theta;
        private double _startRadians;
        private Direction _direction;

        public Polar(AngelMapping theta = AngelMapping.X, double startRadians = 0, Direction direction = Direction.Clockwise)
        {
            _theta = theta;
            _startRadians = startRadians;
            _direction = direction;
        }

        public virtual Axis XAxis<T, TX, TY>(Data<T, TX, TY>.Panel panel) where TX : struct where TY : struct => _theta == AngelMapping.Y ? panel.xAxis : panel.yAxis;
        public virtual (double min, double max) XRange<T, TX, TY> (Data<T, TX, TY>.Panel panel) where TX: struct where TY : struct => panel.X.Range;
        public virtual ITransformation<double> XTransformation<T, TX, TY>(Data<T, TX, TY>.Panel panel) where TX : struct where TY : struct => panel.X.RangeTransformation;

        public virtual Axis YAxis<T, TX, TY>(Data<T, TX, TY>.Panel panel) where TX : struct where TY : struct => _theta == AngelMapping.X ? panel.xAxis : panel.yAxis;
        public virtual (double min, double max) YRange<T, TX, TY>(Data<T, TX, TY>.Panel panel) where TX : struct where TY : struct => panel.Y.Range;
        public virtual ITransformation<double> YTransformation<T, TX, TY>(Data<T, TX, TY>.Panel panel) where TX : struct where TY : struct => panel.Y.RangeTransformation;

        public virtual int PanelRows<T, TX, TY>(Data<T, TX, TY> data) where TX : struct where TY : struct => data.N.rows;
        public virtual int PanelColumns<T, TX, TY>(Data<T, TX, TY> data) where TX : struct where TY : struct => data.N.cols;
        public virtual (int row, int col) PanelSize((int row, int col) size) => size; 

        public virtual void SetAxisVisibility<T, TX, TY>(Data<T, TX, TY>.Panel panel, Theme theme, bool showX, bool showY)
            where TX : struct 
            where TY : struct
        {
            panel.xAxis.Show = (_theta == AngelMapping.Y ? showX : showY) || panel.Position.row == (panel.Data.N.rows - 1);
            panel.yAxis.Show = (_theta == AngelMapping.X ? showX : showY) || theme.Axis.Y == Position.Left ? panel.Position.col == 0 : panel.Position.col == (panel.Data.N.cols - 1); ;
        }

        public virtual (double x1, double y1, double x2, double y2) XGrid(Zone zone, double grid)
        {
            if (_direction == Direction.Clockwise)
                grid = 1 - grid;

            var radius = Math.Min(1, zone.Height / zone.Width) / 2;
            var (x1, y1) = PolarToCartesian(zone, 0, grid * 360);
            var (x2, y2) = PolarToCartesian(zone, radius, grid * 360);

            return (zone.CoordX(x1), zone.CoordY(y1), zone.CoordX(x2), zone.CoordY(y2));
        }

        public virtual (double x1, double y1, double x2, double y2) YGrid(Zone zone, double grid)
        {
            if (_direction == Direction.Clockwise)
                grid = 1 - grid;

            var radius = Math.Min(1, zone.Height / zone.Width) / 2;
            var (x1, y1) = PolarToCartesian(zone, 0, grid * 360);
            var (x2, y2) = PolarToCartesian(zone, radius, grid * 360);

            return (zone.CoordX(x1), zone.CoordY(y1), zone.CoordX(x2), zone.CoordY(y2));
        }

        public virtual (double, double) Transform(Zone zone, double x, double y)
        {
            double radius, angle, factor; 
            
            if (_theta == AngelMapping.Y)
            {
                factor = Math.Min(1, zone.Height / zone.Width) / 2;

                radius = x * factor;
                angle = y * 360;
            }
            else
            {
                factor = Math.Min(1, zone.Width / zone.Height) / 2;

                radius = y * factor;
                angle = x * 360;
            }

            if (_direction == Direction.Clockwise)
                angle = 360 - angle;

            var point = PolarToCartesian(zone, radius, angle);

            return (zone.CoordX(point.x), zone.CoordY(point.y));
        }

        public virtual Shape Transform (Zone zone, Shape shape)
        {
            switch (shape)
            {
                case Rectangle r:
                    var curve = new Polycurve()
                    {
                        Aesthetic = r.Aesthetic,
                        Classes = r.Classes,
                        OnClick = r.OnClick,
                        OnMouseOut = r.OnMouseOut,
                        OnMouseOver = r.OnMouseOver,
                    };

                    double innerRadius, outerRadius, startAngle, endAngle, factor;

                    if (_theta == AngelMapping.Y)
                    {
                        factor = Math.Min(1, zone.Height / zone.Width) / 2;

                        innerRadius = (r.X - r.Width / 2) * factor;
                        outerRadius = (r.X + r.Width / 2) * factor;
                        startAngle = r.Y * 360;
                        endAngle = (r.Y + r.Height) * 360;

                    }
                    else
                    {
                        factor = Math.Min(1, zone.Width / zone.Height) / 2;

                        innerRadius = (r.Y + r.Height / 2) * factor;
                        outerRadius = (r.Y + r.Height / 2) * factor;
                        startAngle = r.X * 360;
                        endAngle = (r.X + r.Width) * 360;
                    }

                    if (_direction == Direction.Clockwise)
                    {
                        startAngle = 360 - startAngle;
                        endAngle = 360 - endAngle;
                    }

                    var startPoint = PolarToCartesian(zone, innerRadius, startAngle);
                    curve.Points.Add(new Polycurve.Arc(startPoint.x, startPoint.y));

                    var point = PolarToCartesian(zone, outerRadius, startAngle);
                    curve.Points.Add(new Polycurve.Arc(point.x, point.y));

                    var isCircle = Math.Abs(endAngle - startAngle) == 360;
                    if (isCircle)
                    {
                        var middleAngle = startAngle + (endAngle - startAngle) / 2;

                        point = PolarToCartesian(zone, outerRadius, middleAngle);
                        curve.Points.Add(new Polycurve.Arc(point.x, point.y, outerRadius, startAngle, middleAngle));

                        point = PolarToCartesian(zone, outerRadius, endAngle);
                        curve.Points.Add(new Polycurve.Arc(point.x, point.y, outerRadius, middleAngle, endAngle));
                    }
                    else
                    {
                        point = PolarToCartesian(zone, outerRadius, endAngle);
                        curve.Points.Add(new Polycurve.Arc(point.x, point.y, outerRadius, startAngle, endAngle));
                    }

                    point = PolarToCartesian(zone, innerRadius, endAngle);
                    curve.Points.Add(new Polycurve.Arc(point.x, point.y));

                    if (isCircle)
                    {
                        var middleAngle = startAngle + (endAngle - startAngle) / 2;

                        point = PolarToCartesian(zone, innerRadius, middleAngle);
                        curve.Points.Add(new Polycurve.Arc(point.x, point.y, innerRadius, endAngle, middleAngle));

                        curve.Points.Add(new Polycurve.Arc(startPoint.x, startPoint.y, innerRadius, middleAngle, startAngle));
                    }
                    else
                    {
                        curve.Points.Add(new Polycurve.Arc(startPoint.x, startPoint.y, innerRadius, endAngle, startAngle));
                    }

                    return curve;
            }

            return shape;
        }

        private (double x, double y) PolarToCartesian(Zone zone, double radius, double angleInDegrees)
        {
            var angleInRadians = (angleInDegrees + 90) * Math.PI / 180.0 + (_direction == Direction.Clockwise ? -_startRadians : _startRadians);
            var factor = _theta == AngelMapping.Y ? zone.Width / zone.Height : zone.Height / zone.Width;

            return (x: radius * Math.Cos(angleInRadians) + 0.5, y: radius * factor * Math.Sin(angleInRadians) + 0.5);
        }
    }
}
