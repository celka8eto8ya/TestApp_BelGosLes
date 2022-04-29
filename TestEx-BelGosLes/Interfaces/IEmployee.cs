using System.Collections.Generic;
using TestEx_BelGosLes.Models.DTO;

namespace TestEx_BelGosLes.Interfaces
{
    public interface IEmployee
    {
        IEnumerable<EmployeeDTO> GetList();
        void Create(EmployeeDTO employeeDTO);
        void Delete(int id);
        void Update(EmployeeDTO employeeDTO);
        EmployeeDTO GetById(int id);
        bool IsUnique(EmployeeDTO employeeDTO);
        IEnumerable<EmployeeDTO> Sort(IEnumerable<EmployeeDTO> employees, string sortOrder);
        byte[] ExportToWord(IEnumerable<EmployeeDTO> employeeDTO);
        byte[] ExportToXML(IEnumerable<EmployeeDTO> employeeDTO);
    }
}
