using Services.Functions;

namespace ServicesTests.Functions
{
    [TestClass()]
    public class JwtTokenServiceTests
    {
        //https://generate-random.org/encryption-key-generator?count=1&bytes=32&cipher=aes-256-cbc&string=&password=
        private const string JwtKey = "iezfxheYc3rduxaqQ+OXQNkbp0MAfZs4jU/8nU+c3isVuvcOdFPV1TzLDIy9X6oe";
        private readonly JwtTokenService _jwtTokenService;

        public JwtTokenServiceTests()
        {
            _jwtTokenService = new JwtTokenService(JwtKey);
        }

        [TestMethod()]
        public void GetUidFromToken_ShouldReturnCorrectUid()
        {
            int uid = 123;
            string email = "test@example.com";
            DateTime expireDt = DateTime.UtcNow.AddHours(1);
            string token = _jwtTokenService.GenerateToken(uid, email, expireDt);

            int? extractedUid = _jwtTokenService.GetUidFromToken(token);

            Assert.IsNotNull(extractedUid);
            Assert.AreEqual(uid, extractedUid);
        }
    }
}