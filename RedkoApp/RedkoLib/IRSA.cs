using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RedkoLib
{
    public interface IRSA
    {
        void dataToRsa(Stream sourceData, Stream returnData, RSAKey publicKey);
        void rsaToData(Stream sourceData, Stream returnData, RSAKey privateKey);
    }

}
