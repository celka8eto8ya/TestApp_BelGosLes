using System;
using System.Collections.Generic;
using System.Linq;
using TestEx_BelGosLes.Interfaces;
using TestEx_BelGosLes.Models.DTO;
using TestEx_BelGosLes.Models.Entities;
using System.Data;
using System.Text;
using System.Xml;


namespace TestEx_BelGosLes.Services
{
    public class CompanyService : ICompany
    {
        private readonly IGenericRepository<Company> _companyRepository;

        public CompanyService(IGenericRepository<Company> companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public IEnumerable<CompanyDTO> GetList()
            => _companyRepository.GetList().Select(x => new CompanyDTO
            {
                Id = x.Id,
                Name = x.Name,
                Address = x.Address,
                HeadFullName = x.HeadFullName,
                CreateDate = x.CreateDate,
                UpdateDate = x.UpdateDate,
                PhoneNumber = x.PhoneNumber,
                Email = x.Email
            });


        public bool IsUnique(CompanyDTO companyDTO)
           => _companyRepository.GetList().Any(x => x.Name == companyDTO.Name && x.Id != companyDTO.Id);


        public void Create(CompanyDTO companyDTO)
            => _companyRepository.Create(new Company()
            {
                Name = companyDTO.Name,
                Address = companyDTO.Address,
                HeadFullName = companyDTO.HeadFullName,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                PhoneNumber = companyDTO.PhoneNumber,
                Email = companyDTO.Email
            });


        public void Delete(int id)
            => _companyRepository.Delete(id);


        public void Update(CompanyDTO companyDTO)
            => _companyRepository.Update(new Company
            {
                Id = companyDTO.Id,
                Name = companyDTO.Name,
                Address = companyDTO.Address,
                HeadFullName = companyDTO.HeadFullName,
                CreateDate = companyDTO.CreateDate,
                UpdateDate = DateTime.Now,
                PhoneNumber = companyDTO.PhoneNumber,
                Email = companyDTO.Email
            });


        public CompanyDTO GetById(int id)
        {
            Company company = _companyRepository.GetById(id);
            return new CompanyDTO()
            {
                Id = company.Id,
                Name = company.Name,
                Address = company.Address,
                HeadFullName = company.HeadFullName,
                CreateDate = company.CreateDate,
                UpdateDate = company.UpdateDate,
                PhoneNumber = company.PhoneNumber,
                Email = company.Email
            };
        }

        public IEnumerable<CompanyDTO> Sort(IEnumerable<CompanyDTO> companies, string sortOrder)
        {
            switch (sortOrder)
            {
                case "name_desc":
                    companies = companies.OrderByDescending(s => s.Name).ToList();
                    break;
                case "Address":
                    companies = companies.OrderBy(s => s.Address).ToList();
                    break;
                case "Address_desc":
                    companies = companies.OrderByDescending(s => s.Address).ToList();
                    break;
                case "HeadFullName":
                    companies = companies.OrderBy(s => s.HeadFullName).ToList();
                    break;
                case "HeadFullName_desc":
                    companies = companies.OrderByDescending(s => s.HeadFullName).ToList();
                    break;
                case "CreateDate":
                    companies = companies.OrderBy(s => s.CreateDate).ToList();
                    break;
                case "CreateDate_desc":
                    companies = companies.OrderByDescending(s => s.CreateDate).ToList();
                    break;
                case "UpdateDate":
                    companies = companies.OrderBy(s => s.UpdateDate).ToList();
                    break;
                case "UpdateDate_desc":
                    companies = companies.OrderByDescending(s => s.UpdateDate).ToList();
                    break;
                case "PhoneNumber":
                    companies = companies.OrderBy(s => s.PhoneNumber).ToList();
                    break;
                case "PhoneNumber_desc":
                    companies = companies.OrderByDescending(s => s.PhoneNumber).ToList();
                    break;
                case "Email":
                    companies = companies.OrderBy(s => s.Email).ToList();
                    break;
                case "Email_desc":
                    companies = companies.OrderByDescending(s => s.Email).ToList();
                    break;
            }
            return companies;
        }

        public byte[] ExportToWord(IEnumerable<CompanyDTO> companyDTO)
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
                return ASCIIEncoding.ASCII.GetBytes(sbDocumentBody.ToString());
            }
            return ASCIIEncoding.ASCII.GetBytes("none");
        }

        public byte[] ExportToXML(IEnumerable<CompanyDTO> companyDTO)
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
            return ASCIIEncoding.ASCII.GetBytes(xml.OuterXml.ToString());
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


    }
}
