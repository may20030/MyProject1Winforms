using ECommerceManagement.Application.DTOs.UserDTOs;
using ECommerceManagement.Application.Interfaces.Repositories;
using ECommerceManagement.Application.Interfaces.Services;
using ECommerceManagement.Domain.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ECommerceManagement.Application.Services
{
    public class UserService : IUserService
    {
        private  IGenericRepository<User, int> _repository;

        public UserService(IGenericRepository<User, int> repository)
        {
            _repository = repository;
        }

        public async Task<List<UserDTO>> GetAllAsync()
        {
            var users = await _repository.GetAll().ToListAsync();
            return users.Adapt<List<UserDTO>>();
        }

        public async Task<UserDTO> GetByIdAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            return user?.Adapt<UserDTO>();
        }

        public async Task CreateAsync(CreateUserDTO dto)
        {
            var user = dto.Adapt<User>();
            await _repository.AddAsync(user);
        }

       
        public async Task DeleteAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null) return;

            _repository.Delete(user);
        }

        public async Task<UserDTO> LoginAsync(LoginDTO dto)
        {
            var user = await _repository.GetAll()
                .FirstOrDefaultAsync(u => u.Username == dto.Username && u.Password == dto.Password);

            return user?.Adapt<UserDTO>();
        }

        public async Task<UserDTO> GetByUsernameAsync(string username)
        {
            var user = await _repository.GetAll()
                .FirstOrDefaultAsync(u => u.Username == username);

            return user?.Adapt<UserDTO>();
        }

        public async Task<string?> ValidateUserAsync(CreateUserDTO dto, string confirmPassword)
        {
            // 1. تحقق من اسم المستخدم
            if (string.IsNullOrWhiteSpace(dto.Username) || dto.Username.Length < 3)
                return "Username must be at least 3 characters ❌";

            if (System.Text.RegularExpressions.Regex.IsMatch(dto.Username, @"[^a-zA-Z]"))
                return "Username can only contain letters ❌";

            // تحقق إن الاسم مش موجود مسبقًا
            var existingUser = await _repository.GetAll()
                .FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (existingUser != null)
                return "Username already exists ❌";

            // 2. تحقق من كلمة المرور
            if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 6)
                return "Password must be at least 6 characters ❌";

            if (!System.Text.RegularExpressions.Regex.IsMatch(dto.Password, @"[A-Z]"))
                return "Password must contain at least one uppercase letter ❌";

            if (!System.Text.RegularExpressions.Regex.IsMatch(dto.Password, @"[a-z]"))
                return "Password must contain at least one lowercase letter ❌";

            if (!System.Text.RegularExpressions.Regex.IsMatch(dto.Password, @"[0-9]"))
                return "Password must contain at least one number ❌";

            if (!System.Text.RegularExpressions.Regex.IsMatch(dto.Password, @"[\W_]"))
                return "Password must contain at least one special character ❌";

            // 3. تحقق من تطابق كلمة المرور مع التأكيد
            if (dto.Password != confirmPassword)
                return "Passwords do not match ❌";

            return null; // كل حاجة تمام
        }
    }
}
