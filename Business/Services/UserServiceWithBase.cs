using AppCore.Business.Models.Results;
using AppCore.Business.Services.Bases;
using Business.Models;
using DataAccess.EntityFramework.Repositories;
using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Business.Services
{
    public interface IUserService : IService<UserModel>
    {
        Result<List<UserModel>> GetUsers();
        Result<UserModel> GetUser(int id);
        Result<UserModel> GetUser(Expression<Func<UserModel, bool>> predicate);
    }

    public class UserService : IUserService
    {
        private readonly UserRepositoryBase _userRepository;
        private readonly CountryRepositoryBase _countryRepository;
        private readonly CityRepositoryBase _cityRepository;

        public UserService(UserRepositoryBase userRepository, CountryRepositoryBase countryRepository, CityRepositoryBase cityRepository)
        {
            _userRepository = userRepository;
            _countryRepository = countryRepository;
            _cityRepository = cityRepository;
        }

        public Result Add(UserModel model)
        {
            try
            {
                if (_userRepository.EntityQuery().Any(u => u.UserName.ToUpper() == model.UserName.ToUpper().Trim()))
                    return new ErrorResult("User with the same user name exists!");
                if (_userRepository.EntityQuery("UserDetail").Any(u => u.UserDetail.EMail.ToUpper() == model.UserDetail.EMail.ToUpper().Trim()))
                    return new ErrorResult("User with the same e-mail exists!");
                var entity = new User()
                {
                    Active = model.Active,
                    UserName = model.UserName.Trim(),
                    Password = model.Password.Trim(),
                    RoleId = model.RoleId,
                    UserDetail = new UserDetail()
                    {
                        Address = model.UserDetail.Address.Trim(),
                        CityId = model.UserDetail.CityId,
                        CountryId = model.UserDetail.CountryId,
                        EMail = model.UserDetail.EMail.Trim()
                    }
                };
                _userRepository.Add(entity);
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }

        public Result Delete(int id)
        {
            try
            {
                _userRepository.DeleteEntity(id);
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }

        public void Dispose()
        {
            _userRepository?.Dispose();
        }

        public IQueryable<UserModel> Query()
        {
            var userQuery = _userRepository.EntityQuery("UserDetail", "Role");
            var countryQuery = _countryRepository.Query();
            var cityQuery = _cityRepository.Query();

            var query = from user in userQuery
                        join country in countryQuery
                            on user.UserDetail.CountryId equals country.Id
                        join city in cityQuery
                            on user.UserDetail.CityId equals city.Id
                        //orderby new { user.Role.Name, user.UserName }
                        select new UserModel()
                        {
                            Active = user.Active,
                            ActiveText = user.Active ? "Yes" : "No",
                            Guid = user.Guid,
                            Id = user.Id,
                            Password = user.Password,
                            Role = new RoleModel()
                            {
                                Guid = user.Role.Guid,
                                Id = user.Role.Id,
                                Name = user.Role.Name
                            },
                            RoleId = user.RoleId,
                            UserDetail = new UserDetailModel()
                            {
                                Address = user.UserDetail.Address,
                                CityId = user.UserDetail.CityId,
                                CountryId = user.UserDetail.CountryId,
                                EMail = user.UserDetail.EMail,
                                Guid = user.UserDetail.Guid,
                                Id = user.UserDetail.Id,
                                Country = new CountryModel()
                                {
                                    Id = country.Id,
                                    Guid = country.Guid,
                                    Name = country.Name
                                },
                                City = new CityModel()
                                {
                                    Id = city.Id,
                                    Guid = city.Guid,
                                    Name = city.Name,
                                    CountryId = city.CountryId
                                }
                            },
                            UserDetailId = user.UserDetailId,
                            UserName = user.UserName
                        };
            return query;
        }

        public Result<List<UserModel>> GetUsers()
        {
            try
            {
                var users = Query().ToList();
                if (users == null || users.Count == 0)
                    return new ErrorResult<List<UserModel>>("No users found!");
                return new SuccessResult<List<UserModel>>(users);
            }
            catch (Exception exc)
            {
                return new ExceptionResult<List<UserModel>>(exc);
            }
        }

        public Result<UserModel> GetUser(int id)
        {
            try
            {
                var user = Query().SingleOrDefault(u => u.Id == id);
                if (user == null)
                    return new ErrorResult<UserModel>("No user found!");
                return new SuccessResult<UserModel>(user);
            }
            catch (Exception exc)
            {
                return new ExceptionResult<UserModel>(exc);
            }
        }

        public Result<UserModel> GetUser(Expression<Func<UserModel, bool>> predicate)
        {
            try
            {
                var user = Query().SingleOrDefault(predicate);
                if (user == null)
                    return new ErrorResult<UserModel>("No user found!");
                return new SuccessResult<UserModel>(user);
            }
            catch (Exception exc)
            {
                return new ExceptionResult<UserModel>(exc);
            }
        }

        public Result Update(UserModel model)
        {
            try
            {
                if (_userRepository.EntityQuery().Any(u => u.UserName.ToUpper() == model.UserName.ToUpper().Trim() && u.Id != model.Id))
                    return new ErrorResult("User with the same user name exists!");
                if (_userRepository.EntityQuery("UserDetail").Any(u => u.UserDetail.EMail.ToUpper() == model.UserDetail.EMail.ToUpper().Trim() && u.Id != model.Id))
                    return new ErrorResult("User with the same e-mail exists!");
                var entity = new User()
                {
                    Id = model.Id,
                    Active = model.Active,
                    UserName = model.UserName.Trim(),
                    Password = model.Password.Trim(),
                    RoleId = model.RoleId,
                    UserDetail = new UserDetail()
                    {
                        Id = model.UserDetail.Id,
                        Address = model.UserDetail.Address.Trim(),
                        CityId = model.UserDetail.CityId,
                        CountryId = model.UserDetail.CountryId,
                        EMail = model.UserDetail.EMail.Trim()
                    }
                };
                _userRepository.Update(entity);
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }
    }
}
