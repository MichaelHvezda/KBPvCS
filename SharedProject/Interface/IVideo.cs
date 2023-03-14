using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedProject.Interface
{
    internal interface IVideo
    {
        void NextFrame();
        void Init( string path, InternalFormat internalFormat);
    }
}
