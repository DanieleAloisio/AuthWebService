namespace AuthWebService.Dto
{
    public class JwtTokenDto
    {
        public JwtTokenDto(string token)
        {
            this.token = token;
        }

        public string token { get; set; }
    }
}
