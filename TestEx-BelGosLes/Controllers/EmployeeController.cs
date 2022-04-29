using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Linq;
using System.Text;
using TestEx_BelGosLes.Interfaces;
using TestEx_BelGosLes.Models.DTO;
using Nancy.Json;

namespace TestEx_BelGosLes.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ICompany _companyService;
        private readonly IEmployee _employeeService;

        public EmployeeController(ICompany companyService, IEmployee employeeService)
        {
            _companyService = companyService;
            _employeeService = employeeService;
        }


        [HttpGet]
        public IActionResult Show(int id, string sortOrder)
        {
            ViewBag.Employees = _employeeService.GetList().GroupBy(x => x.Department).Select(y => y.First());
            ViewBag.Company = _companyService.GetById(id);
            var employees = _employeeService.GetList().Where(x => x.CompanyId == id);

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.CreateDateSortParm = sortOrder == "CreateDate" ? "CreateDate_desc" : "CreateDate";
            ViewBag.AddressSortParm = sortOrder == "Address" ? "Address_desc" : "Address";
            ViewBag.PhoneSortParm = sortOrder == "PhoneNumber" ? "PhoneNumber_desc" : "PhoneNumber";
            ViewBag.PositionSortParm = sortOrder == "Position" ? "Position_desc" : "Position";
            ViewBag.DepartmentSortParm = sortOrder == "Department" ? "Department_desc" : "Department";

            return View(_employeeService.Sort(employees, sortOrder));
        }

        [HttpPost]
        public ActionResult ExporDataToFile()
        {
            var dictioneryexportType = Request.Form.ToDictionary(x => x.Key, x => x.Value.ToString());
            var exportType = dictioneryexportType["Export"];
            var department = dictioneryexportType["department"];
            var employeeDTO = _employeeService.GetList();

            if (department != "All")
                employeeDTO = employeeDTO.Where(x => x.Department == department);

            switch (exportType)
            {
                case "Word":
                    Response.Clear();
                    Response.Headers.Add("Content-Type", "application/msword");
                    Response.Headers.Add("Content-disposition", "attachment; filename=EmployeesByDepartments.doc");
                    Response.Body.WriteAsync(_employeeService.ExportToWord(employeeDTO));
                    Response.Body.FlushAsync();
                    break;
                case "Json":
                    string jsonProductList = new JavaScriptSerializer().Serialize(employeeDTO);
                    byte[] byteArray = ASCIIEncoding.ASCII.GetBytes(jsonProductList);

                    Response.Clear();
                    Response.Headers.Clear();
                    Response.ContentType = "application/json";
                    Response.Headers.Add("Content-Length", jsonProductList.Length.ToString());
                    Response.Headers.Add("Content-Disposition", "attachment; filename=Employees.json;");
                    Response.Body.WriteAsync(byteArray);
                    Response.Body.FlushAsync();
                    break;
                case "Xml":
                    Response.Clear();
                    Response.Headers.Clear();
                    Response.ContentType = "application/xml";
                    Response.Headers.Add("Content-Disposition", "attachment; filename=Enployees.xml;");
                    Response.Body.WriteAsync(_employeeService.ExportToXML(employeeDTO));
                    Response.Body.Flush();
                    break;
            }
            return null;
        }


        [HttpGet]
        public IActionResult Create(int id)
        {
            ViewBag.Company = _companyService.GetById(id);
            return View();
        }


        [HttpPost]
        public ActionResult Create(EmployeeDTO employeeDTO)
        {
            ViewBag.Company = _companyService.GetById(employeeDTO.CompanyId);

            if (!_employeeService.IsUnique(employeeDTO))
            {
                if (ModelState.IsValid)
                {
                    _employeeService.Create(employeeDTO);
                    ViewBag.CreateResult = "Employee is successfully created!";
                }
                else
                {
                    ModelState.AddModelError("", "Not correct data!");
                }
            }
            else
            {
                ModelState.AddModelError("", "Employee already exists!");
            }
            return View();
        }

        public IActionResult Delete(int id)
        {
            int companyId = _employeeService.GetById(id).CompanyId;
            _employeeService.Delete(id);
            return RedirectToAction("Show", "Employee", new { id = companyId });
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (_employeeService.GetList().Any(x => x.Id == id) && id > 0)
            {
                ViewBag.Company = _companyService.GetById(_employeeService.GetById(id).CompanyId);
                return View(_employeeService.GetById(id));
            }
            else
            {
                ModelState.AddModelError("", "Eror 400 - Bad Request!");
                return View();
            }
        }

        [HttpPost]
        public IActionResult Edit(EmployeeDTO employeeDTO)
        {
            if (_employeeService.GetList().Any(x => x.Id == employeeDTO.Id) && employeeDTO.Id > 0)
            {
                if (!_employeeService.IsUnique(employeeDTO))
                {
                    ViewBag.Company = _companyService.GetById(employeeDTO.CompanyId);
                    _employeeService.Update(employeeDTO);
                    ViewBag.CreateResult = "Employee is successfully edited!";
                }
                else
                {
                    ModelState.AddModelError("", "Company already exists!");
                }
            }
            else
            {
                ModelState.AddModelError("", "Eror 400 - Bad Request!");
            }
            return View(_employeeService.GetById(employeeDTO.Id));
        }
    }
}
