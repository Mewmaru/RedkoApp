using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RedkoLib
{
    public class DES : IDES
    {
        string encryptKey;
        string decryptKey;
        string data;

        string[] Blocks;
        private const int blockSize = 64;

        private const int charSize = 16;
        private const int keyShift = 2;
        private const int roundNumber = 16;

        public DES(Stream inpKey)
        {
            byte[] buf = new byte[inpKey.Length];
            inpKey.Read(buf, 0, (int)inpKey.Length);
            encryptKey = Encoding.Default.GetString(buf, 0, buf.Length);
        }

        private string CompleteLength(string data)
        {
            while (((data.Length * charSize) % blockSize) != 0)
                data = "0" + data;

            return data;
        }

        private void SplitBinaryIntoBlocks(string data)
        {
            Blocks = new string[data.Length / blockSize];

            int lengthOfBlock = data.Length / Blocks.Length;

            for (int i = 0; i < Blocks.Length; i++)
                Blocks[i] = data.Substring(i * lengthOfBlock, lengthOfBlock);
        }

        private void SplitStringIntoBlocks(string data)
        {
            Blocks = new string[(data.Length * charSize) / blockSize];

            int lengthOfBlock = data.Length / Blocks.Length;

            for (int i = 0; i < Blocks.Length; i++)
            {
                Blocks[i] = data.Substring(i * lengthOfBlock, lengthOfBlock);
                Blocks[i] = StringToBinary(Blocks[i]);
            }
        }

        private string CompleteKeyLength(string key, int keyLength)
        {
            if (key.Length > keyLength)
                key = key.Substring(0, keyLength);
            else
                while (key.Length < keyLength)
                    key = "0" + key;

            return key;
        }

        private string EncodeOneRound(string data, string key)
        {
            string L = data.Substring(0, data.Length / 2);
            string R = data.Substring(data.Length / 2, data.Length / 2);

            return (R + XOR(L, AND(R, key)));
        }

        private string DecodeOneRound(string data, string key)
        {
            string L = data.Substring(0, data.Length / 2);
            string R = data.Substring(data.Length / 2, data.Length / 2);

            return (XOR(AND(L, key), R) + L);
        }

        private string AND(string s1, string s2)
        {
            string result = "";

            for (int i = 0; i < s1.Length; i++)
            {
                bool a = Convert.ToBoolean(Convert.ToInt32(s1[i].ToString()));
                bool b = Convert.ToBoolean(Convert.ToInt32(s2[i].ToString()));

                if (a & b)
                    result += "1";
                else
                    result += "0";
            }
            return result;
        }

        private string XOR(string s1, string s2)
        {
            string result = "";

            for (int i = 0; i < s1.Length; i++)
            {
                bool a = Convert.ToBoolean(Convert.ToInt32(s1[i].ToString()));
                bool b = Convert.ToBoolean(Convert.ToInt32(s2[i].ToString()));

                if (a ^ b)
                    result += "1";
                else
                    result += "0";
            }
            return result;
        }

        private string NextKey(string key)
        {
            for (int i = 0; i < keyShift; i++)
            {
                key = key[key.Length - 1] + key;
                key = key.Remove(key.Length - 1);
            }

            return key;
        }

        private string PrevKey(string key)
        {
            for (int i = 0; i < keyShift; i++)
            {
                key = key + key[0];
                key = key.Remove(0, 1);
            }

            return key;
        }

        private string StringToBinary(string data)
        {
            string result = "";

            for (int i = 0; i < data.Length; i++)
            {
                string binaryChar = Convert.ToString(data[i], 2);

                while (binaryChar.Length < charSize)
                    binaryChar = "0" + binaryChar;

                result += binaryChar;
            }

            return result;
        }

        private string BinaryToString(string data)
        {
            string result = "";

            while (data.Length > 0)
            {
                string char_binary = data.Substring(0, charSize);
                data = data.Remove(0, charSize);

                int a = 0;
                int degree = char_binary.Length - 1;

                foreach (char c in char_binary)
                    a += Convert.ToInt32(c.ToString()) * (int)Math.Pow(2, degree--);

                result += ((char)a).ToString();
            }

            return result;
        }

        public void Encrypt(Stream inp, Stream outp)
        {
            byte[] buf = new byte[inp.Length];
            inp.Read(buf, 0, (int)inp.Length);
            data = Encoding.Default.GetString(buf, 0, buf.Length);

            data = CompleteLength(data);
            SplitStringIntoBlocks(data);
            encryptKey = CompleteKeyLength(encryptKey, data.Length / (2 * Blocks.Length));
            encryptKey = StringToBinary(encryptKey);

            for (int j = 0; j < roundNumber; j++)
            {
                for (int i = 0; i < Blocks.Length; i++)
                    Blocks[i] = EncodeOneRound(Blocks[i], encryptKey);

                encryptKey = NextKey(encryptKey);
            }

            encryptKey = PrevKey(encryptKey);

            decryptKey = BinaryToString(encryptKey);

            String result = "";

            for (int i = 0; i < Blocks.Length; i++)
                result += Blocks[i];

            result = BinaryToString(result);

            byte[] byteArray = Encoding.Default.GetBytes(result);
            outp.Write(byteArray, 0, Convert.ToInt32(byteArray.Length));
            outp.Position = 0;
        }

        public void Decrypt(Stream inp, Stream outp)
        {
            byte[] buf = new byte[inp.Length];
            inp.Read(buf, 0, (int)inp.Length);
            data = Encoding.Default.GetString(buf, 0, buf.Length);

            decryptKey = StringToBinary(decryptKey);
            data = StringToBinary(data);

            SplitBinaryIntoBlocks(data);

            for (int j = 0; j < roundNumber; j++)
            {
                for (int i = 0; i < Blocks.Length; i++)
                    Blocks[i] = DecodeOneRound(Blocks[i], decryptKey);

                decryptKey = PrevKey(decryptKey);
            }

            decryptKey = NextKey(decryptKey);
            string result = "";

            for (int i = 0; i < Blocks.Length; i++)
                result += Blocks[i];
            result = BinaryToString(result);
            result = result.TrimStart('0');

            byte[] byteArray = Encoding.Default.GetBytes(result);
            outp.Write(byteArray, 0, byteArray.Length);
            outp.Position = 0;
        }

    }

}
