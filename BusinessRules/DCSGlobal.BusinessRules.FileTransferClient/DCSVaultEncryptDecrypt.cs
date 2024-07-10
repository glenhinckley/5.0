using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

public class DCSVaultEncryptDecrypt
{
    private const string VAULT_ENCRYPT_DECRYPT_KEY = "&%#@?,:*#$$($#$#(%%%(%($(#$#(@(##)@$$(*%";

    public static string Decrypt(string strText)
    {
        byte[] byKey = {
		
	};
        byte[] IV = {
		0x0,
		0x2,
		0x4,
		0x6,
		0x8,
		0x10,
		0x12,
		0x14,
		0x16,
		0x18,
		0x20,
		0x22,
		0x24,
		0x26,
		0x28,
		0x30
	};

        try
        {
            byKey = System.Text.Encoding.UTF8.GetBytes(VAULT_ENCRYPT_DECRYPT_KEY.Substring(0, 16));
            RijndaelManaged rjnd = new RijndaelManaged();
            KeySizes ks = new KeySizes(16, 16, 2);
            byte[] inputByteArray = Convert.FromBase64String(strText);
            MemoryStream ms = new MemoryStream(strText.Length);
            CryptoStream cs = new CryptoStream(ms, rjnd.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            System.Text.Encoding encoding = System.Text.Encoding.UTF8;
            return encoding.GetString(ms.ToArray());
        }
        catch (System.Exception ex)
        {
            return ex.Message;
        }

    }

}

