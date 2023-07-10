using BenchmarkDotNet.Attributes;
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
    public class VideoPlayer
    {
        public static IWindow window;
        public static GL Gl;
        private static Providers.SLVideo SLVideo;
        private static Providers.EMGUVideo EMGUVideo;
        private static Providers.VLVideo VLVideo;
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
            SLVideo = new(Gl, ResourcesProvider.Video_4K, InternalFormat.Rgba8, 3);
            EMGUVideo = new(Gl, ResourcesProvider.Video_4K, InternalFormat.Rgba8, 3);
            VLVideo = new(Gl, ResourcesProvider.Video_4K, InternalFormat.Rgba8, 3);
        }

        [Benchmark]
        public unsafe void SLVideoFce()
        {
            for (int i = 0; i < NumberOfIter; i++)
            {
                SLVideo.NextFrame();
                SLVideo.BindAndApplyShader();
            }
        }

        [Benchmark]
        public unsafe void EMGUVideoFce()
        {
            for (int i = 0; i < NumberOfIter; i++)
            {
                SLVideo.NextFrame();
                SLVideo.BindAndApplyShader();
            }
        }

        [Benchmark]
        public unsafe void VLVideoFce()
        {
            for (int i = 0; i < NumberOfIter; i++)
            {
                SLVideo.NextFrame();
                SLVideo.BindAndApplyShader();
            }
        }

        [GlobalCleanup]
        public void Dispose()
        {
            SLVideo?.Dispose();
            EMGUVideo?.Dispose();
            VLVideo?.Dispose();
            Gl?.Dispose();
        }
    }
}
