using RedkoLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using System.IO;
using System.Text;

namespace RedkoTest
{   
    [TestClass]
    public class TMD5
    {
        [TestMethod]
        public void EmptyHashEquals()
        {
            string sourceDataString = "";
            MemoryStream input = new MemoryStream(Encoding.Default.GetBytes(sourceDataString));
            MemoryStream output = new MemoryStream();
            RedkoLib.MD5 md5 = new RedkoLib.MD5();
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            md5.GetHash(input, output);

            string myHash = Encoding.Default.GetString(output.ToArray()).TrimEnd('\0');
            Assert.AreEqual(comparer.Compare(myHash, "d41d8cd98f00b204e9800998ecf8427e"), 0);
        }

        [TestMethod]
        public void HashEquals()
        {
            string sourceDataString = "test";

            RedkoLib.MD5 md5 = new RedkoLib.MD5();
            MemoryStream input = new MemoryStream(Encoding.Default.GetBytes(sourceDataString));
            MemoryStream output = new MemoryStream();

            md5.GetHash(input, output);
            string myHash = Encoding.Default.GetString(output.ToArray()).TrimEnd('\0');

            System.Security.Cryptography.MD5 md5Hash = System.Security.Cryptography.MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(sourceDataString));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            string hash = sBuilder.ToString();

            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            Assert.AreEqual(comparer.Compare(myHash, hash), 0);

        }
    }
}
