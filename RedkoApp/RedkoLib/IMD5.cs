using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RedkoLib
{
    public interface IMD5
    {
        void GetHash(Stream input, Stream output);
    }
}
