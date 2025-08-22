using ToDoWebAPI.Dtos;
using static ToDoWebAPI.Dtos.TokenDtos;

namespace ToDoWebAPI.Service.Interface
{
    public interface IUser
    {
        Task<ResponseDto<bool>> CreateUserAsync(CreateUserDto createUserDto);
        Task<ResponseDto<LoginResponseDto>> LoginAsync(LoginDto loginDto);
        Task<ResponseDto<UserDto>> GetUserByIdAsync(int userId);
        Task<ResponseDto<List<UserDto>>> GetAllUsersAsync();
        Task<ResponseDto<bool>> DeleteUserAsync(string userId);
        Task<ResponseDto<bool>> UpdateUserAsync(string userId, UpdateUserDto userDto);

        Task<ResponseDto<bool>> ChangeUserPassword(ChangePasswordDto changePasswordDto);

        Task<ResponseDto<LoginResponseDto>> RefreshToken(RefreshTokenRequestDto refreshTokenDto);


    }
}
