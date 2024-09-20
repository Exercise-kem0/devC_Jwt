namespace devC_Jwt.Models
{
    public class AuthenModel
    {
        public string Message { get; set; }
        public bool IsAuthenticated { get; set; } //false by default 
        public string Username { get; set; }
        public string Email {  get; set; }
        public List<string> Roles { get; set; }
        public string Token {  get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}
