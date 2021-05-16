using AppCore.Business.Models.Results;
using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Authorization;

namespace MvcWebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CountriesController : Controller
    {
        private readonly ICountryService _countryService;

        public CountriesController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        // GET: Countries
        public IActionResult Index()
        {
            var result = _countryService.GetCountries();
            if (result.Status == ResultStatus.Exception)
                throw new Exception(result.Message);
            return View(result.Data);
        }

        // GET: Countries/Create
        public IActionResult Create()
        {
            var model = new CountryModel();
            return View(model);
        }

        // POST: Countries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CountryModel country)
        {
            if (ModelState.IsValid)
            {
                var result = _countryService.Add(country);
                if (result.Status == ResultStatus.Success)
                {
                    return RedirectToAction(nameof(Index));
                }
                if (result.Status == ResultStatus.Error)
                {
                    ModelState.AddModelError("", result.Message);
                    return View(country);
                }
                throw new Exception(result.Message);
            }
            return View(country);
        }

        // GET: Countries/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var result = _countryService.GetCountry(id.Value);

            if (result.Status == ResultStatus.Exception)
                throw new Exception(result.Message);
            if (result.Status == ResultStatus.Error)
                return View("NotFound");
            
            return View(result.Data);
        }

        // POST: Countries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CountryModel country)
        {
            if (ModelState.IsValid)
            {
                var result = _countryService.Update(country);
                if (result.Status == ResultStatus.Success)
                {
                    return RedirectToAction(nameof(Index));
                }

                if (result.Status == ResultStatus.Error)
                {
                    ModelState.AddModelError("", result.Message);
                    return View(country);
                }

                throw new Exception(result.Message);
            }
            return View(country);
        }

        // POST: Countries/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var deleteResult = _countryService.Delete(id);
            if (deleteResult.Status == ResultStatus.Success)
            {
                return RedirectToAction(nameof(Index));
            }

            if (deleteResult.Status == ResultStatus.Error)
            {
                ModelState.AddModelError("", deleteResult.Message);

                var getResult = _countryService.GetCountry(id);
                if (getResult.Status == ResultStatus.Exception)
                    throw new Exception(getResult.Message);
                if (getResult.Status == ResultStatus.Error)
                    return View("NotFound");
                return View("Edit", getResult.Data);
            }
            throw new Exception(deleteResult.Message);
        }
    }
}
