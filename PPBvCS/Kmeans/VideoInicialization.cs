using BenchmarkDotNet.Attributes;
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
    [MinIterationCount(50)]
    [MaxIterationCount(55)]
    [AllStatisticsColumn]
    [Outliers(Perfolizer.Mathematics.OutlierDetection.OutlierMode.DontRemove)]
    public class VideoInicialization
    {
        public static IWindow window;
        public static GL Gl;
        private static Providers.SLVideo SLVideo;
        private static Providers.EMGUVideo EMGUVideo;
        private static Providers.VLVideo VLVideo;

        private static SharedProject.Implementation.SLVideo SLVideoBase;
        private static SharedProject.Implementation.EMGUVideo EMGUVideoBase;
        private static SharedProject.Implementation.VLVideo VLVideoBase;
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
        }

        [Benchmark]
        public unsafe void SLVideoFce()
        {
            SLVideo = new(Gl, ResourcesProvider.Video_4K, InternalFormat.Rgba8, 3);
        }

        [Benchmark]
        public unsafe void EMGUVideoFce()
        {
            EMGUVideo = new(Gl, ResourcesProvider.Video_4K, InternalFormat.Rgba8, 3);
        }

        [Benchmark]
        public unsafe void VLVideoFce()
        {
            VLVideo = new(Gl, ResourcesProvider.Video_4K, InternalFormat.Rgba8, 3);
        }

        [Benchmark]
        public unsafe void SLVideoBaseFce()
        {
            SLVideoBase = new(Gl, ResourcesProvider.Video_4K, InternalFormat.Rgba8, 3);
        }

        [Benchmark]
        public unsafe void EMGUVideoBaseFce()
        {
            EMGUVideoBase = new(Gl, ResourcesProvider.Video_4K, InternalFormat.Rgba8, 3);
        }

        [Benchmark]
        public unsafe void VLVideoBaseFce()
        {
            VLVideoBase = new(Gl, ResourcesProvider.Video_4K, InternalFormat.Rgba8, 3);
        }

        [IterationCleanup]
        public void IterDispose()
        {

            SLVideo?.Dispose();
            EMGUVideo?.Dispose();
            VLVideo?.Dispose();
            SLVideoBase?.Dispose();
            EMGUVideoBase?.Dispose();
            VLVideoBase?.Dispose();
            //RenderTarget.Dispose();
        }

        [GlobalCleanup]
        public void Dispose()
        {

            SLVideo?.Dispose();
            EMGUVideo?.Dispose();
            VLVideo?.Dispose();
            Gl?.Dispose();
            //RenderTarget.Dispose();
        }
    }
}
