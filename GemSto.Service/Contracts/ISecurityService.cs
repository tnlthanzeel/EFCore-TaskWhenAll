using GemSto.Service.Models;
using GemSto.Service.Models.Account;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GemSto.Service.Contracts
{
    public interface ISecurityService
    {
        Task<LoginModel> LoginAsync(LoginModel loginViewModel);
        Task<StaffModel> CreateNewUserAsync(StaffModel StaffModel);
        Task<IEnumerable<StaffModel>> GetAllStaff();
        Task DeleteStaffAsync(string id);
        Task ResetPasswordAsync(UserUpdateModel userUpdateModel);
    }
}
