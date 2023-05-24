using System;
using System.Collections.Generic;
using System.Text;

namespace SharedResProject
{
    public static class ResourcesProvider
    {
        private static string StaticPath = Path.GetFullPath("../../../../SharedProject/resources/");
        private static string StaticHDPath = Path.GetFullPath("../../../../SharedProject/resources/hd");

        public static string Video1 = Path.Combine(StaticPath, "video1.mp4");
        public static string Video2 = Path.Combine(StaticPath, "video2.mp4");
        public static string Video3 = Path.Combine(StaticPath, "video3.mp4");
        public static string Cat = Path.Combine(StaticPath, "cat.mp4");
        public static string Img = Path.Combine(StaticPath, "img.jpg");
        public static string Big = Path.Combine(StaticPath, "big.jpg");
        public static string Back = Path.Combine(StaticPath, "back1.jpg");

        public static string Video_4K = Path.Combine(StaticHDPath, "video_4K.mp4");
        public static string Video_HD = Path.Combine(StaticHDPath, "video_HD.mp4");
        public static string Video4K = Path.Combine(StaticHDPath, "video4K.mp4");
        public static string VideoHD = Path.Combine(StaticHDPath, "videoHD.mp4");

    }
}
