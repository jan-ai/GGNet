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

            var labelZone = Label.Zone(textSettings);

            Zone.X = zone.X + (_alignment == TextAlignment.BottomLeft ? margin.Left : zone.Width - margin.Right - labelZone.Width);
            Zone.Y = zone.Y + margin.Top + (_alignment == TextAlignment.BottomLeft ? labelZone.Height : 0);
            Zone.Width = margin.Left + labelZone.Width + margin.Right;
            Zone.Height = margin.Top + labelZone.Height + margin.Bottom;

            return true;
        }
    }
}
