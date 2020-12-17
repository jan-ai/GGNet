using System;
using System.Collections.Generic;
using System.Text;

namespace GGNet.Data
{
    public class Axis
    {
        public IEnumerable<double> Breaks { get; set; } = Array.Empty<double>();

        public IEnumerable<double> MinorBreaks { get; set; } = Array.Empty<double>();

        public IEnumerable<(double, string label)> Labels { get; set; } = Array.Empty<(double, string label)>();

        public IEnumerable<(double, string title)> Titles { get; set; } = Array.Empty<(double, string title)>();

        public double Width { get; set; }
        public double Height { get; set; }
        public double TitlesSize { get; set; }
        
        public bool TitlesVisibility => TitlesSize > 0.0;

        public bool Show { get; set; }
    }
}
