using System;
using System.ComponentModel.DataAnnotations;

namespace TestEx_BelGosLes.Models.DTO
{
    public class EmployeeDTO
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Enter Full Name, please !")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Full Name length is [5;50] !")]
        public string FullName { get; set; }
        public DateTime CreateDate { get; set; }


        [Required(ErrorMessage = "Enter Address, please !")]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "Address length is [10;100] !")]
        public string Address { get; set; }


        [StringLength(40, MinimumLength = 9, ErrorMessage = "Phone Number length is [9;40] !")]
        [Required(ErrorMessage = "Enter Phone Number, please !")]
        [RegularExpression(@"[+]{1}[0-9]*", ErrorMessage = "Not Correct Phone Number !")]
        public string Phone { get; set; }


        [Required(ErrorMessage = "Enter Position, please !")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Position length is [2;50] !")]
        public string Position { get; set; }


        [Required(ErrorMessage = "Enter Department, please !")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Department length is [5;50] !")]
        public string Department { get; set; }


        public int CompanyId { get; set; } 
    }
}
