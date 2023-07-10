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
    [MinIterationCount(100)]
    [MaxIterationCount(125)]
    [AllStatisticsColumn]
    [Outliers(Perfolizer.Mathematics.OutlierDetection.OutlierMode.DontRemove)]
    public class TextureChange
    {
        public static IWindow window;
        public static GL Gl;

        //Our new abstracted objects, here we specify what the types are.

        public static ITexture Texture0;
        public static ITexture Texture1;
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

        private static int NumberOfIter = 100;
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

            fixed (void* d = &ImageBytes[0])
                Texture4 = new SharedProject.Implementation.AvgTexture(Gl, d, (uint)Height, (uint)Width, InternalFormat.Rgb8, PixelFormat.Rgb);

            fixed (void* d = &ImageBytes[0])
                Texture0 = new SharedProject.Implementation.AvgTexture(Gl, d, (uint)Height, (uint)Width, InternalFormat.Rgb8, PixelFormat.Rgb);

            fixed (void* d = &ImageBytes[0])
                Texture8 = new SharedProject.Implementation.AvgTexture(Gl, d, (uint)Height, (uint)Width, InternalFormat.Rgba16f, PixelFormat.Bgr);

            fixed (void* d = &ImageBytes[0])
                Texture12 = new SharedProject.Implementation.AvgTexture(Gl, d, (uint)Height, (uint)Width, InternalFormat.Rgba16f, PixelFormat.Rgb);

            fixed (void* d = &ImageBytes[0])
                Texture9 = new SharedProject.Implementation.AvgTexture(Gl, d, (uint)Height, (uint)Width, InternalFormat.Rgba16f, PixelFormat.Bgra);

            fixed (void* d = &ImageBytes[0])
                Texture10 = new SharedProject.Implementation.AvgTexture(Gl, d, (uint)Height, (uint)Width, InternalFormat.Rgba8, PixelFormat.Bgr);

            fixed (void* d = &ImageBytes[0])
                Texture13 = new SharedProject.Implementation.AvgTexture(Gl, d, (uint)Height, (uint)Width, InternalFormat.Rgba8, PixelFormat.Rgb);

            fixed (void* d = &ImageBytes[0])
                Texture11 = new SharedProject.Implementation.AvgTexture(Gl, d, (uint)Height, (uint)Width, InternalFormat.Rgba8, PixelFormat.Bgra);

            fixed (void* d = &ImageBytes[0])
                Texture1 = new SharedProject.Implementation.AvgTexture(Gl, d, (uint)Height, (uint)Width, InternalFormat.Rgba16);

            fixed (void* d = &ImageBytes[0])
                Texture3 = new SharedProject.Implementation.AvgTexture(Gl, d, (uint)Height, (uint)Width, InternalFormat.Rgba32f);

            fixed (void* d = &ImageBytes[0])
                Texture5 = new SharedProject.Implementation.AvgTexture(Gl, d, (uint)Height, (uint)Width, InternalFormat.Rgba16f);

            fixed (void* d = &ImageBytes[0])
                Texture6 = new SharedProject.Implementation.AvgTexture(Gl, d, (uint)Height, (uint)Width, InternalFormat.Rgba8SNorm);

            fixed (void* d = &ImageBytes[0])
                Texture7 = new SharedProject.Implementation.AvgTexture(Gl, d, (uint)Height, (uint)Width, InternalFormat.Rgba16SNorm);
        }

        [Benchmark]
        public unsafe void Rgba4()
        {
            for (int i = 0; i < NumberOfIter; i++)
            {
                fixed (void* d = &ImageBytes[0])
                    Texture4.ChangeContent(d);
            }
        }
        [Benchmark]
        public unsafe void Rgba8()
        {
            for (int i = 0; i < NumberOfIter; i++)
            {
                fixed (void* d = &ImageBytes[0])
                    Texture0.ChangeContent(d);
            }
        }

        [Benchmark]
        public unsafe void Rgba16fBgr()
        {
            for (int i = 0; i < NumberOfIter; i++)
            {
                fixed (void* d = &ImageBytes[0])
                    Texture8.ChangeContent(d);
            }
        }

        [Benchmark]
        public unsafe void Rgba16fRgb()
        {
            for (int i = 0; i < NumberOfIter; i++)
            {
                fixed (void* d = &ImageBytes[0])
                    Texture12.ChangeContent(d);
            }
        }
        [Benchmark]
        public unsafe void Rgba16fBgra()
        {
            for (int i = 0; i < NumberOfIter; i++)
            {
                fixed (void* d = &ImageBytes[0])
                    Texture9.ChangeContent(d);
            }
        }
        [Benchmark]
        public unsafe void Rgba8Bgr()
        {
            for (int i = 0; i < NumberOfIter; i++)
            {
                fixed (void* d = &ImageBytes[0])
                    Texture10.ChangeContent(d);
            }
        }

        [Benchmark]
        public unsafe void Rgba8Rgb()
        {
            for (int i = 0; i < NumberOfIter; i++)
            {
                fixed (void* d = &ImageBytes[0])
                    Texture13.ChangeContent(d);
            }
        }

        [Benchmark]
        public unsafe void Rgba8Bgra()
        {
            for (int i = 0; i < NumberOfIter; i++)
            {
                fixed (void* d = &ImageBytes[0])
                    Texture11.ChangeContent(d);
            }
        }
        [Benchmark]
        public unsafe void Rgba16()
        {
            for (int i = 0; i < NumberOfIter; i++)
            {
                fixed (void* d = &ImageBytes[0])
                    Texture1.ChangeContent(d);
            }
        }

        [Benchmark]
        public unsafe void Rgba32f()
        {
            for (int i = 0; i < NumberOfIter; i++)
            {
                fixed (void* d = &ImageBytes[0])
                    Texture3.ChangeContent(d);
            }
        }
        [Benchmark(Baseline = true)]
        public unsafe void Rgba16fPointer()
        {
            for (int i = 0; i < NumberOfIter; i++)
            {
                fixed (void* d = &ImageBytes[0])
                    Texture5.ChangeContent(d);
            }
        }

        [GlobalCleanup]
        public void Dispose()
        {
            //Remember to dispose all the instances.
            Texture0?.Dispose();
            Texture1?.Dispose();
            Texture3?.Dispose();
            Texture4?.Dispose();
            Texture5?.Dispose();
            Texture6?.Dispose();
            Texture7?.Dispose();
            Texture8?.Dispose();
            Texture9?.Dispose();
            Texture10?.Dispose();
            Texture11?.Dispose();
            Gl?.Dispose();
            window?.Dispose();
        }
    }
}
