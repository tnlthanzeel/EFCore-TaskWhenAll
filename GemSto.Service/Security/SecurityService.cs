using AutoMapper;
using GemSto.Common.Enum;
using GemSto.Data;
using GemSto.Domain.User;
using GemSto.Service.Contracts;
using GemSto.Service.Models;
using GemSto.Service.Models.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GemSto.Service.Security
{
    public class SecurityService : ISecurityService
    {
        private readonly SignInManager<StoreUser> signInManager;
        private readonly UserManager<StoreUser> userManager;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;
        private readonly GemStoContext gemStoContext;
        private readonly IHttpContextAccessor httpContextAccessor;

        public SecurityService
            (
            SignInManager<StoreUser> signInManager,
            UserManager<StoreUser> userManager,
            IConfiguration configuration,
            IMapper mapper,
            GemStoContext gemStoContext,
            IHttpContextAccessor httpContextAccessor
            )
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.configuration = configuration;
            this.mapper = mapper;
            this.gemStoContext = gemStoContext;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<StaffModel> CreateNewUserAsync(StaffModel StaffModel)
        {
            try
            {
                var storeUser = mapper.Map<StoreUser>(StaffModel);
                var user = await userManager.CreateAsync(storeUser, StaffModel.Password);
                if (user.Succeeded)
                {
                    var result = mapper.Map<StaffModel>(storeUser);
                    return result;
                }
                else
                {
                    return new StaffModel();
                }
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public async Task<LoginModel> LoginAsync(LoginModel loginModel)
        {
            var user = await userManager.FindByNameAsync(loginModel.UserName);
            if (user != null && !user.IsDeleted)
            {
                var result = await signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);
                var ip = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

                if (result.Succeeded)
                {

                    var auditRecord = UserAudit.CreateAuditEvent(user.Id, UserAuditEventType.Login, ip);

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Tokens:Key"]));
                    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub,user.Id),
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.UniqueName,user.UserName),
                        new Claim("isAdmin",user.IsAdmin.ToString()),
                    };

                    var token = new JwtSecurityToken(
                        configuration["Tokens:Issuer"],
                        configuration["Tokens:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddDays(1),
                        signingCredentials: credentials
                        );

                    var generatedToken = new LoginModel
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        TokenExpiration = token.ValidTo,
                        IsAdmin = user.IsAdmin
                    };

                    await gemStoContext.UserAudits.AddAsync(auditRecord);
                    await gemStoContext.SaveChangesAsync();
                    return generatedToken;
                }
                else
                {
                    var auditRecord = UserAudit.CreateAuditEvent(loginModel.UserName, UserAuditEventType.FailedLogin, ip);
                    await gemStoContext.UserAudits.AddAsync(auditRecord);
                    await gemStoContext.SaveChangesAsync();
                }
            }

            return new LoginModel();
        }

        public async Task<IEnumerable<StaffModel>> GetAllStaff()
        {
            var allStaff = userManager.Users.Where(w => w.IsDeleted == false).Select(s =>
                new StaffModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    PhoneNumber = s.PhoneNumber,
                    IsAdmin = s.IsAdmin,
                    Address = s.Address,
                    UserName = s.UserName
                }).AsNoTracking().AsQueryable();

            return await allStaff.ToListAsync();
        }

        public async Task DeleteStaffAsync(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            user.IsDeleted = true;
            await userManager.UpdateAsync(user);
        }


        public async Task ResetPasswordAsync(UserUpdateModel userUpdateModel)
        {
            try
            {
                var userStore = new UserStore<StoreUser>(gemStoContext);

                var user = await userManager.FindByIdAsync(userUpdateModel.Id);

                var hashedNewPassword = userManager.PasswordHasher.HashPassword(user, userUpdateModel.Password);

                await userStore.SetPasswordHashAsync(user, hashedNewPassword);
                await userStore.UpdateAsync(user);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
