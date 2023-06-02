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

namespace KBPU2vCS;

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
                (base.KMeans[0][2] - bgVal, 0),
                (base.KMeans[1][2] - bgVal, 1),
                (base.KMeans[2][2] - bgVal, 2)
            };

        return values.OrderByDescending(p => p.Item1).First().Item2;
    }
}