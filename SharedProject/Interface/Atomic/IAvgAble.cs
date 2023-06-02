using Silk.NET.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedProject.Interface.Atomic
{
    public interface IAvgAble
    {
        void RecalculateAvrColor();
        public Vector4D<float> AvgColor { get; set; }
    }
}
