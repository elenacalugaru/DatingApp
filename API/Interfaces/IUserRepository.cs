using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        
        Task<IEnumerable<MemberDto>> GetMembersAsync();
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<MemberDto> GetMemberByIdAsync(int id);
        Task<AppUser> GetUserByUsernameAsync(string username);
        Task<MemberDto> GetMemberByUsernameAsync(string username);
    }
}