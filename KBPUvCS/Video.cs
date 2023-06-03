using SharedProject.Base;
using SharedProject.Implementation;
using SharedProject.Interface;
using Silk.NET.OpenGL;
using Silk.NET.SDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KBPUvCS
{
    public class Video : AvrSLVideo
    {
        public Video(GL gl, string path, InternalFormat internalFormat, uint renderTargetSize) : base(gl, path, internalFormat, renderTargetSize)
        {
        }

        public uint BGTextureId { get; set; } = 0;

        public uint GetBGTextureId(float bgVal)
        {
            List<(float, uint)> values = new()
            {
                (Math.Abs(base.KMeans[0][0] - bgVal), 0),
                (Math.Abs(base.KMeans[1][0] - bgVal), 1),
                (Math.Abs(base.KMeans[2][0] - bgVal), 2)
            };

            return values.OrderBy(p => p.Item1).First().Item2;
        }
    }
}
