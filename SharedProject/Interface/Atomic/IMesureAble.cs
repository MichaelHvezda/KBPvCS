using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedProject.Interface.Atomic
{
    public interface IMesureAble
    {
        public uint Height { get; set; }
        public uint Width { get; set; }
    }
}
