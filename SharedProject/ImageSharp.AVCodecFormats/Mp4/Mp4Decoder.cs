using FFMediaToolkit.Decoding;
using FFMediaToolkit.Graphics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

using DrawingSize = System.Drawing.Size;

namespace HeyRed.ImageSharp.AVCodecFormats.Mp4
{
    public sealed class Mp4Decoder : BaseAVDecoder
    {
        public Mp4Decoder() : base()
        {
        }

        private readonly IAVDecoderOptions? _options;
        public Mp4Decoder(IAVDecoderOptions decoderOptions) : base(decoderOptions)
        {
            _options = decoderOptions;
        }

        public override Image<TPixel> Decode<TPixel>(Configuration configuration, Stream stream, CancellationToken cancellationToken)
        {
            DrawingSize? targetFrameSize = CalculateTargetSize(configuration, stream);

            using var file = MediaFile.Open(stream, new MediaOptions
            {
                // Map imagesharp pixel format to ffmpeg pixel format
                VideoPixelFormat = MapPixelFormat(default(TPixel)),
                TargetVideoSize = targetFrameSize,
                DemuxerOptions = new ContainerOptions
                {
                    FlagDiscardCorrupt = true,
                },
                StreamsToLoad = MediaMode.Video,
            });

            Image<TPixel> img = null!;
            ImageData lastDecodedFrame = default;

            int decodedFrames = 0;
            while (file.Video.TryGetNextFrame(out var frame))
            {
                if(decodedFrames== 0)
                {
                    img = Image.LoadPixelData<TPixel>(frame.Data, frame.ImageSize.Width, frame.ImageSize.Height);
                }


                decodedFrames++;

                lastDecodedFrame = frame;
                img.Frames.AddFrame(Image.LoadPixelData<TPixel>(lastDecodedFrame.Data, lastDecodedFrame.ImageSize.Width, lastDecodedFrame.ImageSize.Height).Frames[0]);

                if(decodedFrames > 500)
                    break;
            }

            return img;
        }

        private DrawingSize? CalculateTargetSize(Configuration configuration, Stream stream)
        {
            DrawingSize? targetFrameSize = null;
            if (_options?.FrameSizeOptions != null)
            {
                (int targetWidth, int targetHeight) = _options.FrameSizeOptions.TargetFrameSize;
                // Calculate frames size with aspect ratio
                if (_options.FrameSizeOptions.PreserveAspectRatio)
                {
                    IImageInfo? sourceInfo = Identify(configuration, stream, CancellationToken.None);
                    if (sourceInfo is not null)
                    {
                        int size = Math.Max(targetWidth, targetHeight);
                        targetFrameSize = CalculateSizeWithAspectRatio(new Size(sourceInfo.Width, sourceInfo.Height), size);
                    }

                    if (stream.CanSeek)
                    {
                        stream.Position = 0;
                    }
                }
                else
                {
                    targetFrameSize = new DrawingSize(targetWidth, targetHeight);
                }
            }
            return targetFrameSize;
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
        private DrawingSize CalculateSizeWithAspectRatio(Size sourceSize, int size)
        {
            double ratio = 1;
            if (sourceSize.Width > size || sourceSize.Height > size)
            {
                var ratioX = size / (double)sourceSize.Width;
                var ratioY = size / (double)sourceSize.Height;
                ratio = Math.Min(ratioX, ratioY);
            }

            return new(
                (int)Math.Round(sourceSize.Width * ratio),
                (int)Math.Round(sourceSize.Height * ratio));
        }
    }
}