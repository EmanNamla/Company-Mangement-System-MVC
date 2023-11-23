using AutoMapper;
using Company.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Company.PL.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IMapper mapper;

        public RoleController(RoleManager<IdentityRole> roleManager,IMapper mapper)
        {
            this.roleManager = roleManager;
            this.mapper = mapper;
        }
        #region Action Index
        public async Task<IActionResult> Index(string SearchValue)
        {
            if (string.IsNullOrEmpty(SearchValue))
            {
                var roles = await roleManager.Roles.ToListAsync();
                var MappedUser = mapper.Map<IEnumerable<IdentityRole>, IEnumerable<RoleViewModel>>(roles);
                return View(MappedUser);
            }
            else
            {
                var role = await roleManager.FindByNameAsync(SearchValue);
                var MappedUser = mapper.Map<IdentityRole, RoleViewModel>(role);
                return View(MappedUser);
            }

        } 
        #endregion

        #region Actions Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var MapedRole = mapper.Map<RoleViewModel, IdentityRole>(model);
                await roleManager.CreateAsync(MapedRole);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        #endregion

        #region Action Details
        public async Task<IActionResult> Details([FromRoute] string id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var role = await roleManager.FindByIdAsync(id);
            var mappedrole = mapper.Map<IdentityRole, RoleViewModel>(role);
            return View(ViewName, mappedrole);
        } 
        #endregion
        #region Actions Edit
        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, "Edit");

        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var role = await roleManager.FindByIdAsync(id);
                    role.Name = model.RoleName;
                    await roleManager.UpdateAsync(role);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(model);
        }
        #endregion

        #region Actions Delete
        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirm(string id)
        {
            try
            {
                var role = await roleManager.FindByIdAsync(id);

                await roleManager.DeleteAsync(role);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Error", "Home");
            }
        } 
        #endregion
    }
}
