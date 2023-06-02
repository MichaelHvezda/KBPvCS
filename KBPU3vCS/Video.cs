using SharedProject.Base;
using SharedProject.Implementation;
using SharedProject.Interface;
using SharedResProject;
using Silk.NET.OpenGL;
using Silk.NET.SDL;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KBPU21vCS;

public class Video : AvrSLVideo
{
    public Video(GL gl, string path, InternalFormat internalFormat, uint renderTargetSize) : base(gl, path, internalFormat, renderTargetSize)
    {
    }

    public unsafe new void BindAndApplyShader()
    {
        //Gl.Clear(ClearBufferMask.ColorBufferBit);
        RenderTarget.Bind();
        DrawBuffer.Bind();
        Shader.Use();
        Texture.Bind();
        Shader.SetUniform("uTexture0", 0);
        Shader.SetUniformVec3("cent1", KMeans[0]);
        Shader.SetUniformVec3("cent2", KMeans[1]);
        Shader.SetUniformVec3("cent3", KMeans[2]);
        var color = VideoData.Frames[FramePosition][10, 10];
        Shader.SetUniform3("cent4", color.R, color.G, color.B);

        Gl.DrawElements(PrimitiveType.Triangles, (uint)DrawBuffer.Indices.Length, DrawElementsType.UnsignedInt, null);
        RenderTarget.UnBind();
        KMeans = RenderTarget.RecalculateAndGetAvrColor();

        if (IsNaNAbleKMeans)
        {
            KMeansUnsetNaN();
        }
    }
}