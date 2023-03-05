using HeyRed.ImageSharp.AVCodecFormats;
using HeyRed.ImageSharp.AVCodecFormats.Mp4;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KBPvCS
{
    public static class VideoConfiguration
    {
        public static Configuration GetConfiguration()
        {
            return new Configuration(new Mp4ConfigurationModule());
        }
    }
}
