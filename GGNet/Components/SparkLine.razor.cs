using Microsoft.AspNetCore.Components;

using GGNet.Scales;
using GGNet.Transformations;

namespace GGNet.Components
{
    public partial class SparkLine<T, TX, TY> : PlotBase<T, TX, TY>, IPanel
        where TX : struct
        where TY : struct
    {
        [Parameter]
        public double Width { get; set; } = 150;

        [Parameter]
        public double Height { get; set; } = 50;

        private RenderChildPolicyBase renderChildPolicy;

        private Zone Area;
        private Data<T, TX, TY>.Panel Panel;

        protected Tooltips.SparkLine tooltip;
        public ITooltip Tooltip => tooltip;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            renderChildPolicy = Policy.Child();

            Area.Width = Width;
            Area.Height = Height;

            Data.Init(false);

            Data.Render(true);

            Panel = Data.Panels[0];

            Panel.Register(this);
        }

        public override void Render()
        {
            Data.Render(false);
            renderChildPolicy.Refresh();
        }
    }
}
