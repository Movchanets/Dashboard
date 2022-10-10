using Dashboard.Data.Data.Models;

namespace Dashboard.Services
{
    public class ServiceResponse
    {
        public  string? Message { get; set; }
        public bool IsSuccess { get; set; }
        public IEnumerable<string>? Errors { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }

        public object? Payload { get; set; }
       
    }
}
