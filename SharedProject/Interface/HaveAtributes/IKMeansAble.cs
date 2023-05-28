using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedProject.Interface.HaveAtributes
{
    public interface IKMeansAble
    {
        float[,] KMeans { get; set; }
        bool IsNaNAbleKMeans { get; set; }
    }
}
