using SharedProject.Interface.Atomic;
using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedProject.Base
{
    public class BaseGLClass : IGLAble, IDisposable
    {
        public GL Gl { get; set; }

        public BaseGLClass(GL gl)
        {
            this.Gl = gl;
        }

        public virtual void Dispose()
        {

        }
    }
}
