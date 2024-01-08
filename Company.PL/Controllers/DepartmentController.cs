using AutoMapper;
using Company.BLL.Interfaces;
using Company.BLL.Repositories;
using Company.DAL.Models;
using Company.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Company.PL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public DepartmentController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            //ViewData["Message"] = "Hello To View Data";
            //  //ViewBag.Message = "Hello To View Bag";
            var department = await unitOfWork.DepartmentRepository.GetAllAsyn();
            var MapperdDept = mapper.Map<IEnumerable< Department>, IEnumerable< DepartmentViewModel >>(department);
       
            return View(MapperdDept);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task< IActionResult> Create(DepartmentViewModel DepartmentVM)
        {
            var MapperdDept = mapper.Map<DepartmentViewModel, Department>(DepartmentVM);
            if (ModelState.IsValid)
            {
               
            await  unitOfWork.DepartmentRepository.AddAsyn(MapperdDept);
              int result= await unitOfWork.Complete();
                if (result>0)
                {
                    TempData["message"] = "Department Is Create";
                }
                return RedirectToAction(nameof(Index));
            }
            return View(DepartmentVM);
        }
        public async Task< IActionResult> Details(int? id,string viewname="Details")
        {
            if (id is null)
            {
                return BadRequest();
            }
            var department =await unitOfWork.DepartmentRepository.GetAsyn(id.Value);
            var MapperdDept = mapper.Map<Department, DepartmentViewModel>(department);
            if (department == null)
            {
                return NotFound();
            }

            return View(viewname, MapperdDept);
        }
        [HttpGet]
        public Task<IActionResult> Edit(int id)
        {
            return Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute]int id,DepartmentViewModel DepartmentVM)
        {
            var MapperdDept = mapper.Map<DepartmentViewModel, Department>(DepartmentVM);
            if (id!=MapperdDept.Id)
            {
                return BadRequest();
            }
            if(ModelState.IsValid)
                try
                {
                    unitOfWork.DepartmentRepository.Update(MapperdDept);
                  await  unitOfWork.Complete();
                    return RedirectToAction(nameof(Index)); 
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            return View(DepartmentVM);
        }
        [HttpGet]
        public Task<IActionResult> Delete(int id)
        {
            return Details(id, "Delete");
        }
        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int id, DepartmentViewModel DepartmentVM)
        {
            var MapperdDept = mapper.Map<DepartmentViewModel, Department>(DepartmentVM);

            if (id != MapperdDept.Id)
                return BadRequest();

            try
            {
                var departmentHasEmployees = await unitOfWork.EmployeeRepository.AnyAsync(e => e.DepartmentId == MapperdDept.Id);

                if (departmentHasEmployees)
                {
                    ModelState.AddModelError(string.Empty, "Cannot delete. There are employees associated with this department.");
                    return View(DepartmentVM);
                }

                unitOfWork.DepartmentRepository.Delete(MapperdDept);
                await unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(DepartmentVM);
            }
        }
    }
    
}