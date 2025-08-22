using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using ToDoWebAPI.Data.Models;
using ToDoWebAPI.Dtos;
using ToDoWebAPI.Service.Interface;

namespace ToDoWebAPI.Service.Implimentation
{
    public class UserService : IUser
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly IUserEmailStore<User> _emailStore;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;

        public UserService(
            UserManager<User> userManager,
            IUserStore<User> userStore,
            IUserEmailStore<User> emailStore,
            SignInManager<User> signInManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = emailStore;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }




        public async Task<ResponseDto<bool>> CreateUserAsync(CreateUserDto createUserDto)
        {
            var userExisits = await _userManager.FindByEmailAsync(createUserDto.Email);
            if (userExisits != null)
            {
                return ResponseDto<bool>.Failure("User already exists");
            }
            var user = new User
            {
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
                DateOfBirth = createUserDto.DateOfBirth
            };
            await _userStore.SetUserNameAsync(user, createUserDto.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, createUserDto.Email, CancellationToken.None);

            var result = await _userManager.CreateAsync(user, createUserDto.Password);
            if (result.Succeeded)
            {
                return ResponseDto<bool>.SuccessResponse(true, "User created successfully");
            }
            var errors = result.Errors.Select(e => new ApiError
            {
                ErrorCode = e.Code,
                ErrorMessage = e.Description
            }).ToList();


            return ResponseDto<bool>.Failure("User creation failed", errors);

        }

        public async Task<ResponseDto<bool>> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return ResponseDto<bool>.Failure("User not found.");
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return ResponseDto<bool>.SuccessResponse(true, "User deleted successfully.");
            }
            var errors = result.Errors.Select(e => new ApiError
            {
                ErrorCode = e.Code,
                ErrorMessage = e.Description
            }).ToList();

            return ResponseDto<bool>.Failure("User deletion failed.", errors);
        }

        public async Task<ResponseDto<List<UserDto>>> GetAllUsersAsync()
        {
            return ResponseDto<List<UserDto>>.SuccessResponse(
                _userManager.Users.Select(u => new UserDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    DateOfBirth = u.DateOfBirth,
                    Email = u.Email
                }).ToList(), "Users retrieved successfully.");
        }

        public async Task<ResponseDto<UserDto>> GetUserByIdAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return ResponseDto<UserDto>.Failure("User not found.");
            }
            var userDto = new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Email = user.Email
            };
            return ResponseDto<UserDto>.SuccessResponse(userDto, "User retrieved successfully.");

        }

        public async Task<ResponseDto<LoginResponseDto>> LoginAsync(LoginDto loginDto)
        {
            var userexitist = await _userManager.FindByEmailAsync(loginDto.Email);
            if (userexitist == null)
            {
                return ResponseDto<LoginResponseDto>.Failure("User does not exist");
            }
            var result = await _signInManager.PasswordSignInAsync(userexitist, loginDto.Password, false, false);
            if (result.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(userexitist);


                var token = _tokenService.GenerateAccessToken(userexitist, roles.ToList());
                var refreshToken = _tokenService.GenerateRrefreshToken();
                userexitist.RefreshToken = refreshToken.RefreshToken;
                userexitist.RefreshTokenExpiryTime = refreshToken.RefreshTokenExipirityDate;

                await _userManager.UpdateAsync(userexitist);

                var loginResponse = new LoginResponseDto
                {
                    DisplayName = $"{userexitist.FirstName} {userexitist.LastName}",
                    Email = userexitist.Email,
                    AccessToken = token,
                    RefreshToken = refreshToken.RefreshToken,
                    RefreshTokenExpiryTime = refreshToken.RefreshTokenExipirityDate
                };
                return ResponseDto<LoginResponseDto>.SuccessResponse(loginResponse, "Login successful");
            }

            return ResponseDto<LoginResponseDto>.Failure("Login failed, please check your credentials");
        }

        public async Task<ResponseDto<bool>> UpdateUserAsync(string userId, UpdateUserDto userDto)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return ResponseDto<bool>.Failure("User not found.");
            }

            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.DateOfBirth = userDto.DateOfBirth;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return ResponseDto<bool>.SuccessResponse(true, "User updated successfully.");
            }

            var errors = result.Errors.Select(e => new ApiError
            {
                ErrorCode = e.Code,
                ErrorMessage = e.Description
            }).ToList();

            return ResponseDto<bool>.Failure("User update failed.", errors);
        }

        public async Task<ResponseDto<bool>> ChangeUserPassword(ChangePasswordDto changePasswordDto)
        {
            var user = await _userManager.FindByIdAsync(changePasswordDto.UserId);
            if (user == null)
            {
                return ResponseDto<bool>.Failure("User does not exist");
            }

            var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.OldPassword, changePasswordDto.NewPassword);
            if (result.Succeeded)
            {
                return ResponseDto<bool>.SuccessResponse(true, "Password changed successfully");
            }
            var errors = result.Errors.Select(e => new ApiError
            {
                ErrorCode = e.Code,
                ErrorMessage = e.Description
            }).ToList();
            return ResponseDto<bool>.Failure("Password change failed", errors);


        }

        public async Task<ResponseDto<LoginResponseDto>> RefreshToken(TokenDtos.RefreshTokenRequestDto refreshTokenDto)
        {
            var claimPrincipal = _tokenService.GetClaimsPrincipal(refreshTokenDto.AccessToken);
            if (claimPrincipal == null)
            {
                return ResponseDto<LoginResponseDto>.Failure("Invalid access token");
            }

            var userId = claimPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user.RefreshToken != refreshTokenDto.RefreshToken || user.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                return ResponseDto<LoginResponseDto>.Failure("Invalid or expired refresh token");
            }

            if (user == null)
            {
                return ResponseDto<LoginResponseDto>.Failure("User does not exist");
            }

            var roles = await _userManager.GetRolesAsync(user);


            var token = _tokenService.GenerateAccessToken(user, roles.ToList());
            var refreshToken = _tokenService.GenerateRrefreshToken();
            user.RefreshToken = refreshToken.RefreshToken;
            user.RefreshTokenExpiryTime = refreshToken.RefreshTokenExipirityDate;

            await _userManager.UpdateAsync(user);

            var loginResponse = new LoginResponseDto
            {
                DisplayName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                AccessToken = token,
                RefreshToken = refreshToken.RefreshToken,
                RefreshTokenExpiryTime = refreshToken.RefreshTokenExipirityDate
            };
            return ResponseDto<LoginResponseDto>.SuccessResponse(loginResponse, "Login successful");

        }
    }
}
