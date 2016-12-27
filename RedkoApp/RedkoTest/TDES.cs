using RedkoLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using System.IO;
using System.Text;

namespace RedkoTest
{
    [TestClass()]
    public class TDES
    {       
        [TestMethod()]
        public void TestDES()
        {
            string data = "test message";
            string key = "f93hnt57w";

            byte[] inpByteArray = Encoding.Default.GetBytes(data);
            MemoryStream inpStream = new MemoryStream(inpByteArray);
            byte[] keyByteArray = Encoding.Default.GetBytes(key);
            MemoryStream keyStream = new MemoryStream(keyByteArray);

            Stream encrStream = new MemoryStream();
            Stream decrStream = new MemoryStream();

            DES target = new DES(keyStream);

            target.Encrypt(inpStream, encrStream);
            target.Decrypt(encrStream, decrStream);

            byte[] buf = new byte[decrStream.Length];
            decrStream.Read(buf, 0, (int)decrStream.Length);
            string decrypted = Encoding.Default.GetString(buf, 0, buf.Length);

            Assert.AreEqual(data, decrypted);
        }

    }
}
