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
    public class EmployeeService : IEmployee
    {
        private readonly IGenericRepository<Employee> _employeeRepository;

        public EmployeeService(IGenericRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public IEnumerable<EmployeeDTO> GetList()
            => _employeeRepository.GetList().Select(x => new EmployeeDTO
            {
                Id = x.Id,
                FullName = x.FullName,
                CreateDate = x.CreateDate,
                Address = x.Address,
                Phone = x.Phone,
                Position = x.Position,
                Department = x.Department,

                CompanyId = x.CompanyId
            });


        public bool IsUnique(EmployeeDTO employeeDTO)
           => _employeeRepository.GetList().Any(x => x.FullName == employeeDTO.FullName && x.Id != employeeDTO.Id);


        public void Create(EmployeeDTO employeeDTO)
            => _employeeRepository.Create(new Employee()
            {
                FullName = employeeDTO.FullName,
                CreateDate = DateTime.Now,
                Address = employeeDTO.Address,
                Phone = employeeDTO.Phone,
                Position = employeeDTO.Position,
                Department = employeeDTO.Department,

                CompanyId = employeeDTO.CompanyId
            });


        public void Delete(int id)
            => _employeeRepository.Delete(id);


        public void Update(EmployeeDTO employeeDTO)
            => _employeeRepository.Update(new Employee
            {
                Id = employeeDTO.Id,
                FullName = employeeDTO.FullName,
                CreateDate = employeeDTO.CreateDate,
                Address = employeeDTO.Address,
                Phone = employeeDTO.Phone,
                Position = employeeDTO.Position,
                Department = employeeDTO.Department,

                CompanyId = employeeDTO.CompanyId
            });


        public EmployeeDTO GetById(int id)
        {
            Employee employee = _employeeRepository.GetById(id);
            return new EmployeeDTO()
            {
                Id = employee.Id,
                FullName = employee.FullName,
                CreateDate = employee.CreateDate,
                Address = employee.Address,
                Phone = employee.Phone,
                Position = employee.Position,
                Department = employee.Department,

                CompanyId = employee.CompanyId
            };
        }

        public IEnumerable<EmployeeDTO> Sort(IEnumerable<EmployeeDTO> employees, string sortOrder)
        {
            switch (sortOrder)
            {
                case "name_desc":
                    employees = employees.OrderByDescending(s => s.FullName).ToList();
                    break;
                case "CreateDate":
                    employees = employees.OrderBy(s => s.CreateDate).ToList();
                    break;
                case "CreateDate_desc":
                    employees = employees.OrderByDescending(s => s.CreateDate).ToList();
                    break;
                case "Address":
                    employees = employees.OrderBy(s => s.Address).ToList();
                    break;
                case "Address_desc":
                    employees = employees.OrderByDescending(s => s.Address).ToList();
                    break;
                case "PhoneNumber":
                    employees = employees.OrderBy(s => s.Phone).ToList();
                    break;
                case "PhoneNumber_desc":
                    employees = employees.OrderByDescending(s => s.Phone).ToList();
                    break;
                case "Position":
                    employees = employees.OrderBy(s => s.Position).ToList();
                    break;
                case "Position_desc":
                    employees = employees.OrderByDescending(s => s.Position).ToList();
                    break;

                case "Department":
                    employees = employees.OrderBy(s => s.Department).ToList();
                    break;
                case "Department_desc":
                    employees = employees.OrderByDescending(s => s.Department).ToList();
                    break;
            }
            return employees;
        }


        public byte[] ExportToWord(IEnumerable<EmployeeDTO> employeeDTO)
        {
            DataTable dtProduct = GetDataTable(employeeDTO);

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

        public byte[] ExportToXML(IEnumerable<EmployeeDTO> employeeDTO)
        {
            XmlDocument xml = new XmlDocument();
            XmlElement root = xml.CreateElement("Employees");
            xml.AppendChild(root);
            foreach (var employee in employeeDTO)
            {
                XmlElement child = xml.CreateElement("Employee");
                child.SetAttribute("FullName", employee.FullName);
                child.SetAttribute("CreateDate", employee.CreateDate.ToString());
                child.SetAttribute("Address", employee.Address);
                child.SetAttribute("Phone", employee.Phone);
                child.SetAttribute("Position", employee.Position);
                child.SetAttribute("Department", employee.Department);
                root.AppendChild(child);
            }
            return ASCIIEncoding.ASCII.GetBytes(xml.OuterXml.ToString());
        }

        private DataTable GetDataTable(IEnumerable<EmployeeDTO> employeeDTO)
        {
            DataTable dtEmployee = new DataTable("Employees");
            dtEmployee.Columns.AddRange(new DataColumn[6] {
                new DataColumn("FullName"),
                new DataColumn("CreateDate"),
                new DataColumn("Address"),
                new DataColumn("Phone"),
                new DataColumn("Position"),
                new DataColumn("Department"),
                });


            foreach (var employee in employeeDTO)
            {
                dtEmployee.Rows.Add(
                    employee.FullName,
                    employee.CreateDate,
                    employee.Address,
                    employee.Phone,
                    employee.Position,
                    employee.Department);
            }

            return dtEmployee;
        }

    }
}
