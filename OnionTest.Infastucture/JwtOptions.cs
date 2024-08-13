namespace OnionTest.Infastucture
{
    public class JwtOptions
    {
        public string SecretKey { get; set; } = string.Empty;
        public int ExpitesHours { get; set; }
        public string ValidIssuer {  get; set; } = string.Empty;
        public string ValidAudience {  get; set; } = string.Empty;
    }
}