using RedkoLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using System.IO;
using System.Text;

namespace RedkoTest
{
    [TestClass]
    public class TSign
    {
        [TestMethod]
        public void SignAndVerify()
        {
            string sourceDataString = "test";
            MemoryStream input = new MemoryStream(Encoding.Default.GetBytes(sourceDataString));
            MemoryStream output = new MemoryStream();

            RedkoLib.SignatureRSA rsa = new RedkoLib.SignatureRSA();
            rsa.RSA_Params();
            rsa.SetHashFunction(new RedkoLib.MD5());
            rsa.Sign(input, output);

            Assert.IsTrue(rsa.Verify(output));
        }
    }
}
