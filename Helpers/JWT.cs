namespace devC_Jwt.Helpers
{
    public class JWT // this map the JWT configrations in appsettings.json
    {
        public string Key { get;set; }
        public string Issuer { get;set; }
        public string Audience { get;set; }
        public int DurationInDays { get;set; }
    }
}
