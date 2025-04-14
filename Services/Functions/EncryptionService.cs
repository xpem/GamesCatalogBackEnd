using System.Security.Cryptography;
using System.Text;

namespace Services.Functions;

public interface IEncryptionService
{
    string Decrypt(string cipherText);
    string Encrypt(string plainText);
}

public class EncryptionService(string Key32, string IV16) : IEncryptionService
{
    private readonly byte[] Key = Encoding.UTF8.GetBytes(Key32);
    private readonly byte[] IV = Encoding.UTF8.GetBytes(IV16);

    public string Encrypt(string plainText)
    {
        string encrypted = "";

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using MemoryStream msEncrypt = new();
            using (CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write))
            using (StreamWriter swEncrypt = new(csEncrypt))
            {
                swEncrypt.Write(plainText);
            }
            encrypted = Convert.ToBase64String(msEncrypt.ToArray());
        }

        return encrypted;
    }

    public string Decrypt(string cipherText)
    {
        try
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            using Aes aesAlg = Aes.Create();
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using MemoryStream msDecrypt = new(cipherBytes);
            using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
            using StreamReader srDecrypt = new(csDecrypt);
            string plainText = srDecrypt.ReadToEnd();
            return plainText;
        }
        catch (Exception /*ex*/) { throw; }
    }
}
