using AutoMapper;
using Company.BLL.Interfaces;
using Company.DAL.Models;
using Company.PL.Helpers;
using Company.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Operations;
//using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Company.PL.Controllers
{
	[Authorize]
	public class EmployeeController : Controller
    {
        
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public EmployeeController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<IActionResult> Index(string SearchValue)
        {
            IEnumerable<Employee> employees;
            if ((string.IsNullOrEmpty(SearchValue)))
            {
                employees = await unitOfWork.EmployeeRepository.GetAllAsyn();
            }
            else
            {

                employees = unitOfWork.EmployeeRepository.GetEmployeeByName(SearchValue);
            }

            var MappedEmployee = mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            return View(MappedEmployee);
        }
        public IActionResult Create()
        {
            //ViewBag.Departments=departmentRepository.GetAll();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {

            if (ModelState.IsValid)
            {

                employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image, "Images");

                var MappedEmployee = mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                await unitOfWork.EmployeeRepository.AddAsyn(MappedEmployee);
                await unitOfWork.Complete();
                return RedirectToAction("Index");
            }

            return View(employeeVM);
        }

        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            ViewBag.Departments = await unitOfWork.DepartmentRepository.GetAllAsyn();
            if (id == null)
            {
                return BadRequest();
            }

            var employee = await unitOfWork.EmployeeRepository.GetAsyn(id.Value);
            var MappedEmployee = mapper.Map<Employee, EmployeeViewModel>(employee);
            if (employee == null)
            {
                return NotFound();
            }
            return View(ViewName, MappedEmployee);
        }
        public Task<IActionResult> Edit(int id)
        {
            return Details(id,"Edit");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, EmployeeViewModel employeeVM)
        {

            if (ModelState.IsValid)
            {
                if (employeeVM.Image is not null)
                {
                    employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image, "Images");

                }
                var MappedEmployee = mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                unitOfWork.EmployeeRepository.Update(MappedEmployee);
                await unitOfWork.Complete();
                return RedirectToAction("Index");
            }
            return View(employeeVM);
        }

        public Task< IActionResult> Delete(int id)
        {
            return Details(id, "Delete");
        }
        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int? id, EmployeeViewModel employeeVM)
        {
            ViewBag.Departments = await unitOfWork.DepartmentRepository.GetAllAsyn();
           
            if (id!= employeeVM.Id)
            {
                return BadRequest();
            }
            try
            {
                var MappedEmployee = mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                unitOfWork.EmployeeRepository.Delete(MappedEmployee);
                int count= await unitOfWork.Complete();
                if(count>0)
                {
                    DocumentSettings.DeleteFile(employeeVM.ImageName,"Images");
                }
                return RedirectToAction(nameof(Index));
            }
            catch(System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(employeeVM);
            }
            
            
           
        }
    }
}
