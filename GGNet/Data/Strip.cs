using System;
using System.Collections.Generic;
using System.Text;
using GGNet.Elements;

namespace GGNet.Data
{
    public class Strip
    {
        public enum TextAlignment
        {
            BottomLeft,
            TopRight
        }

        private TextAlignment _alignment;
        public Zone Zone;

        public Strip (TextAlignment alignment)
        {
            _alignment = alignment;
        }

        public string Label { get; set; }

        public bool SetZone (Text textSettings, Margin margin, Zone zone) 
        {
            if (string.IsNullOrEmpty(Label))
                return false;

            var width = Label.Width(textSettings.Size);
            var height = Label.Height(textSettings.Size);

            Zone.X = zone.X + (_alignment == TextAlignment.BottomLeft ? margin.Left : zone.Width - margin.Right - width);
            Zone.Y = zone.Y + margin.Top + (_alignment == TextAlignment.BottomLeft ? height : 0);
            Zone.Width = margin.Left + width + margin.Right;
            Zone.Height = margin.Top + height + margin.Bottom;

            return true;
        }
    }
}
