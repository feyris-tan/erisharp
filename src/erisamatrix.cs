using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERIShArp.Matrix
{
    public enum DCTDegreeLimit
    {
        MIN_DCT_DEGREE = 2,
        MAX_DCT_DEGREE = 12,
    }

    public struct ERI_SIN_COS
    {
        float rSin;
        float rCos;
    }
}
