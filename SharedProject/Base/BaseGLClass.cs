using SharedProject.Interface.Atomic;
using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedProject.Base
{
    public class BaseGLClass : IGLAble, IInitAble
    {
        public GL Gl { get; set; }
        public bool Inicialized { get; set; }

        public BaseGLClass(GL gl, bool inicialized)
        {
            this.Gl = gl;
            this.Inicialized = inicialized;
        }
    }
}
