using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERIShArp.File;

namespace ERIShArp
{
    class Program
    {
        static void Main(string[] args)
        {
            ERISAArchive arc = new ERISAArchive();
            arc.Open(System.IO.File.OpenRead(args[0]));

            arc.Close();
        }
    }
}
