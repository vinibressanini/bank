using System.Text.Json.Serialization;

namespace desafioAPI.Models
{
    public class User : BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        [JsonIgnore]
        public string Password { get; set; } = string.Empty;
        [JsonIgnore]
        public Wallet Wallet { get; set; }

    }
}
