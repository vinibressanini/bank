namespace desafioAPI.Models
{
    public record Authorization
    {
        public string Status { get; set; }
      
        public bool isAuthorized() => Status == "success";
    }
}
