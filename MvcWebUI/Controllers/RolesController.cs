using AppCore.Business.Models.Results;
using Business.Services.Bases;
using DataAccess.EntityFramework.Contexts;
using Entities.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Authorization;

namespace MvcWebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        // GET: Roles
        public IActionResult Index()
        {
            var query = _roleService.Query();
            var model = query.ToList();
            return View(model);
        }

        // GET: Roles/Create
        public IActionResult Create()
        {
            var model = new RoleModel();
            return View(model);
        }

        // POST: Roles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RoleModel role)
        {
            if (ModelState.IsValid)
            {
                var result = _roleService.Add(role);
                if (result.Status == ResultStatus.Success)
                {
                    return RedirectToAction(nameof(Index));
                }
                if (result.Status == ResultStatus.Error)
                {
                    ModelState.AddModelError("", result.Message);
                    return View(role);
                }
                throw new Exception(result.Message);
            }
            return View(role);
        }

        // GET: Roles/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var query = _roleService.Query();

            var role = query.SingleOrDefault(r => r.Id == id.Value);

            if (role == null)
            {
                return View("NotFound");
            }
            return View(role);
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(RoleModel role)
        {
            if (ModelState.IsValid)
            {
                var result = _roleService.Update(role);
                if (result.Status == ResultStatus.Success)
                {
                    return RedirectToAction(nameof(Index));
                }

                if (result.Status == ResultStatus.Error)
                {
                    ModelState.AddModelError("", result.Message);
                    return View(role);
                }

                throw new Exception(result.Message);
            }
            return View(role);
        }

        // POST: Roles/Delete
        [HttpPost]
        public JsonResult Delete(int id)
        {
            var deleteResult = _roleService.Delete(id);
            if (deleteResult.Status == ResultStatus.Exception)
                deleteResult.Message = "An exception occured while deleting the role!";
            return Json(deleteResult.Message);
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var query = _roleService.Query();

            var role = query.SingleOrDefault(r => r.Id == id.Value);

            if (role == null)
            {
                return View("NotFound");
            }
            return View(role);
        }
    }
}
