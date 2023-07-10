using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
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
using System.IO;
using PixelFormat = Silk.NET.OpenGL.PixelFormat;

namespace PPBvCS
{
    [MemoryDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    //[MinIterationCount(100)]
    //[MaxIterationCount(125)]
    [MinIterationCount(2)]
    [MaxIterationCount(5)]
    [AllStatisticsColumn]
    [Outliers(Perfolizer.Mathematics.OutlierDetection.OutlierMode.DontRemove)]
    public class TextureCreation
    {
        public static IWindow window;
        public static GL Gl;

        //Our new abstracted objects, here we specify what the types are.

        public static ITexture Texture0;
        public static ITexture Texture1;
        public static ITexture Texture2;
        public static ITexture Texture3;
        public static ITexture Texture4;
        public static ITexture Texture5;
        public static ITexture Texture6;
        public static ITexture Texture7;
        public static ITexture Texture8;
        public static ITexture Texture9;
        public static ITexture Texture10;
        public static ITexture Texture11;
        public static ITexture Texture12;
        public static ITexture Texture13;
        public static byte[] ImageBytes;
        public static int Width;
        public static int Height;
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
            var image = Image.Load<Rgba32>(ResourcesProvider.Big);
            ImageBytes = new byte[image.Width * image.Height * 4];
            image.CopyPixelDataTo(ImageBytes);
            Width = image.Width;
            Height = image.Height;
            image.Dispose();
        }

        [Benchmark]
        public unsafe Vector3D<float> Cpu()
        {
            using var image = Image.Load<Rgba32>(ResourcesProvider.Big);

            var width = image.Width;
            var height = image.Height;
            Vector3D<float> sum = new();
            for (int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    var col = image[w, h];
                    sum += new Vector3D<float>() { X = col.R, Y = col.G, Z = col.B };
                }
            }
            return sum / (width * height);
        }
        [Benchmark]
        public unsafe Vector4D<float> Rgba4()
        {

            fixed (void* d = &ImageBytes[0])
                Texture4 = new SharedProject.Implementation.AvgTexture(Gl, d, (uint)Height, (uint)Width, InternalFormat.Rgba4);
            return Texture4.AvgColor;
        }
        [Benchmark]
        public unsafe Vector4D<float> Rgba8()
        {

            fixed (void* d = &ImageBytes[0])
                Texture0 = new SharedProject.Implementation.AvgTexture(Gl, d, (uint)Height, (uint)Width, InternalFormat.Rgba8);
            return Texture0.AvgColor;
        }

