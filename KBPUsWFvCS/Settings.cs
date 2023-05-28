using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KBPUsWFvCS
{
    public static class Settings
    {

        public static readonly float GreenH = 95 / 360f;
        public static readonly float BlueH = 220 / 360f;
        public static float Saturation { get; set; } = 0;
        public static float KeyColor { get; set; } = 0;
        public static float Brightness { get; set; } = 0;
        public static float Hue { get; set; } = 0;
    }
}
