using FFMediaToolkit.Decoding;
using FFMediaToolkit.Graphics;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedProject.Video
{
    public class VideoLoader : IDisposable
    {
        public MediaFile MediaFileData { get; set; }
        public int Width { get; internal set; }
        public int Height { get; internal set; }
        public int FrameCount { get; internal set; }

        public VideoLoader(string path)
        {
            MediaFileData = MediaFile.Open(path, new MediaOptions
            {
                // Map imagesharp pixel format to ffmpeg pixel format
                VideoPixelFormat = MapPixelFormat(default(Rgba32)),
                TargetVideoSize = null,
                DemuxerOptions = new ContainerOptions
                {
                    FlagDiscardCorrupt = true,
                },
                StreamsToLoad = MediaMode.Video,
            });
            Width = MediaFileData.Video.Info.FrameSize.Width;
            Height = MediaFileData.Video.Info.FrameSize.Height;
            FrameCount = MediaFileData.Video.Info.FrameCount;
        }

        public void Dispose()
        {
            MediaFileData?.Dispose();
        }

        public bool Load(out Span<byte> frames)
        {
            var res = MediaFileData.Video.TryGetNextFrame(out var frame);
            frames = frame.Data;
            return res && frames.Length is not 0;
        }
        public bool LoadFirst(out Span<byte> frames)
        {
            var res = MediaFileData.Video.TryGetFrame(TimeSpan.Zero, out var frame);
            frames = frame.Data;
            return res && frames.Length is not 0;
        }
        private ImagePixelFormat MapPixelFormat<TPixel>(TPixel sourcePixelFormat) => sourcePixelFormat switch
        {
            Rgb24 _ => ImagePixelFormat.Rgb24,
            Bgr24 _ => ImagePixelFormat.Bgr24,
            Rgba32 _ => ImagePixelFormat.Rgba32,
            Argb32 _ => ImagePixelFormat.Argb32,
            Bgra32 _ => ImagePixelFormat.Bgra32,
            _ => throw new ArgumentException("Unsupported pixel format."),
        };
    }
}
