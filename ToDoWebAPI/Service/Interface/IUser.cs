using ToDoWebAPI.Dtos.Requests;
using ToDoWebAPI.Dtos.Responses;
using static ToDoWebAPI.Dtos.System.TokenDtos;

namespace ToDoWebAPI.Service.Interface
{
    public interface IUser
    {
        Task<ResponseDto<bool>> CreateUserAsync(CreateUserDto createUserDto);
        Task<ResponseDto<bool>> CreateUserWithRoleAsync(CreateUserDto createUserDto, string role);
        Task<ResponseDto<LoginResponseDto>> LoginAsync(LoginDto loginDto);
        Task<ResponseDto<UserDto>> GetUserByIdAsync(int userId);
        Task<ResponseDto<List<UserDto>>> GetAllUsersAsync();
        Task<ResponseDto<bool>> DeleteUserAsync(string userId);
        Task<ResponseDto<bool>> UpdateUserAsync(string userId, UpdateUserDto userDto);

        Task<ResponseDto<bool>> ChangeUserPassword(ChangePasswordDto changePasswordDto);

        Task<ResponseDto<LoginResponseDto>> RefreshToken(RefreshTokenRequestDto refreshTokenDto);


    }
}
