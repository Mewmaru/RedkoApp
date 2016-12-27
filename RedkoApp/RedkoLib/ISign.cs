using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RedkoLib
{
    public interface ISign
    {
        void Sign(Stream input, Stream output);
        void SetKey(Stream key);
        void SetHashFunction(IMD5 hash);
        bool Verify(Stream input);
    }
}
