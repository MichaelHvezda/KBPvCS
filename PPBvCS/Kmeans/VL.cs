using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using PPBvCS.Providers;
using SharedProject.Base;
using SharedProject.Implementation;
using SharedProject.Interface;
using SharedProject.Video;
using SharedResProject;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.SDL;
using Silk.NET.Windowing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Data;
using System.IO;
using PixelFormat = Silk.NET.OpenGL.PixelFormat;

namespace PPBvCS.Kmeans
{
    [MemoryDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    [MinIterationCount(100)]
    [MaxIterationCount(125)]
    [AllStatisticsColumn]
    [Outliers(Perfolizer.Mathematics.OutlierDetection.OutlierMode.DontRemove)]
    public class VL
    {
        private static readonly float BlueH = 200 / 360f;
        public static IWindow window;
        public static GL Gl;
        private static SharedResProject.Shader Shader;
        private static DrawBuffer DrawBufferr;
        private static ITexture Texture;
        private static Providers.VLVideo Video;

        [GlobalSetup]
        public unsafe void Init()
        {
            var options = WindowOptions.Default;
            options.Size = new Vector2D<int>(800, 600);
            options.Title = "LearnOpenGL with Silk.NET";
            window = Silk.NET.Windowing.Window.Create(options);
            window.IsVisible = false;
            window.Initialize();

            Gl = GL.GetApi(window);

            //Instantiating our new abstractions
            DrawBufferr = new(Gl);
            Shader = new(Gl, "../../../../../shaders/kmean");
            Texture = new SharedProject.Implementation.Texture(Gl, ResourcesProvider.Back, InternalFormat.Rgba8);
            Video = new (Gl, ResourcesProvider.Video_4K, InternalFormat.Rgba8, 3);
        }

        [Benchmark]
        public unsafe void OnRender()
        {
            Gl.Clear(ClearBufferMask.ColorBufferBit);

            DrawBufferr.Bind();

            Shader.Use();
            //Bind a texture and and set the uTexture0 to use texture0.

            Gl.Viewport(window.Size);
            Video.Texture.Bind(TextureUnit.Texture0);
            Shader.SetUniform("uTexture0", 0);

            Texture.Bind(TextureUnit.Texture1);
            Shader.SetUniform("uTexture1", 1);

            Video.RenderTarget.ColorBuffers[Video.GetBGTextureId(BlueH)].Bind(TextureUnit.Texture2);
            Shader.SetUniform("uTexture2", 2);

            Gl.DrawElements(PrimitiveType.Triangles, (uint)DrawBuffer.Indices.Length, DrawElementsType.UnsignedInt, null);

            Video.NextFrame();
            Video.BindAndApplyShader();
        }


        [GlobalCleanup]
        public void Dispose()
        {
            //Remember to dispose all the instances.
            DrawBufferr?.Dispose();
            Shader?.Dispose();
            Texture?.Dispose();
            Video?.Dispose();
            Gl?.Dispose();
        }
    }
}
