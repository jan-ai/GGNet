using Microsoft.AspNetCore.Components;

using static GGNet.Position;

namespace GGNet.Components
{
    public partial class Plot<T, TX, TY> : PlotBase<T, TX, TY>
        where TX : struct
        where TY : struct
    {
        [Parameter]
        public double Width { get; set; } = 720;

        [Parameter]
        public double Height { get; set; } = 576;

        public Zone Title;
        public Zone SubTitle;
        public Zone Legend;
        public Zone Caption;

        private Zone wrapper;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Data.Init();

            Render(true);
        }

        protected void Render(bool firstRender)
        {
            Data.Render(firstRender);

            wrapper.X = 0;
            wrapper.Y = 0;
            wrapper.Width = Width;
            wrapper.Height = Height;

            if (!string.IsNullOrEmpty(Data.Title))
            {
                var width = Data.Title.Width(Theme.Plot.Title.Size);
                var height = Data.Title.Height(Theme.Plot.Title.Size);

                Title.X = Theme.Plot.Title.Margin.Left;
                Title.Y = Theme.Plot.Title.Margin.Top + height;
                Title.Width = Theme.Plot.Title.Margin.Left + width + Theme.Plot.Title.Margin.Right;
                Title.Height = Theme.Plot.Title.Margin.Top + height + Theme.Plot.Title.Margin.Bottom;

                wrapper.Y += Title.Height;
                wrapper.Height -= Title.Height;
            }

            if (!string.IsNullOrEmpty(Data.SubTitle))
            {
                var width = Data.SubTitle.Width(Theme.Plot.SubTitle.Size);
                var height = Data.SubTitle.Height(Theme.Plot.SubTitle.Size);

                SubTitle.X = Theme.Plot.SubTitle.Margin.Left;
                SubTitle.Y = Title.Height + Theme.Plot.SubTitle.Margin.Top + height;
                SubTitle.Width = Theme.Plot.SubTitle.Margin.Left + width + Theme.Plot.SubTitle.Margin.Right;
                SubTitle.Height = Theme.Plot.SubTitle.Margin.Top + height + Theme.Plot.SubTitle.Margin.Bottom;

                wrapper.Y += SubTitle.Height;
                wrapper.Height -= SubTitle.Height;
            }

            if (!string.IsNullOrEmpty(Data.Caption))
            {
                var width = Data.Caption.Width(Theme.Plot.Caption.Size);
                var height = Data.Caption.Height(Theme.Plot.Caption.Size);

                Caption.Y = Height - Theme.Plot.Caption.Margin.Bottom;
                Caption.Width = Theme.Plot.Caption.Margin.Left + width + Theme.Plot.Caption.Margin.Right;
                Caption.Height = Theme.Plot.Caption.Margin.Top + height + Theme.Plot.Caption.Margin.Bottom;

                wrapper.Height -= Caption.Height;
            }

            if (Data.Legends.Count > 0)
            {
                var (width, height) = Data.Legends.Dimension();

                if (width > 0 && height > 0)
                {
                    if (Theme.Legend.Position == Right)
                    {
                        Legend.X = Width - width - Theme.Legend.Margin.Right;
                        Legend.Y = wrapper.Y + (wrapper.Height - height) / 2.0;
                        Legend.Width = Theme.Legend.Margin.Left + width + Theme.Legend.Margin.Right;
                        Legend.Height = wrapper.Height;

                        wrapper.Width -= Legend.Width;
                    }
                    else if (Theme.Legend.Position == Left)
                    {
                        Legend.X = Theme.Legend.Margin.Left;
                        Legend.Y = wrapper.Y + (wrapper.Height - height) / 2.0; ;
                        Legend.Width = Theme.Legend.Margin.Left + width + Theme.Legend.Margin.Right;
                        Legend.Height = wrapper.Height;

                        wrapper.X += Legend.Width;
                        wrapper.Width -= Legend.Width;
                    }
                    else if (Theme.Legend.Position == Top)
                    {
                        Legend.X = wrapper.X + (wrapper.Width - width) / 2.0;
                        Legend.Y = wrapper.Y + Theme.Legend.Margin.Top;
                        Legend.Width = wrapper.Width;
                        Legend.Height = Theme.Legend.Margin.Top + height + Theme.Legend.Margin.Bottom;

                        wrapper.Y += Legend.Height;
                        wrapper.Height -= Legend.Height;
                    }
                    else if (Theme.Legend.Position == Bottom)
                    {
                        Legend.X = wrapper.X + (wrapper.Width - width) / 2.0;
                        Legend.Y = wrapper.Y + wrapper.Height - height - Theme.Legend.Margin.Bottom;
                        Legend.Width = wrapper.Width;
                        Legend.Height = Theme.Legend.Margin.Top + height + Theme.Legend.Margin.Bottom;

                        wrapper.Height -= Legend.Height;
                    }
                }
            }

            if (Caption.Width > 0)
            {
                Caption.X = wrapper.X + wrapper.Width - Theme.Plot.Caption.Margin.Right;
            }

            if (!firstRender)
            {
                for (var i = 0; i < Data.Panels.Count; i++)
                {
                    Data.Panels[i].Component.Refresh();
                }
            }
        }

        public override void Render() => Render(false);
    }
}
