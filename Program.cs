using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERIShArp.File;
using ERIShArp.Context;
using ERIShArp.Image;
using System.Drawing;

namespace ERIShArp
{
    /*class Program
    {
        static void Main(string[] args)
        {
            
            ERIFile test = new ERIFile();
            test.Open(new PhysicalFile(System.IO.File.OpenRead(args[0])));
            ERISADecodeContext ctx = new ERISADecodeContext((uint)test.GetLargeLength());
            ctx.AttachInputFile(test);
            ERISADecoder decoder = new ERISADecoder();
            decoder.Initalize(test.m_InfoHeader);
            EGL_IMAGE_INFO target = new EGL_IMAGE_INFO(test.m_InfoHeader);
            decoder.DecodeImage(target, ctx);
            System.IO.File.WriteAllBytes("none.dmp", target.ptrImageArray.Data);
            test.Close();
        }
    }*/
}
