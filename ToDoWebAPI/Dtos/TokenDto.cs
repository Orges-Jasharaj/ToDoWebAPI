namespace ToDoWebAPI.Dtos
{
    public class TokenDtos
    {
        public class RefreshTokenDto
        {
            public string RefreshToken { get; set; } = string.Empty;
            public DateTime RefreshTokenExipirityDate { get; set; }
        }

        public class RefreshTokenRequestDto
        {
            public string RefreshToken { get; set; } = string.Empty;
            public string AccessToken { get; set; } = string.Empty;
        }

    }
}
