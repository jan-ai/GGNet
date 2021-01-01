using System.Linq;
using Microsoft.AspNetCore.Components;

namespace GGNet.Components
{
    public partial class Panel<T, TX, TY> : ComponentBase, IPanel
        where TX : struct
        where TY : struct
    {
        protected RenderChildPolicyBase policy;
        protected RenderChildPolicyBase areaPolicy;
        protected RenderChildPolicyBase axisXPolicy;
        protected RenderChildPolicyBase axisYPolicy;

        protected Zone Area;

        protected Tooltips.Plot tooltip;

        protected string clip;

        protected bool firstRender = true;

        [CascadingParameter]
        public Plot<T, TX, TY> Plot { get; set; }

        [CascadingParameter]
        public Theme Theme { get; set; }

        [Parameter]
        public Data<T, TX, TY>.Panel Data { get; set; }

        [Parameter]
        public Zone Zone { get; set; }

        [Parameter]
        public double XAxisSpace { get; set; }

        [Parameter]
        public double YAxisSpace { get; set; }

        public ITooltip Tooltip => tooltip;

        protected override void OnInitialized()
        {
            Data.Register(this);

            policy = Plot.Policy.Child();
            areaPolicy = Plot.Policy.Child();
            axisXPolicy = Plot.Policy.Child();
            axisYPolicy = Plot.Policy.Child();

            clip = Plot.Id + "-" + Data.Id;
        }

        protected void Render(bool firstRender)
        {
            Area.X = Zone.X;
            Area.Y = Zone.Y;
            Area.Width = Zone.Width - YAxisSpace;
            Area.Height = Zone.Height - XAxisSpace;

            if (Theme.Axis.Y == Position.Left)
                Area.X += YAxisSpace;

            if (Data.XStrip.SetZone(Theme.Strip.Text.X, Theme.Strip.Text.X.Margin, Area))
            {
                Area.Y += Data.XStrip.Zone.Height;
                Area.Height -= Data.XStrip.Zone.Height;
            }

            if (Data.YStrip.SetZone(Theme.Strip.Text.Y, Theme.Strip.Text.Y.Margin, Area))
                Area.Width -= Data.YStrip.Zone.Width;

            if (Data.yAxis.Show)
            {
                if (Data.yAxis.AxisLabelSize > 0.0 || Data.yAxis.Titles.Any())
                {
                    var width = Theme.Axis.Title.Y.Margin.Left + Data.yAxis.AxisLabelSize + Theme.Axis.Title.Y.Margin.Right;

                    if (Theme.Axis.Y == Position.Left)
                    {
                        Area.X += width;
                        Data.XStrip.Zone.X += YAxisSpace;
                    }

                    Area.Width -= width;
                }
            }

            if (!firstRender)
            {
                areaPolicy.Refresh();
                axisXPolicy.Refresh();
                axisYPolicy.Refresh();
            }
        }

        public void Refresh() => policy.Refresh();

        protected override bool ShouldRender() => policy.ShouldRender();
    }
}
