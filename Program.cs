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
    class Program
    {
        static void Main(string[] args)
        {
            ERISAArchive arc = new ERISAArchive();
            arc.Open(new PhysicalFile(System.IO.File.OpenRead(args[0])));
            ERISAArchive.EDirectory files = arc.ReferFileEntries();

            ERIFile test = null;
            foreach (KeyValuePair<string, ERISAArchive.FILE_INFO> file in files)
            {
                test = new ERIFile();
                test.Open(arc.OpenFileObject(file.Key, 0));
                ERISADecodeContext ctx = new ERISADecodeContext((uint)file.Value.nBytes);
                ctx.AttachInputFile(test);
                ERISADecoder decoder = new ERISADecoder();
                decoder.Initalize(test.m_InfoHeader);
                EGL_IMAGE_INFO target = new EGL_IMAGE_INFO(test.m_InfoHeader);
                decoder.DecodeImage(target, ctx);
                test.Close();  
            }
            arc.Close();
        }
    }
}
