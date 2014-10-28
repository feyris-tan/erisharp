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
            arc.Open(new PhysicalFile(System.IO.File.OpenRead(args[0])));
            ERISAArchive.EDirectory files = arc.ReferFileEntries();

            ERIFile test = null;
            foreach (KeyValuePair<string, ERISAArchive.FILE_INFO> file in files)
            {
                test = new ERIFile();
                test.Open(arc.OpenFileObject(file.Key, 0));
                test.Close();  
            }
            arc.Close();
        }
    }
}
