using AppCore.Business.Models.Results;
using Business.Enums;
using Business.Models;
using System;

namespace Business.Services
{
    public interface IAccountService
    {
        Result Register(UserRegisterModel model);
        Result<UserModel> Login(UserLoginModel model);
    }

    public class AccountService : IAccountService
    {
        private readonly IUserService _userService;

        public AccountService(IUserService userService)
        {
            _userService = userService;
        }

        public Result<UserModel> Login(UserLoginModel model)
        {
            try
            {
                return _userService.GetUser(u => u.UserName == model.UserName && u.Password == model.Password && u.Active);
            }
            catch (Exception exc)
            {
                return new ExceptionResult<UserModel>(exc);
            }
        }

        public Result Register(UserRegisterModel model)
        {
            try
            {
                var user = new UserModel()
                {
                    Active = true,
                    UserName = model.UserName.Trim(),
                    Password = model.Password.Trim(),
                    RoleId = (int)Roles.User,
                    UserDetail = new UserDetailModel()
                    {
                        Address = model.UserDetail.Address.Trim(),
                        CityId = model.UserDetail.CityId,
                        CountryId = model.UserDetail.CountryId,
                        EMail = model.UserDetail.EMail.Trim()
                    }
                };
                return _userService.Add(user);
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }
    }
}
