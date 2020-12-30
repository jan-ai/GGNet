using System;
using GGNet.Coords;
using GGNet.Data;
using Microsoft.AspNetCore.Components;

namespace GGNet.Components.Labeling
{
    public partial class PolarYAxis : AxisBase
    {
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            zone.X = Area.X + Theme.Axis.Y switch
            {
                Position.Left => -Theme.Axis.Text.Y.Margin.Right,
                Position.Right => Area.Width + Theme.Axis.Text.Y.Margin.Left,
                _ => throw new IndexOutOfRangeException(),
            };

            zone.Y = Area.Y;
            zone.Width = Axis.Zone.Width;
            zone.Height = Area.Height;
        }
    }
}
