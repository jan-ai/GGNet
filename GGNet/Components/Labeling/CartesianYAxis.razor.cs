using System;
using System.Linq;
using GGNet.Data;
using Microsoft.AspNetCore.Components;

namespace GGNet.Components.Labeling
{
    public partial class CartesianYAxis : ComponentBase
    {
        [CascadingParameter]
        public Theme Theme { get; set; }

        [Parameter]
        public Zone Area { get; set; }

        [Parameter]
        public string ClipId { get; set; }

        [Parameter]
        public Axis Axis { get; set; }

        [Parameter]
        public double Space { get; set; }

        private Zone zone;
        private Zone zoneTitle;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            zone.X = Area.X + Theme.Axis.Y switch
            {
                Position.Left => -Theme.Axis.Text.Y.Margin.Right,
                Position.Right => Area.Width + Theme.Axis.Text.Y.Margin.Left,
                _ => throw new IndexOutOfRangeException(),
            };

            zone.Y = Area.Y;
            zone.Width = Axis.Width;
            zone.Height = Area.Height;

            if (Axis.AxisLabelSize > 0.0 || Axis.Titles.Any())
            {
                if (Theme.Axis.Y == Position.Left)
                {
                    zoneTitle.X = Theme.Axis.Title.Y.Margin.Left + Axis.AxisLabelSize;
                }
                else if (Theme.Axis.Y == Position.Right)
                {
                    zoneTitle.X = Area.X + Area.Width + Axis.AxisLabelSize - Theme.Axis.Title.Y.Margin.Right;
                }

                zoneTitle.Y = Area.Y + Theme.Axis.Title.Y.Margin.Bottom;
                zoneTitle.Width = Theme.Axis.Title.Y.Margin.Left + Axis.AxisLabelSize + Theme.Axis.Title.Y.Margin.Right;
                zoneTitle.Height = Area.Height;
            }

            zone.Width += zoneTitle.Width;
        }
    }
}
