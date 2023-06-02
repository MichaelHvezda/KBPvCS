using SharedResProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedProject.Interface.HaveAtributes
{
    public interface IShaderAble
    {
        Shader Shader { get; set; }
        void BindAndApplyShader();
    }
}
