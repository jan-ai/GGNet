using Microsoft.AspNetCore.Components;

namespace GGNet.Components.Definitions
{
    public partial class Animation : ComponentBase
    {
        [CascadingParameter]
        public Theme Theme { get; set; }

        [Parameter]
        public string Id { get; set; }

        protected override bool ShouldRender() => false;
    }
}
