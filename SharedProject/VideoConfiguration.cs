using HeyRed.ImageSharp.AVCodecFormats;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedResProject
{
    public static class VideoConfiguration
    {
        public static Configuration GetConfiguration() => new Configuration(new HeyRed.ImageSharp.AVCodecFormats.Mp4.Mp4ConfigurationModule());
    }
}
