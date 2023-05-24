using SharedProject.Base;
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

public class Video : BaseVideo<BaseTexture, BaseRenderTarget<BaseTexture>>
{
    public Video(GL gl, string path, InternalFormat internalFormat, uint renderTargetSize) : base(gl, path, internalFormat, renderTargetSize)
    {
    }


    public static new IVideo Init(GL gl, string path, InternalFormat internalFormat)
    {
        return new Video(gl, path, internalFormat, 4);
    }

    public unsafe new void BindAndApplyShader()
    {
        //Gl.Clear(ClearBufferMask.ColorBufferBit);
        RenderTarget.Bind();
        DrawBuffer.Bind();
        ShaderCentrloids.Use();
        Texture.Bind();
        ShaderCentrloids.SetUniform("uTexture0", 0);
        ShaderCentrloids.SetUniform3("cent1", KMeans[0, 0], KMeans[0, 1], KMeans[0, 2]);
        ShaderCentrloids.SetUniform3("cent2", KMeans[1, 0], KMeans[1, 1], KMeans[1, 2]);
        ShaderCentrloids.SetUniform3("cent3", KMeans[2, 0], KMeans[2, 1], KMeans[2, 2]);
        var color = video.Frames[FramePosition][10, 10];
        ShaderCentrloids.SetUniform3("cent4", color.R, color.G, color.B);

        Gl.DrawElements(PrimitiveType.Triangles, (uint)DrawBuffer.Indices.Length, DrawElementsType.UnsignedInt, null);
        RenderTarget.UnBind();
        KMeans = RenderTarget.RecalculateAndGetAvrColor();

        if (IsNaNAbleKMeans)
        {
            KMeansUnsetNaN();
        }
    }
}