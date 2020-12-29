using GGNet.Coords;
using GGNet.Data;
using Microsoft.AspNetCore.Components;

namespace GGNet.Components.Labeling
{
    public class AxisBase : ComponentBase
    {
        [CascadingParameter]
        public Theme Theme { get; set; }

        [Parameter]
        public Zone Area { get; set; }

        [Parameter]
        public ICoord Coord { get; set; }

        [Parameter]
        public string ClipId { get; set; }

        [Parameter]
        public Axis Axis { get; set; }

        [Parameter]
        public double Space { get; set; }

        [Parameter]
        public RenderChildPolicyBase RenderPolicy { get; set; }

        protected Zone zone;

        protected override bool ShouldRender() => RenderPolicy.ShouldRender();
    }
}
