using AppCore.Business.Models.Results;
using Business.Models;
using Business.Services;
using DataAccess.EntityFramework.Contexts;
using Entities.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Business.Enums;

namespace MvcWebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly ICountryService _countryService;
        private readonly ICityService _cityService;

        public UsersController(IUserService userService, IRoleService roleService, ICountryService countryService, ICityService cityService)
        {
            _userService = userService;
            _roleService = roleService;
            _countryService = countryService;
            _cityService = cityService;
        }

        // GET: Users
        public IActionResult Index()
        {
            var result = _userService.GetUsers();
            if (result.Status == ResultStatus.Exception)
                throw new Exception(result.Message);
            if (result.Status == ResultStatus.Error)
                ViewBag.Message = result.Message;
            return View(result.Data);
        }

        // GET: Users/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var result = _userService.GetUser(id.Value);
            if (result.Status == ResultStatus.Exception)
                throw new Exception(result.Message);
            if (result.Status == ResultStatus.Error)
                ViewData["Message"] = result.Message;
            return View(result.Data);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["Roles"] = new SelectList(_roleService.Query().ToList(), "Id", "Name", (int)Roles.Admin);
            var countryResult = _countryService.GetCountries();
            if (countryResult.Status == ResultStatus.Exception)
                throw new Exception(countryResult.Message);
            ViewData["Countries"] = new SelectList(countryResult.Data, "Id", "Name");
            var model = new UserModel();
            return View(model);
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UserModel user)
        {
            if (ModelState.IsValid)
            {
                var userResult = _userService.Add(user);
                if (userResult.Status == ResultStatus.Exception)
                    throw new Exception(userResult.Message);
                if (userResult.Status == ResultStatus.Success)
                    return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", userResult.Message);
            }
            ViewData["Roles"] = new SelectList(_roleService.Query().ToList(), "Id", "Name", user.RoleId);
            var countryResult = _countryService.GetCountries();
            if (countryResult.Status == ResultStatus.Exception)
                throw new Exception(countryResult.Message);
            ViewData["Countries"] = new SelectList(countryResult.Data, "Id", "Name");
            var cityResult = _cityService.GetCities(user.UserDetail.CountryId);
            if (cityResult.Status == ResultStatus.Exception)
                throw new Exception(cityResult.Message);
            ViewData["Cities"] = new SelectList(cityResult.Data, "Id", "Name", user.UserDetail.CityId);
            return View(user);
        }

        // GET: Users/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var userResult = _userService.GetUser(id.Value);
            if (userResult.Status == ResultStatus.Exception)
                throw new Exception(userResult.Message);
            if (userResult.Status == ResultStatus.Error)
                return View("NotFound");
            ViewBag.Roles = new SelectList(_roleService.Query().ToList(), "Id", "Name", userResult.Data.RoleId);
            var countryResult = _countryService.GetCountries();
            if (countryResult.Status == ResultStatus.Exception)
                throw new Exception(countryResult.Message);
            ViewBag.Countries = new SelectList(countryResult.Data, "Id", "Name", userResult.Data.UserDetail.CountryId);
            var cityResult = _cityService.GetCities(userResult.Data.UserDetail.CountryId);
            if (cityResult.Status == ResultStatus.Exception)
                throw new Exception(cityResult.Message);
            ViewData["Cities"] = new SelectList(cityResult.Data, "Id", "Name", userResult.Data.UserDetail.CityId);
            return View(userResult.Data);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UserModel user)
        {
            if (ModelState.IsValid)
            {
                var userResult = _userService.Update(user);
                if(userResult.Status == ResultStatus.Exception)
                    throw new Exception(userResult.Message);
                if (userResult.Status == ResultStatus.Success)
                    return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", userResult.Message);
            }
            ViewBag.Roles = new SelectList(_roleService.Query().ToList(), "Id", "Name", user.RoleId);
            var countryResult = _countryService.GetCountries();
            if (countryResult.Status == ResultStatus.Exception)
                throw new Exception(countryResult.Message);
            ViewBag.Countries = new SelectList(countryResult.Data, "Id", "Name", user.UserDetail.CountryId);
            var cityResult = _cityService.GetCities(user.UserDetail.CountryId);
            if (cityResult.Status == ResultStatus.Exception)
                throw new Exception(cityResult.Message);
            ViewData["Cities"] = new SelectList(cityResult.Data, "Id", "Name", user.UserDetail.CityId);
            return View(user);
        }

        // GET: Users/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var result = _userService.Delete(id.Value);
            if (result.Status == ResultStatus.Exception)
                throw new Exception(result.Message);
            return RedirectToAction(nameof(Index));
        }
    }
}
