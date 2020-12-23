using System;
using System.Collections.Generic;
using System.Text;
using GGNet.Coords;
using GGNet.Data;
using Microsoft.AspNetCore.Components;

namespace GGNet.Components
{
    public partial class Grid : ComponentBase
    {
        [CascadingParameter]
        public Theme Theme { get; set; }

        [Parameter]
        public string ClipId { get; set; }

        [Parameter]
        public Zone Zone { get; set; }

        [Parameter]
        public ICoord Coord { get; set; }

        [Parameter]
        public Axis XAxis { get; set; }

        [Parameter]
        public Axis YAxis { get; set; }
    }
}
