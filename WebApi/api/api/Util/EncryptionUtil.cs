using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace api.Util
{
    public class EncryptionUtil
    {
        private SinaptIQPKCS7.PKCS7 pkcs7;

        public EncryptionUtil()
        {
            pkcs7 = new SinaptIQPKCS7.PKCS7();
        }

        public string encrypte(string text, string publicKey)
        {
            //Recipient's public key
            string base64Str = this.pkcs7.encryptMessage(text, pkcs7.getPublicCert(publicKey));

            return base64Str;
        }

        public string decrypte(string encText, string privateKey, string privateKey_pwd)
        {
            X509Certificate2 certificate;

            certificate = new X509Certificate2(@privateKey, privateKey_pwd, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable);
            //certificate = pkcs7.getPrivateCert(privateKey, privateKey_pwd);

            //Sending encrypted message to Recipient
            //Recipient's private key
            String clearMsg = this.pkcs7.decryptMessage(encText, certificate);

            return clearMsg;
        }
    }
}