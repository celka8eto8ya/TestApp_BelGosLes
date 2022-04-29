using System.Collections.Generic;
using TestEx_BelGosLes.Models.DTO;

namespace TestEx_BelGosLes.Interfaces
{
   public interface ICompany
    {
        IEnumerable<CompanyDTO> GetList();
        void Create(CompanyDTO companyDTO);
        void Delete(int id);
        void Update(CompanyDTO companyDTO);
        CompanyDTO GetById(int id);
        bool IsUnique(CompanyDTO companyDTO);
        IEnumerable<CompanyDTO> Sort(IEnumerable<CompanyDTO> companies, string sortOrder);
        byte[] ExportToWord(IEnumerable<CompanyDTO> companyDTO);
        byte[] ExportToXML(IEnumerable<CompanyDTO> companyDTO);
    }
}
