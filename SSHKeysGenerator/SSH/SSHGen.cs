using Chilkat;
using SSHKeysGenerator.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSHKeysGenerator.SSH
{
    public static class SSHGen
    {
        private static Random _rng = new Random();
        private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+=-<>?.,:`[]{}|";

        public static string Generate(string name, string folder, out string password, out bool error)
        {
            error = false;

            SshKey key = new SshKey();

            bool success;

            int numBits;
            int exponent;

            string exportedKey;
            bool exportEncrypted;
            password = GenPassword();

            string fullPath = $"{folder}\\{name}";

            //  numBits may range from 384 to 4096.  Typical values are
            //  1024 or 2048.  (must be a multiple of 64)
            //  A good choice for the exponent is 65537.  Chilkat recommends
            //  always using this value.
            numBits = 2048;
            exponent = 65537;
            success = key.GenerateRsaKey(numBits, exponent);
            if (success != true)
            {
                error = true;
                return string.Empty;
            }

            //  Note: Generating a public/private key pair is CPU intensive
            //  and may take a short amount of time (more than few seconds,
            //  but less than a minute).           

            string exportFile = string.Empty;

            string extension = ".pem";
            int i = 0;
            do
            {
                if(i == 0)
                    exportFile = fullPath + extension;
                else
                    exportFile = fullPath + $"-{i}{extension}";

                i++;
            }
            while (FoldersInit.CheckIfFileExist(exportFile));

            //  Export with encryption to OpenSSH private key format:
            key.Password = password;
            exportEncrypted = true;
            exportedKey = key.ToOpenSshPrivateKey(exportEncrypted);
            success = key.SaveText(exportedKey, exportFile);

            extension = ".ppk";
            i = 0;
            do
            {
                if (i == 0)
                    exportFile = fullPath + extension;
                else
                    exportFile = fullPath + $"-{i}{extension}";

                i++;
            }
            while (FoldersInit.CheckIfFileExist(exportFile));

            //  Export the RSA private key to encrypted PuTTY format:
            key.Password = password;
            exportEncrypted = true;
            exportedKey = key.ToPuttyPrivateKey(exportEncrypted);
            success = key.SaveText(exportedKey, exportFile);


            //  ----------------------------------------------------
            //  Now for the public key....
            //  ----------------------------------------------------

            //  The Secure Shell (SSH) Public Key File Format
            //  is documented in RFC 4716.
            extension = "_pubkey_rfc4716.pub";
            i = 0;
            do
            {
                if (i == 0)
                    exportFile = fullPath + extension;
                else
                    exportFile = fullPath + $"-{i}{extension}";

                i++;
            }
            while (FoldersInit.CheckIfFileExist(exportFile));

            exportedKey = key.ToRfc4716PublicKey();
            success = key.SaveText(exportedKey, exportFile);

            //  OpenSSH has a separate public-key file format, which
            //  is also supported by Chilkat SshKey:

            extension = "_pubkey_openSsh.pub";
            i = 0;
            do
            {
                if (i == 0)
                    exportFile = fullPath + extension;
                else
                    exportFile = fullPath + $"-{i}{extension}";

                i++;
            }
            while (FoldersInit.CheckIfFileExist(exportFile));

            exportedKey = key.ToOpenSshPublicKey();
            success = key.SaveText(exportedKey, exportFile);

            return exportedKey;

        }

        private static string GenPassword()
        {
            int size = 30;
            char[] buffer = new char[size];

            for (int i = 0; i < size; i++)
            {
                buffer[i] = _chars[_rng.Next(_chars.Length)];
            }
            return new string(buffer);

        }
    }
}
