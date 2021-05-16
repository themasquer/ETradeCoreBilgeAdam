using System;
using AppCore.Business.Models.Results;
using Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace MvcWebUI.Controllers
{
    [Route("[controller]")] // ~/CitiesJson
    public class CitiesJsonController : Controller
    {
        private readonly ICityService _cityService;

        public CitiesJsonController(ICityService cityService)
        {
            _cityService = cityService;
        }

        //[HttpGet]
        [Route("CitiesGet/{countryId?}")] // ~/CitiesJson/CitiesGet/1
        public IActionResult GetCitiesByCountryIdWithGet(int? countryId)
        {
            if (countryId == null)
                return View("NotFound");
            var result = _cityService.GetCities(countryId.Value);
            if (result.Status == ResultStatus.Exception)
                throw new Exception(result.Message);
            return Json(result.Data);
        }

        [HttpPost]
        [Route("CitiesPost/{countryId?}")] // ~/CitiesJson/CitiesPost
        public IActionResult GetCitiesByCountryIdWithPost(int? countryId)
        {
            if (countryId == null)
                return View("NotFound");
            var result = _cityService.GetCities(countryId.Value);
            if (result.Status == ResultStatus.Exception)
                throw new Exception(result.Message);
            return Json(result.Data);
        }
    }
}
