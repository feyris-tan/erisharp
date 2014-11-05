using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERIShArp
{
    public interface IChecksum
    {
        ulong Checksum
        {
            get;
        }
    }
}
