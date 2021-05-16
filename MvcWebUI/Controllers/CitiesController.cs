using AppCore.Business.Models.Results;
using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace MvcWebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CitiesController : Controller
    {
        private readonly ICityService _cityService;
        private readonly ICountryService _countryService;

        public CitiesController(ICityService cityService, ICountryService countryService)
        {
            _cityService = cityService;
            _countryService = countryService;
        }

        // GET: Cities
        public IActionResult Index()
        {
            var result = _cityService.GetCities();
            if (result.Status == ResultStatus.Exception)
                throw new Exception(result.Message);
            return View(result.Data);
        }

        // GET: Cities/Create
        public IActionResult Create()
        {
            var countriesResult = _countryService.GetCountries();
            if (countriesResult.Status == ResultStatus.Exception)
                throw new Exception(countriesResult.Message);
            ViewBag.Countries = new SelectList(countriesResult.Data, "Id", "Name");
            var model = new CityModel();
            return View(model);
        }

        // POST: Cities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CityModel city)
        {
            Result<List<CountryModel>> countriesResult;
            if (ModelState.IsValid)
            {
                var result = _cityService.Add(city);
                if (result.Status == ResultStatus.Success)
                {
                    return RedirectToAction(nameof(Index));
                }
                if (result.Status == ResultStatus.Error)
                {
                    ModelState.AddModelError("", result.Message);
                    countriesResult = _countryService.GetCountries();
                    if (countriesResult.Status == ResultStatus.Exception)
                        throw new Exception(countriesResult.Message);
                    ViewBag.Countries = new SelectList(countriesResult.Data, "Id", "Name", city.CountryId);
                    return View(city);
                }
                throw new Exception(result.Message);
            }
            countriesResult = _countryService.GetCountries();
            if (countriesResult.Status == ResultStatus.Exception)
                throw new Exception(countriesResult.Message);
            ViewBag.Countries = new SelectList(countriesResult.Data, "Id", "Name", city.CountryId);
            return View(city);
        }

        // GET: Cities/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var result = _cityService.GetCity(id.Value);

            if (result.Status == ResultStatus.Exception)
                throw new Exception(result.Message);
            if (result.Status == ResultStatus.Error)
                return View("NotFound");

            var countriesResult = _countryService.GetCountries();
            if (countriesResult.Status == ResultStatus.Exception)
                throw new Exception(countriesResult.Message);
            ViewBag.Countries = new SelectList(countriesResult.Data, "Id", "Name", result.Data.CountryId);
            return View(result.Data);
        }

        // POST: Cities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CityModel city)
        {
            Result<List<CountryModel>> countriesResult;
            if (ModelState.IsValid)
            {
                var result = _cityService.Update(city);
                if (result.Status == ResultStatus.Success)
                {
                    return RedirectToAction(nameof(Index));
                }

                if (result.Status == ResultStatus.Error)
                {
                    ModelState.AddModelError("", result.Message);
                    countriesResult = _countryService.GetCountries();
                    if (countriesResult.Status == ResultStatus.Exception)
                        throw new Exception(countriesResult.Message);
                    ViewBag.Countries = new SelectList(countriesResult.Data, "Id", "Name", city.CountryId);
                    return View(city);
                }

                throw new Exception(result.Message);
            }

            countriesResult = _countryService.GetCountries();
            if (countriesResult.Status == ResultStatus.Exception)
                throw new Exception(countriesResult.Message);
            ViewBag.Countries = new SelectList(countriesResult.Data, "Id", "Name", city.CountryId);
            return View(city);
        }

        // POST: Cities/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var deleteResult = _cityService.Delete(id);
            if (deleteResult.Status == ResultStatus.Success)
            {
                return RedirectToAction(nameof(Index));
            }

            if (deleteResult.Status == ResultStatus.Error)
            {
                ModelState.AddModelError("", deleteResult.Message);

                var getResult = _cityService.GetCity(id);
                if (getResult.Status == ResultStatus.Exception)
                    throw new Exception(getResult.Message);
                if (getResult.Status == ResultStatus.Error)
                    return View("NotFound");

                var countriesResult = _countryService.GetCountries();
                if (countriesResult.Status == ResultStatus.Exception)
                    throw new Exception(countriesResult.Message);
                ViewBag.Countries = new SelectList(countriesResult.Data, "Id", "Name", getResult.Data.CountryId);
                return View("Edit", getResult.Data);
            }
            throw new Exception(deleteResult.Message);
        }
    }
}