        [Benchmark]
        public unsafe Vector4D<float> Rgba16fBgr()
        {

            fixed (void* d = &ImageBytes[0])
                Texture8 = new SharedProject.Implementation.AvgTexture(Gl, d, (uint)Height, (uint)Width, InternalFormat.Rgba16f, PixelFormat.Bgr);
            return Texture8.AvgColor;
        }
        [Benchmark]
        public unsafe Vector4D<float> Rgba16fRgb()
        {

            fixed (void* d = &ImageBytes[0])
                Texture12 = new SharedProject.Implementation.AvgTexture(Gl, d, (uint)Height, (uint)Width, InternalFormat.Rgba16f, PixelFormat.Rgb);
            return Texture12.AvgColor;
        }
        [Benchmark]
        public unsafe Vector4D<float> Rgba16fBgra()
        {

            fixed (void* d = &ImageBytes[0])
                Texture9 = new SharedProject.Implementation.AvgTexture(Gl, d, (uint)Height, (uint)Width, InternalFormat.Rgba16f, PixelFormat.Bgra);
            return Texture9.AvgColor;
        }
        [Benchmark]
        public unsafe Vector4D<float> Rgba8Bgr()
        {

            fixed (void* d = &ImageBytes[0])
                Texture10 = new SharedProject.Implementation.AvgTexture(Gl, d, (uint)Height, (uint)Width, InternalFormat.Rgba8, PixelFormat.Bgr);
            return Texture10.AvgColor;
        }
        [Benchmark]
        public unsafe Vector4D<float> Rgba8Rgb()
        {

            fixed (void* d = &ImageBytes[0])
                Texture13 = new SharedProject.Implementation.AvgTexture(Gl, d, (uint)Height, (uint)Width, InternalFormat.Rgba8, PixelFormat.Rgb);
            return Texture13.AvgColor;
        }
        [Benchmark]
        public unsafe Vector4D<float> Rgba8Bgra()
        {

            fixed (void* d = &ImageBytes[0])
                Texture11 = new SharedProject.Implementation.AvgTexture(Gl, d, (uint)Height, (uint)Width, InternalFormat.Rgba8, PixelFormat.Bgra);
            return Texture11.AvgColor;
        }
        [Benchmark]
        public unsafe Vector4D<float> Rgba16()
        {

            fixed (void* d = &ImageBytes[0])
                Texture1 = new SharedProject.Implementation.AvgTexture(Gl, d, (uint)Height, (uint)Width, InternalFormat.Rgba16);
            return Texture1.AvgColor;
        }
        [Benchmark]
        public unsafe Vector4D<float> Rgba16fFromFile()
        {

            Texture2 = new SharedProject.Implementation.AvgTexture(Gl, ResourcesProvider.Big, InternalFormat.Rgba16f);
            return Texture2.AvgColor;
        }
        [Benchmark]
        public unsafe Vector4D<float> Rgba32f()
        {

            fixed (void* d = &ImageBytes[0])
                Texture3 = new SharedProject.Implementation.AvgTexture(Gl, d, (uint)Height, (uint)Width, InternalFormat.Rgba32f);
            return Texture3.AvgColor;
        }
        [Benchmark(Baseline = true)]
        public unsafe Vector4D<float> Rgba16fPointer()
        {
            fixed (void* d = &ImageBytes[0])
                Texture5 = new SharedProject.Implementation.AvgTexture(Gl, d, (uint)Height, (uint)Width, InternalFormat.Rgba16f);
            return Texture5.AvgColor;
        }

        [Benchmark]
        public unsafe Vector4D<float> Rgba8SNorm()
        {

            fixed (void* d = &ImageBytes[0])
                Texture6 = new SharedProject.Implementation.AvgTexture(Gl, d, (uint)Height, (uint)Width, InternalFormat.Rgba8SNorm);
            return Texture6.AvgColor;
        }

        [Benchmark]
        public unsafe Vector4D<float> Rgba16SNorm()
        {

            fixed (void* d = &ImageBytes[0])
                Texture7 = new SharedProject.Implementation.AvgTexture(Gl, d, (uint)Height, (uint)Width, InternalFormat.Rgba16SNorm);
            return Texture7.AvgColor;
        }

        [IterationCleanup]
        public void IterClean()
        {
            //Remember to dispose all the instances.
            Texture0?.Dispose();
            Texture1?.Dispose();
            Texture2?.Dispose();
            Texture3?.Dispose();
            Texture4?.Dispose();
            Texture5?.Dispose();
            Texture6?.Dispose();
            Texture7?.Dispose();
            Texture8?.Dispose();
            Texture9?.Dispose();
            Texture10?.Dispose();
            Texture11?.Dispose();
            Texture12?.Dispose();
            Texture13?.Dispose();
            //RenderTarget.Dispose();
        }

        [GlobalCleanup]
        public void Dispose()
        {
            //Remember to dispose all the instances.
            Texture0?.Dispose();
            Texture1?.Dispose();
            Texture2?.Dispose();
            Texture3?.Dispose();
            Texture4?.Dispose();
            Texture5?.Dispose();
            Texture6?.Dispose();
            Texture7?.Dispose();
            Texture8?.Dispose();
            Texture9?.Dispose();
            Texture10?.Dispose();
            Texture11?.Dispose();
            Texture12?.Dispose();
            Texture13?.Dispose();
            Gl?.Dispose();
            window?.Dispose();
            //RenderTarget.Dispose();
        }
    }
}
