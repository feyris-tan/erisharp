﻿using System;
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
            ERISAArchive.EDirectory files = arc.ReferFileEntries();

            EMCFile test = null;
            foreach (KeyValuePair<string, ERISAArchive.FILE_INFO> file in files)
            {
                test = arc.OpenFileObject(file.Key, 0);
                uint length = test.GetLength();
                byte[] buffer = new byte[length];
            }
            arc.Close();
        }
    }
}
