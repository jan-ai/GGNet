using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace GGNet.Components.Labeling
{
    public partial class Strip : ComponentBase
    {
        [CascadingParameter]
        public Theme Theme { get; set; }

        [Parameter]
        public string ClipId { get; set; }

        [Parameter]
        public Data.Strip XStrip { get; set; }

        [Parameter]
        public Data.Strip YStrip { get; set; }
    }
}
