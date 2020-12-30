using System;
using GGNet.Coords;
using GGNet.Data;
using Microsoft.AspNetCore.Components;

namespace GGNet.Components.Labeling
{
    public partial class PolarXAxis : AxisBase
    {
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            zone.X = Area.X + Area.Width / 2;
            zone.Y = Area.Y;
            zone.Width = Axis.Zone.Width;
            zone.Height = Area.Height;
        }
    }
}
