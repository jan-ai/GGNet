using System;
using System.Threading.Tasks;
using GGNet.Scales;
using Microsoft.AspNetCore.Components.Web;

namespace GGNet.Shapes
{
    public abstract class Shape
    {
        public string Classes { get; set; }

        public Func<MouseEventArgs, Task> OnClick { get; internal set; }

        public Func<MouseEventArgs, Task> OnMouseOver { get; internal set; }

        public Func<MouseEventArgs, Task> OnMouseOut { get; internal set; }

        public abstract void Scale<TX, TY>(Position<TX> ScaleX, Position<TY> ScaleY) where TX : struct where TY : struct;
    }
}
