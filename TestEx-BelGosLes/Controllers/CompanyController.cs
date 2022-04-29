using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TestEx_BelGosLes.Interfaces;
using TestEx_BelGosLes.Models.DTO;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Xml;
using Nancy.Json;

namespace TestEx_BelGosLes.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ICompany _companyService;

        public CompanyController(ICompany companyService)
        {
            _companyService = companyService;
        }


        [HttpGet]
        public IActionResult Show(string sortOrder)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.AddressSortParm = sortOrder == "Address" ? "Address_desc" : "Address";
            ViewBag.HeadFullNameSortParm = sortOrder == "HeadFullName" ? "HeadFullName_desc" : "HeadFullName";
            ViewBag.CreateDateSortParm = sortOrder == "CreateDate" ? "CreateDate_desc" : "CreateDate";
            ViewBag.UpdateDateSortParm = sortOrder == "UpdateDate" ? "UpdateDate_desc" : "UpdateDate";
            ViewBag.PhoneNumberSortParm = sortOrder == "PhoneNumber" ? "PhoneNumber_desc" : "PhoneNumber";
            ViewBag.EmailSortParm = sortOrder == "Email" ? "Email_desc" : "Email";

            return View(_companyService.Sort(_companyService.GetList(), sortOrder));
        }


        [HttpPost]
        public IActionResult ExporDataToFile()
        {
            var dictioneryexportType = Request.Form.ToDictionary(x => x.Key, x => x.Value.ToString());
            var exportType = dictioneryexportType["Export"];
            var companyDTO = _companyService.GetList();
            switch (exportType)
            {
                case "Word":
                    Response.Clear();
                    Response.Headers.Add("Content-Type", "application/msword");
                    Response.Headers.Add("Content-disposition", "attachment; filename=CompaniesList.doc");
                    Response.Body.WriteAsync(_companyService.ExportToWord(companyDTO));
                    Response.Body.FlushAsync();
                    break;
                case "Json":
                    string jsonProductList = new JavaScriptSerializer().Serialize(companyDTO);
                    byte[] byteArray = ASCIIEncoding.ASCII.GetBytes(jsonProductList);

                    Response.Clear();
                    Response.Headers.Clear();
                    Response.ContentType = "application/json";
                    Response.Headers.Add("Content-Length", jsonProductList.Length.ToString());
                    Response.Headers.Add("Content-Disposition", "attachment; filename=Companies.json;");
                    Response.Body.WriteAsync(byteArray);
                    Response.Body.FlushAsync();
                    break;
                case "Xml":
                    Response.Clear();
                    Response.Headers.Clear();
                    Response.ContentType = "application/xml";
                    Response.Headers.Add("Content-Disposition", "attachment; filename=Companies.xml;");
                    Response.Body.WriteAsync(_companyService.ExportToXML(companyDTO));
                    Response.Body.Flush();
                    break;
            }
            return null;
        }

        private void ExportToWord(IEnumerable<CompanyDTO> companyDTO)
        {
            DataTable dtProduct = GetDataTable(companyDTO);

            if (dtProduct.Rows.Count > 0)
            {
                StringBuilder sbDocumentBody = new StringBuilder();

                sbDocumentBody.Append("<table width=\"100%\" style=\"background-color:#ffffff;\">");
                if (dtProduct.Rows.Count > 0)
                {
                    sbDocumentBody.Append("<tr><td>");
                    sbDocumentBody.Append("<table width=\"600\" cellpadding=0 cellspacing=0 style=\"border: 1px solid gray;\">");

                    // Add Column Headers dynamically from datatable  
                    sbDocumentBody.Append("<tr>");
                    for (int i = 0; i < dtProduct.Columns.Count; i++)
                    {
                        sbDocumentBody.Append("<td class=\"Header\" width=\"120\" style=\"border: 1px solid gray; text-align:center; font-family:Verdana; font-size:12px; font-weight:bold;\">" + dtProduct.Columns[i].ToString().Replace(".", "<br>") + "</td>");
                    }
                    sbDocumentBody.Append("</tr>");

                    // Add Data Rows dynamically from datatable  
                    for (int i = 0; i < dtProduct.Rows.Count; i++)
                    {
                        sbDocumentBody.Append("<tr>");
                        for (int j = 0; j < dtProduct.Columns.Count; j++)
                        {
                            sbDocumentBody.Append("<td class=\"Content\"style=\"border: 1px solid gray;\">" + dtProduct.Rows[i][j].ToString() + "</td>");
                        }
                        sbDocumentBody.Append("</tr>");
                    }
                    sbDocumentBody.Append("</table>");
                    sbDocumentBody.Append("</td></tr></table>");
                }
                byte[] byteArray = ASCIIEncoding.ASCII.GetBytes(sbDocumentBody.ToString());

                Response.Clear();
                Response.Headers.Add("Content-Type", "application/msword");
                Response.Headers.Add("Content-disposition", "attachment; filename=CompaniesList.doc");
                Response.Body.WriteAsync(byteArray);
                Response.Body.FlushAsync();
                File(byteArray, System.Net.Mime.MediaTypeNames.Application.Octet);
            }
        }


        private void ExportToJson(IEnumerable<CompanyDTO> companyDTO)
        {
            string jsonProductList = new JavaScriptSerializer().Serialize(companyDTO);
            byte[] byteArray = ASCIIEncoding.ASCII.GetBytes(jsonProductList);

            Response.Clear();
            Response.Headers.Clear();
            Response.ContentType = "application/json";
            Response.Headers.Add("Content-Length", jsonProductList.Length.ToString());
            Response.Headers.Add("Content-Disposition", "attachment; filename=Companies.json;");
            Response.Body.WriteAsync(byteArray);
            Response.Body.FlushAsync();
        }

        private void ExportToXML(IEnumerable<CompanyDTO> companyDTO)
        {
            XmlDocument xml = new XmlDocument();
            XmlElement root = xml.CreateElement("Companies");
            xml.AppendChild(root);
            foreach (var company in companyDTO)
            {

                XmlElement child = xml.CreateElement("Company");
                child.SetAttribute("Name", company.Name);
                child.SetAttribute("Address", company.Address);
                child.SetAttribute("HeadFullName", company.HeadFullName);
                child.SetAttribute("CreateDate", company.CreateDate.ToString());
                child.SetAttribute("UpdateDate", company.UpdateDate.ToString());
                child.SetAttribute("PhoneNumber", company.PhoneNumber);
                child.SetAttribute("Email", company.Email);
                root.AppendChild(child);
            }
            byte[] byteArray = ASCIIEncoding.ASCII.GetBytes(xml.OuterXml.ToString());

            Response.Clear();
            Response.Headers.Clear();
            Response.ContentType = "application/xml";
            Response.Headers.Add("Content-Disposition", "attachment; filename=Companies.xml;");
            Response.Body.WriteAsync(byteArray);
            Response.Body.Flush();
        }


        private DataTable GetDataTable(IEnumerable<CompanyDTO> companyDTO)
        {
            DataTable dtEmployee = new DataTable("Companies");
            dtEmployee.Columns.AddRange(new DataColumn[7] {
                new DataColumn("Name"),
                new DataColumn("Address"),
                new DataColumn("HeadFullName"),
                new DataColumn("CreateDate"),
                new DataColumn("UpdateDate"),
                new DataColumn("PhoneNumber"),
                new DataColumn("Email"),
        });

            foreach (var employee in companyDTO)
            {
                dtEmployee.Rows.Add(
                    employee.Name,
                    employee.Address,
                    employee.HeadFullName,
                    employee.CreateDate,
                    employee.UpdateDate,
                    employee.PhoneNumber,
                    employee.Email);
            }

            return dtEmployee;
        }



        [HttpGet]
        public IActionResult Create()
        => View();


        [HttpPost]
        public ActionResult Create(CompanyDTO companyDTO)
        {
            if (!_companyService.IsUnique(companyDTO))
            {
                if (ModelState.IsValid)
                {
                    _companyService.Create(companyDTO);
                    ViewBag.CreateResult = "Company is successfully created!";
                }
                else
                {
                    ModelState.AddModelError("", "Not correct data!");
                }
            }
            else
            {
                ModelState.AddModelError("", "Company already exists!");
            }
            return View();
        }

        public IActionResult Delete(int id)
        {
            _companyService.Delete(id);
            return Redirect("~/Company/Show");
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (_companyService.GetList().Any(x => x.Id == id) && id > 0)
                return View(_companyService.GetById(id));
            else
            {
                ModelState.AddModelError("", "Eror 400 - Bad Request!");
                return View();
            }
        }

        [HttpPost]
        public IActionResult Edit(CompanyDTO companyDTO)
        {
            if (_companyService.GetList().Any(x => x.Id == companyDTO.Id) && companyDTO.Id > 0)
            {
                if (!_companyService.IsUnique(companyDTO))
                {
                    _companyService.Update(companyDTO);
                    ViewBag.CreateResult = "Company is successfully edited!";
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
            return View(_companyService.GetById(companyDTO.Id));
        }


    }
}
