using AutoMapper;
using Company.DAL.Models;
using Company.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company.PL.Controllers
{
    [Authorize]
    public class UserController : Controller
	{
		private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;

        public UserController(UserManager<ApplicationUser> userManager,IMapper mapper)
		{
			this.userManager = userManager;
            this.mapper = mapper;
        }
		#region Action Index
		public async Task<IActionResult> Index(string SearchValue)
		{
			if (string.IsNullOrEmpty(SearchValue))
			{
				var user = await userManager.Users.Select(U => new UserViewModel()
				{
					Id = U.Id,
					Fname = U.FName,
					Lname = U.LName,
					Email = U.Email,
					PhoneNumber = U.PhoneNumber,
					Roles = userManager.GetRolesAsync(U).Result
				}).ToListAsync();
				return View(user);
			}
			else
			{
				var user = await userManager.FindByEmailAsync(SearchValue);
				var MappedUser = new UserViewModel()
				{
					Id = user.Id,
					Fname = user.FName,
					Lname = user.LName,
					Email = user.Email,
					PhoneNumber = user.PhoneNumber,
					Roles = userManager.GetRolesAsync(user).Result
				};

				return View(new List<UserViewModel> { MappedUser });
			}

		}
		#endregion

		#region Action Details
		public async Task<IActionResult> Details(string id, string ViewName = "Details")
		{
			if (id is null)
				return BadRequest();
			var user = await userManager.FindByIdAsync(id);
			if (user is null)
				return NotFound();
			var mappedUser = mapper.Map<ApplicationUser, UserViewModel>(user);
			return View(ViewName, mappedUser);
		}
		#endregion

		#region Actions Edit
		public async Task<IActionResult> Edit(string Id)
		{
			return await Details(Id, "Edit");
		}
		[HttpPost]
		public async Task<IActionResult> Edit([FromRoute]string id, UserViewModel model)
		{
			if (id != model.Id)
				return BadRequest();
			if (ModelState.IsValid)
			{
				try
				{
					var user = await userManager.FindByIdAsync(id);
					user.PhoneNumber = model.PhoneNumber;
					user.FName = model.Fname;
					user.LName = model.Lname;
					await userManager.UpdateAsync(user);
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
		public async Task<IActionResult> ConfirmDelete(string id)
		{
			try
			{
				var user = await userManager.FindByIdAsync(id);
				await userManager.DeleteAsync(user);
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
