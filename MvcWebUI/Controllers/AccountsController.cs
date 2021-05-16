using AppCore.Business.Models.Results;
using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace MvcWebUI.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ICountryService _countryService;
        private readonly ICityService _cityService;

        public AccountsController(IAccountService accountService, ICountryService countryService, ICityService cityService)
        {
            _accountService = accountService;
            _countryService = countryService;
            _cityService = cityService;
        }

        public IActionResult Register()
        {
            var countriesResult = _countryService.GetCountries();
            if (countriesResult.Status == ResultStatus.Exception)
                throw new Exception(countriesResult.Message);
            ViewBag.Countries = new SelectList(countriesResult.Data, "Id", "Name");
            var model = new UserRegisterModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Register(UserRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _accountService.Register(model);
                if (result.Status == ResultStatus.Exception)
                    throw new Exception(result.Message);
                if (result.Status == ResultStatus.Success)
                    return RedirectToAction("Login");

                // result.Status == ResultStatus.Error:
                ModelState.AddModelError("", result.Message);
            }
            var countriesResult = _countryService.GetCountries();
            if (countriesResult.Status == ResultStatus.Exception)
                throw new Exception(countriesResult.Message);
            ViewBag.Countries = new SelectList(countriesResult.Data, "Id", "Name", model.UserDetail.CountryId);
            var citiesResult = _cityService.GetCities(model.UserDetail.CountryId);
            if (citiesResult.Status == ResultStatus.Exception)
                throw new Exception(citiesResult.Message);
            ViewBag.Cities = new SelectList(citiesResult.Data, "Id", "Name", model.UserDetail.CityId);
            return View(model);
        }

        public IActionResult Login()
        {
            var model = new UserLoginModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(UserLoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _accountService.Login(model);
                if (result.Status == ResultStatus.Exception)
                {
                    throw new Exception(result.Message);
                }

                if (result.Status == ResultStatus.Error)
                {
                    ViewBag.Message = result.Message;
                    return View(model);
                }

                var user = result.Data;

                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.UserName), 
                    new Claim(ClaimTypes.Role, user.Role.Name),
                    new Claim(ClaimTypes.Sid, user.Id.ToString())
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        public IActionResult Logout()
        {
            //HttpContext.Session.Remove("cart"); // sadece cart key'ine ait session'ı temizler
            HttpContext.Session.Clear(); // tüm key'lere ait session'ları temizler
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
