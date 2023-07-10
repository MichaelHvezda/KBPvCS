// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using PPBvCS.Kmeans;
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

namespace PPBvCS
{
    class Program
    {
        private static IWindow window;
        private static GL Gl;

        //Our new abstracted objects, here we specify what the types are.

        private static ITexture Texture;
        private static ITexture Texture1;
        private static ITexture Texture2;
        private static ITexture Texture3;
        private static ITexture Texture4;
        private static void Main(string[] args)
        {
            //BenchmarkRunner.Run<TextureChange>();
            //BenchmarkRunner.Run<TextureCreation>();
            BenchmarkRunner.Run<VL>();
            //BenchmarkRunner.Run<SL>();
            //BenchmarkRunner.Run<EMGU>();
            //BenchmarkRunner.Run<VideoPlayerBase>();
            //BenchmarkRunner.Run<WF>();
            //Init();

        }

        private static void Init()
        {
            var options = WindowOptions.Default;
            options.Size = new Vector2D<int>(800, 600);
            options.Title = "LearnOpenGL with Silk.NET";
            window = Silk.NET.Windowing.Window.Create(options);

            window.IsVisible = false;
            window.Initialize();
            OnLoad();
            OnClose();
            window.Run();
        }

        private static unsafe void OnLoad()
        {
            Gl = GL.GetApi(window);
            Texture = new SharedProject.Implementation.AvgTexture(Gl, ResourcesProvider.Big, InternalFormat.Rgba8);
            Texture1 = new SharedProject.Implementation.AvgTexture(Gl, ResourcesProvider.Big, InternalFormat.Rgba16);
            Texture2 = new SharedProject.Implementation.AvgTexture(Gl, ResourcesProvider.Big, InternalFormat.Rgba16f);
            Texture3 = new SharedProject.Implementation.AvgTexture(Gl, ResourcesProvider.Big, InternalFormat.Rgba32f);
            Texture4 = new SharedProject.Implementation.AvgTexture(Gl, ResourcesProvider.Big, InternalFormat.Rgba4);

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
            var avarage = sum / (width * height);

            Console.WriteLine($"avarage          {avarage:F4}");
            Console.WriteLine($"avarage          {(avarage / 255f):  0.0000}");
            Console.WriteLine($"texture avarage  {Texture4.AvgColor:  0.0000}");
            Console.WriteLine($"texture avarage  {Texture.AvgColor:  0.0000}");
            Console.WriteLine($"texture avarage  {Texture1.AvgColor:  0.0000}");
            Console.WriteLine($"texture avarage  {Texture2.AvgColor:  0.0000}");
            Console.WriteLine($"texture avarage  {Texture3.AvgColor:  0.0000}");

            VideoLoader videoLoader = new VideoLoader(ResourcesProvider.Video4K);
            var time = DateTime.Now;
            for (var i = 0; i <= 600; i++)
            {
                var ss = videoLoader.Load(out var bytes);
                Console.WriteLine(bytes.Length.ToString());
            }

            Console.WriteLine("shader {0}", (time - DateTime.Now).TotalMilliseconds);
            videoLoader.Dispose();
            Console.ReadLine();
            window.Close();
        }

        private static void OnClose()
        {
            //Remember to dispose all the instances.
            Gl?.Dispose();
            Texture?.Dispose();
        }
    }
}