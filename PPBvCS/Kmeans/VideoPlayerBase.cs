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
    public class VideoPlayerBase
    {
        public static IWindow window;
        public static GL Gl;
        private static SharedProject.Implementation.SLVideo SLVideoBase;
        private static SharedProject.Implementation.EMGUVideo EMGUVideoBase;
        private static SharedProject.Implementation.VLVideo VLVideoBase;
        private static int NumberOfIter = 200;

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

            VLVideoBase = new(Gl, ResourcesProvider.Video_4K, InternalFormat.Rgba8, 3);
        }

        [Benchmark]
        public unsafe void SLVideoBaseFce()
        {
            for (int i = 0; i < NumberOfIter; i++)
            {
                SLVideoBase.NextFrame();
            }
        }

        [Benchmark]
        public unsafe void EMGUVideoBaseFce()
        {
            for (int i = 0; i < NumberOfIter; i++)
            {
                EMGUVideoBase.NextFrame();
            }
        }

        [Benchmark]
        public unsafe void VLVideoBaseFce()
        {
            for (int i = 0; i < NumberOfIter; i++)
            {
                VLVideoBase.NextFrame();
            }
        }
        [GlobalCleanup]
        public void Dispose()
        {
            SLVideoBase?.Dispose();
            EMGUVideoBase?.Dispose();
            VLVideoBase?.Dispose();
            Gl?.Dispose();
        }
    }
}
