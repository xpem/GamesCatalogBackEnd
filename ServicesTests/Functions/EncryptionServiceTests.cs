using Services.Functions;

namespace ServicesTests.Functions;

[TestClass()]
public class EncryptionServiceTests
{
    public static EncryptionService BuildEncryptionService()
    {
        string key32 = "12345678901234567890123456789012";
        string IV16 = "1234567890123456";

        return new EncryptionService(key32, IV16);
    }

    [TestMethod()]
    public void CompareEncryptionTest()
    {
        string password = "131313";

        var encryptionService = BuildEncryptionService();

        string encryptedString = encryptionService.Encrypt(password);

        string decryptedString = encryptionService.Decrypt(encryptedString);

        Assert.AreEqual(password, decryptedString);
    }

    [TestMethod()]
    public void Compare_Diferent_EncryptionTest()
    {
        string passworda = "123456";

        var encryptionService = BuildEncryptionService();

        string encryptedaString = encryptionService.Encrypt(passworda);

        string passwordb = "654321";

        string encryptedbString = encryptionService.Encrypt(passwordb);

        Assert.AreNotEqual(encryptedaString, encryptedbString);
    }

    [TestMethod()]
    public void DecrypTest()
    {
        string encrypted = "CwY0Vg1K6wNlHSMLMDy2Fw==";
        string decrypted = "131313";
        var encryptionService = BuildEncryptionService();
        string DecryptedString = encryptionService.Decrypt(encrypted);

        Assert.AreEqual(DecryptedString, decrypted);
    }

    [TestMethod()]
    public void EncrypTest()
    {
        string decrypted = "131313";
        string encrypted = "CwY0Vg1K6wNlHSMLMDy2Fw==";
        var encryptionService = BuildEncryptionService();
        string EncryptedString = encryptionService.Encrypt(decrypted);

        Assert.AreEqual(EncryptedString, encrypted);
    }
}    