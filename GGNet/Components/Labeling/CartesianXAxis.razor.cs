using System;
using GGNet.Data;
using Microsoft.AspNetCore.Components;

namespace GGNet.Components.Labeling
{
    public partial class CartesianXAxis : AxisBase
    {
        private Zone zoneTitle;
        private Zone zoneLabel;

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            zone.X = Area.X + Area.Width - Theme.Axis.Text.X.Margin.Right;
            zone.Y = Area.Y + Area.Height + Theme.Axis.Text.X.Margin.Top + Axis.Height;
            zone.Width = Area.Width - Theme.Axis.Text.X.Margin.Left - Theme.Axis.Text.X.Margin.Right;
            zone.Height = Theme.Axis.Text.X.Margin.Top + Axis.AxisLabelSize + Theme.Axis.Text.X.Margin.Bottom;

            zoneTitle.X = Area.X + Area.Width;
            zoneTitle.Y = zone.Y + Theme.Axis.Text.X.Margin.Bottom + Theme.Axis.Title.X.Margin.Top + Axis.TitlesSize;
            zoneTitle.Width = zone.Width;
            zoneTitle.Height = zone.Height;

            zoneLabel.X = Area.X + Area.Width - Theme.Axis.Title.X.Margin.Right;
            zoneLabel.Y = zone.Y + Theme.Axis.Text.X.Margin.Bottom + Axis.AxisLabelSize + Theme.Axis.Title.X.Margin.Top;
            zoneLabel.Width = Area.Width - Theme.Axis.Title.X.Margin.Left - Theme.Axis.Title.X.Margin.Right;
            zoneLabel.Height = Theme.Axis.Title.X.Margin.Top + Axis.AxisLabelSize + Theme.Axis.Title.X.Margin.Bottom;

            if (Axis.TitlesVisibility)
                zoneLabel.Y += Theme.Axis.Title.X.Margin.Top + Axis.TitlesSize + Theme.Axis.Title.X.Margin.Bottom;
        }
    }
}
