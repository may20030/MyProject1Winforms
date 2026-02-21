using ECommerceManagement.Application.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceManagement.Application.Interfaces.Services
{
    public interface IUserService
    {

        Task<List<UserDTO>> GetAllAsync();
        Task<UserDTO> GetByIdAsync(int id);
        Task CreateAsync(CreateUserDTO dto);
        Task DeleteAsync(int id);
        Task<UserDTO> LoginAsync(LoginDTO dto);
        Task<UserDTO> GetByUsernameAsync(string username);
        Task<string?> ValidateUserAsync(CreateUserDTO dto, string confirmPassword);
    }
}
