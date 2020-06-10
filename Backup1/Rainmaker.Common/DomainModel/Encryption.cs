using System;
using System.Collections.Generic;
using System.Text;

namespace Rainmaker.Common.DomainModel
{
    public class Encryption
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Encryption"/> class.
        /// </summary>
        public Encryption()
        { }

        /// <summary>
        /// Encrypts the specified string to encrypt.
        /// </summary>
        /// <param name="StringToEncrypt">The string to encrypt.</param>
        /// <returns></returns>
        public string Encrypt(string StringToEncrypt)
        {

            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(StringToEncrypt);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            return s.ToString();

        }


    }
}
