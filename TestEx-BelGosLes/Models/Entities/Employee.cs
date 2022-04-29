using System;

namespace TestEx_BelGosLes.Models.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime CreateDate { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }


        public int CompanyId { get; set; } // foreign key
        public Company Company { get; set; } // navigation property
    }
}
