using Cat.Encryption.Net35;
using Cat.Encryption.Net35.AES;
using System.IO;
using UnityEngine;

namespace Cat.Auth
{
    public class MacAuth
    {
        string _path;
        public MacAuth()
        {
            _path = Application.streamingAssetsPath + "/license";
        }
        public bool Verify()
        {
            byte[] buffer;
            using (FileStream fs = File.OpenRead(_path))
            {
                buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
            }
            CryptContext context = new CryptContext(new AESCrypt());
            string plainText = context.DecryptFromBytes(buffer);
            return Equals(plainText, MAC.GetMacAddress());
        }
    }

}
