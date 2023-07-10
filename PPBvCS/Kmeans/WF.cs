﻿using BenchmarkDotNet.Attributes;
using SharedProject.Interface;
using SharedResProject;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPBvCS.Kmeans
{
    [MemoryDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    [MinIterationCount(500)]
    [MaxIterationCount(525)]
    [AllStatisticsColumn]
    [Outliers(Perfolizer.Mathematics.OutlierDetection.OutlierMode.DontRemove)]
    public class WF
    {
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
            Shader = new(Gl, "../../../../../shaders/WF/kmean");
            Texture = new SharedProject.Implementation.Texture(Gl, ResourcesProvider.Back, InternalFormat.Rgba8);
            Video = new(Gl, ResourcesProvider.Video3, InternalFormat.Rgba8, 3);
        }

        [Benchmark]
        public unsafe void OnRender()
        {
            Gl.Clear(ClearBufferMask.ColorBufferBit);

            DrawBufferr.Bind();

            Shader.Use();

            Gl.Viewport(window.Size);
            //Bind a texture and and set the uTexture0 to use texture0.

            Video.Texture.Bind(TextureUnit.Texture0);
            Shader.SetUniform("uTexture0", 0);

            Texture.Bind(TextureUnit.Texture1);
            Shader.SetUniform("uTexture1", 1);


            Shader.SetUniform("Saturation", 0 / 100);
            Shader.SetUniform("KeyColor", 0 * 360);
            Shader.SetUniform("Brightness", 0 / 100);
            Shader.SetUniform("Hue", 0);

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
