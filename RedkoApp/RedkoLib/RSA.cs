using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Numerics;

namespace RedkoLib
{
    public struct RSAKey
    {
        public double first;
        public double n;
        public RSAKey(double first1, double n1)
        {
            first = first1;
            n = n1;
        }
    }
    public class cypherRSAclass : IRSA
    {       
        public void dataToRsa(Stream sourceData, Stream returnData, RSAKey publicKey)
        {
            RSAClass bufferRSA = new RSAClass();
            int sourceByte;
            while ((sourceByte = sourceData.ReadByte()) != -1)
            {
                returnData.WriteByte(bufferRSA.RSADataCrypt((byte)sourceByte, publicKey));
            }
        }

        public void rsaToData(Stream sourceData, Stream returnData, RSAKey privateKey)
        {
            RSAClass bufferRSA = new RSAClass();
            int sourceByte;
            while ((sourceByte = sourceData.ReadByte()) != -1)
            {
                returnData.WriteByte(bufferRSA.RSADataDecrypt((byte)sourceByte, privateKey));
            }
        }
    }

    public class RSAClass
    {
        public byte RSADataCrypt(byte sourceByte, RSAKey publicKey)
        {
            BigInteger result = BigInteger.Pow(sourceByte, (int)publicKey.first) % (BigInteger)publicKey.n;
            return (byte)result;
        }

        public byte RSADataDecrypt(byte sourceByte, RSAKey privateKey)
        {
            BigInteger result = BigInteger.Pow(sourceByte, (int)privateKey.first) % (BigInteger)privateKey.n;
            return (byte)result;
        }
    }
}