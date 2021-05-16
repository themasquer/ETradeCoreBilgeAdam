using AppCore.Business.Models.Results;
using AppCore.Business.Services.Bases;
using Business.Models;
using DataAccess.EntityFramework.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using Entities.Entities;

namespace Business.Services
{
    public interface ICityService : IService<CityModel>
    {
        Result<List<CityModel>> GetCities(int? countryId = null);
        Result<CityModel> GetCity(int id);
    }

    public class CityService : ICityService
    {
        private readonly CityRepositoryBase _cityRepository;

        public CityService(CityRepositoryBase cityRepository)
        {
            _cityRepository = cityRepository;
        }

        public Result Add(CityModel model)
        {
            try
            {
                if (_cityRepository.Query().Any(c => c.Name.ToUpper() == model.Name.ToUpper().Trim()))
                    return new ErrorResult("City with the same name exists!");
                var entity = new City()
                {
                    CountryId = model.CountryId,
                    Name = model.Name.Trim()
                };
                _cityRepository.Add(entity);
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
                var city = _cityRepository.EntityQuery("UserDetails").SingleOrDefault(c => c.Id == id);
                if (city == null)
                    return new ErrorResult("City not found!");
                if (city.UserDetails != null && city.UserDetails.Count > 0)
                    return new ErrorResult("City has users so it can't be deleted!");
                _cityRepository.Delete(city);
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }

        public void Dispose()
        {
            _cityRepository?.Dispose();
        }

        public IQueryable<CityModel> Query()
        {
            return _cityRepository.EntityQuery("Country").OrderBy(c => c.Name).Select(c => new CityModel()
            {
                Id = c.Id,
                Guid = c.Guid,
                Name = c.Name,
                CountryId = c.CountryId,
                Country = new CountryModel()
                {
                    Id = c.Country.Id,
                    Guid = c.Country.Guid,
                    Name = c.Country.Name
                }
            });
        }

        public Result<List<CityModel>> GetCities(int? countryId = null)
        {
            try
            {
                var query = Query();
                if (countryId.HasValue)
                    query = query.Where(c => c.CountryId == countryId.Value);
                return new SuccessResult<List<CityModel>>(query.ToList());
            }
            catch (Exception exc)
            {
                return new ExceptionResult<List<CityModel>>(exc);
            }
        }

        public Result Update(CityModel model)
        {
            try
            {
                if (_cityRepository.Query().Any(c => c.Name.ToUpper() == model.Name.ToUpper().Trim() && c.Id != model.Id))
                    return new ErrorResult("City with the same name exists!");
                var entity = new City()
                {
                    Id = model.Id,
                    Name = model.Name.Trim(),
                    CountryId = model.CountryId
                };
                _cityRepository.Update(entity);
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }

        public Result<CityModel> GetCity(int id)
        {
            try
            {
                var city = Query().SingleOrDefault(c => c.Id == id);
                if (city == null)
                    return new ErrorResult<CityModel>("City not found!");
                return new SuccessResult<CityModel>(city);
            }
            catch (Exception exc)
            {
                return new ExceptionResult<CityModel>(exc);
            }
        }
    }
}
