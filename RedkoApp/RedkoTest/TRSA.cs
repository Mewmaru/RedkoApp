using RedkoLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using System.IO;
using System.Text;

namespace RedkoTest
{ 
    [TestClass]
    public class TRSA
    {
        //Проверяет, получаем ли мы строку, эквивалентную исходной, после зашифровки и расшифровки
        //По сути, полное тестирование, но в случае успешного прохождения теста EncryptedEqualeStandard 
        //автоматически становится проверкой только расшифровки
        [TestMethod]
        public void RSASourceEqualeResult()
        {
            RedkoLib.cypherRSAclass bufferRSA = new RedkoLib.cypherRSAclass();
            RedkoLib.RSAKey publicKey = new RedkoLib.RSAKey(17, 249);
            RedkoLib.RSAKey privateKey = new RedkoLib.RSAKey(29, 249);
            Stream encryptedDataStream = new MemoryStream();
            Stream decryptedDataStream = new MemoryStream();
            string sourceDataString = "test";
            MemoryStream sourceDataStream = new MemoryStream(Encoding.Default.GetBytes(sourceDataString));
            bufferRSA.dataToRsa(sourceDataStream, encryptedDataStream, publicKey);
            encryptedDataStream.Position = 0;
            bufferRSA.rsaToData(encryptedDataStream, decryptedDataStream, privateKey);
            StreamReader reader = new StreamReader(decryptedDataStream);
            decryptedDataStream.Position = 0;
            string decryptedDataString = reader.ReadToEnd();
            Assert.AreEqual(sourceDataString, decryptedDataString);
        }

        //Проверка соответствия зашифрованной строки той, что должна была получиться при правильной реализации алгоритма
        [TestMethod]
        public void RSAEncryptedEqualeStandard()
        {
            RedkoLib.cypherRSAclass bufferRSA = new RedkoLib.cypherRSAclass();
            RedkoLib.RSAKey publicKey = new RedkoLib.RSAKey(17, 249);
            RedkoLib.RSAKey privateKey = new RedkoLib.RSAKey(29, 249);
            Stream encryptedDataStream = new MemoryStream();
            string sourceDataString = "test";
            MemoryStream sourceDataStream = new MemoryStream(Encoding.Default.GetBytes(sourceDataString));
            bufferRSA.dataToRsa(sourceDataStream, encryptedDataStream, publicKey);
            encryptedDataStream.Position = 0;
            StreamReader reader = new StreamReader(encryptedDataStream);
            string encryptedDataString = reader.ReadToEnd();
            Assert.AreEqual(encryptedDataString, "_ [_");
        }
    }
}
