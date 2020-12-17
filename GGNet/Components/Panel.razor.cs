using System.Linq;

using Microsoft.AspNetCore.Components;

using static GGNet.Position;

namespace GGNet.Components
{
    public partial class Panel<T, TX, TY> : ComponentBase, IPanel
        where TX : struct
        where TY : struct
    {
        [CascadingParameter]
        public Plot<T, TX, TY> Plot { get; set; }

        [Parameter]
        public Data<T, TX, TY>.Panel Data { get; set; }

        [Parameter]
        public double X { get; set; }

        [Parameter]
        public double Y { get; set; }

        [Parameter]
        public double Width { get; set; }

        [Parameter]
        public double Height { get; set; }

        [Parameter]
        public double XAxisSpace { get; set; }

        [Parameter]
        public double YAxisSpace { get; set; }

        [Parameter]
        public bool First { get; set; }

        [Parameter]
        public bool Last { get; set; }

        private RenderChildPolicyBase policy;
        private RenderChildPolicyBase areaPolicy;

        private Zone xStrip;
        private Zone yStrip;

        public Zone yAxisText;
        private Zone yAxisTitle;

        private Zone xAxisText;
        private Zone xAxisSubText;
        private Zone xAxisTitle;

        public Zone Area;

        protected Tooltips.Plot tooltip;
        public ITooltip Tooltip => tooltip;
        
        protected string clip;

        protected bool firstRender = true;

        protected override void OnInitialized()
        {
            Data.Register(this);

            policy = Plot.Policy.Child();
            areaPolicy = Plot.Policy.Child();

            clip = Plot.Id + "-" + Data.Id;
        }

        protected void Render(bool firstRender)
        {
            Area.X = X;
            Area.Y = Y;
            Area.Width = Width;
            Area.Height = Height;

            if (!string.IsNullOrEmpty(Data.Strip.x))
            {
                var width = Data.Strip.x.Width(Data.Data.Theme.Strip.Text.X.Size);
                var height = Data.Strip.x.Height(Data.Data.Theme.Strip.Text.X.Size);

                //xStrip.X = X + Data.Data.Theme.Strip.Text.X.Margin.Left;
                xStrip.Y = Y + Data.Data.Theme.Strip.Text.X.Margin.Top + height;
                xStrip.Width = Data.Data.Theme.Strip.Text.X.Margin.Left + width + Data.Data.Theme.Strip.Text.X.Margin.Right;
                xStrip.Height = Data.Data.Theme.Strip.Text.X.Margin.Top + height + Data.Data.Theme.Strip.Text.X.Margin.Bottom;

                Area.Y += xStrip.Height;
                Area.Height -= xStrip.Height;
            }

            if (!string.IsNullOrEmpty(Data.Strip.y))
            {
                var width = Data.Strip.y.Height(Data.Data.Theme.Strip.Text.Y.Size);
                var height = Data.Strip.y.Width(Data.Data.Theme.Strip.Text.Y.Size);

                yStrip.X = Area.X + Area.Width - Data.Data.Theme.Strip.Text.X.Margin.Right - width;
                yStrip.Y = Area.Y + Data.Data.Theme.Strip.Text.Y.Margin.Top;
                yStrip.Width = Data.Data.Theme.Strip.Text.Y.Margin.Left + width + Data.Data.Theme.Strip.Text.Y.Margin.Right;
                yStrip.Height = Data.Data.Theme.Strip.Text.Y.Margin.Top + height + Data.Data.Theme.Strip.Text.Y.Margin.Bottom;

                Area.Width -= yStrip.Width;
            }

            if (Data.Data.Theme.Axis.Y == Left)
            {
                Area.X += YAxisSpace;
            }

            if (Data.yAxis.Show)
            {
                var axisWidth = Data.yAxis.Width;

                if (Data.Data.Theme.Axis.Y == Left)
                {
                    if (Data.YLab.width > 0.0 || Data.yAxis.Titles.Any())
                    {
                        yAxisTitle.X = Area.X + Data.Data.Theme.Axis.Title.Y.Margin.Left + Data.YLab.width - YAxisSpace;
                        yAxisTitle.Y = Area.Y + Data.Data.Theme.Axis.Title.Y.Margin.Bottom;
                        yAxisTitle.Width = Data.Data.Theme.Axis.Title.Y.Margin.Left + Data.YLab.width + Data.Data.Theme.Axis.Title.Y.Margin.Right;
                        yAxisTitle.Height = Area.Height;

                        Area.X += yAxisTitle.Width;
                        Area.Width -= yAxisTitle.Width;
                    }

                    yAxisText.X = Area.X - Data.Data.Theme.Axis.Text.Y.Margin.Right;
                    yAxisText.Y = Area.Y;
                    yAxisText.Width = YAxisSpace;
                }
                else if (Data.Data.Theme.Axis.Y == Right)
                {
                    if (Data.YLab.width > 0.0 || Data.yAxis.Titles.Any())
                    {
                        yAxisTitle.X = Area.X + Area.Width - Data.YLab.width - Data.Data.Theme.Axis.Title.Y.Margin.Right;
                        yAxisTitle.Y = Area.Y + Data.Data.Theme.Axis.Title.Y.Margin.Bottom;
                        yAxisTitle.Width = Data.Data.Theme.Axis.Title.Y.Margin.Left + Data.YLab.width + Data.Data.Theme.Axis.Title.Y.Margin.Right;
                        yAxisTitle.Height = Area.Height;

                        Area.Width -= yAxisTitle.Width;
                    }

                    yAxisText.X = Area.X + Area.Width + Data.Data.Theme.Axis.Text.Y.Margin.Left;
                    yAxisText.Y = Area.Y;
                    yAxisText.Width = axisWidth;
                }
            }

            if (Data.xAxis.Show)
            {
                var xTitlesHeight = Data.XLab.height;

                if (xTitlesHeight > 0.0)
                {
                    xAxisTitle.X = Area.X + Area.Width - Data.Data.Theme.Axis.Title.X.Margin.Right;
                    xAxisTitle.Y = Area.Y + Area.Height + XAxisSpace- Data.Data.Theme.Axis.Title.X.Margin.Bottom;
                    xAxisTitle.Width = Data.Data.Theme.Axis.Title.X.Margin.Left + Area.Width + Data.Data.Theme.Axis.Title.X.Margin.Right;
                    xAxisTitle.Height = Data.Data.Theme.Axis.Title.X.Margin.Top + xTitlesHeight + Data.Data.Theme.Axis.Title.X.Margin.Bottom;

                    Area.Height -= xAxisTitle.Height;
                }

                if (Data.xAxis.Titles.Any())
                {
                    xAxisSubText.X = Area.X + Area.Width - Data.Data.Theme.Axis.Title.X.Margin.Right;
                    xAxisSubText.Y = Area.Y + Area.Height + XAxisSpace - Data.Data.Theme.Axis.Title.X.Margin.Bottom;
                    xAxisSubText.Width = Data.Data.Theme.Axis.Title.X.Margin.Left + Area.Width + Data.Data.Theme.Axis.Title.X.Margin.Right;
                    xAxisSubText.Height = Data.Data.Theme.Axis.Title.X.Margin.Top + xTitlesHeight + Data.Data.Theme.Axis.Title.X.Margin.Bottom;

                    Area.Height -= xAxisSubText.Height;
                }

                xAxisText.X = Area.X;
                xAxisText.Y = Area.Y + Area.Height + XAxisSpace - Data.Data.Theme.Axis.Text.X.Margin.Bottom;
                xAxisText.Width = Area.Width;
                xAxisText.Height = Data.Data.Theme.Axis.Text.X.Margin.Top + Data.xAxis.Height + Data.Data.Theme.Axis.Text.X.Margin.Bottom;
            }

            if (xStrip.Width > 0)
            {
                xStrip.X = Area.X + Data.Data.Theme.Strip.Text.X.Margin.Left;
            }

            if (yAxisText.Width > 0)
            {
                yAxisText.Height = Area.Height;
            }

            if (!firstRender)
            {
                areaPolicy.Refresh();
            }
        }

        public void Refresh() => policy.Refresh();

        protected override bool ShouldRender() => policy.ShouldRender();
    }
}
