using System;
using GGNet.Coords;
using GGNet.Data;
using Microsoft.AspNetCore.Components;

namespace GGNet.Components.Labeling
{
    public partial class PolarXAxis : ComponentBase
    {
        [CascadingParameter]
        public Theme Theme { get; set; }

        [Parameter]
        public ICoord Coord { get; set; }

        [Parameter]
        public Zone Area { get; set; }

        [Parameter]
        public string ClipId { get; set; }

        [Parameter]
        public Axis Axis { get; set; }

        private Zone zone;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            zone.X = Area.X + Area.Width / 2;
            zone.Y = Area.Y;
            zone.Width = Axis.Width;
            zone.Height = Area.Height;
        }
    }
}
