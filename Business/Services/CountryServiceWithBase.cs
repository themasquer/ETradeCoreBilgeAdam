using System;
using System.Collections.Generic;
using AppCore.Business.Models.Results;
using AppCore.Business.Services.Bases;
using Business.Models;
using DataAccess.EntityFramework.Repositories;
using System.Linq;
using Entities.Entities;

namespace Business.Services
{
    public interface ICountryService : IService<CountryModel>
    {
        Result<List<CountryModel>> GetCountries();
        Result<CountryModel> GetCountry(int id);
    }

    public class CountryService : ICountryService
    {
        private readonly CountryRepositoryBase _countryRepository;

        public CountryService(CountryRepositoryBase countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public Result Add(CountryModel model)
        {
            try
            {
                if (_countryRepository.Query().Any(c => c.Name.ToUpper() == model.Name.ToUpper().Trim()))
                    return new ErrorResult("Country with the same name exists!");
                var entity = new Country()
                {
                    Name = model.Name.Trim()
                };
                _countryRepository.Add(entity);
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
                var country = _countryRepository.EntityQuery("Cities", "UserDetails").SingleOrDefault(c => c.Id == id);
                if (country == null)
                    return new ErrorResult("Country not found!");
                if (country.Cities != null && country.Cities.Count > 0)
                    return new ErrorResult("Country has cities so it can't be deleted!");
                if (country.UserDetails != null && country.UserDetails.Count > 0)
                    return new ErrorResult("Country has users so it can't be deleted!");
                _countryRepository.Delete(country);
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }

        public void Dispose()
        {
            _countryRepository?.Dispose();
        }

        public IQueryable<CountryModel> Query()
        {
            return _countryRepository.EntityQuery().OrderBy(c => c.Name).Select(c => new CountryModel()
            {
                Id = c.Id,
                Guid = c.Guid,
                Name = c.Name
            });
        }

        public Result Update(CountryModel model)
        {
            try
            {
                if (_countryRepository.Query().Any(c => c.Name.ToUpper() == model.Name.ToUpper().Trim() && c.Id != model.Id))
                    return new ErrorResult("Country with the same name exists!");
                var entity = new Country()
                {
                    Id = model.Id,
                    Name = model.Name.Trim()
                };
                _countryRepository.Update(entity);
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }

        public Result<List<CountryModel>> GetCountries()
        {
            try
            {
                var countries = Query().ToList();
                return new SuccessResult<List<CountryModel>>(countries);
            }
            catch (Exception exc)
            {
                return new ExceptionResult<List<CountryModel>>(exc);
            }
        }

        public Result<CountryModel> GetCountry(int id)
        {
            try
            {
                var country = Query().SingleOrDefault(c => c.Id == id);
                if (country == null)
                    return new ErrorResult<CountryModel>("Country not found!");
                return new SuccessResult<CountryModel>(country);
            }
            catch (Exception exc)
            {
                return new ExceptionResult<CountryModel>(exc);
            }
        }
    }
}
