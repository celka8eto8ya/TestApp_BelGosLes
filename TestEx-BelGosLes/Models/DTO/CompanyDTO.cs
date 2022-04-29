using System;
using System.ComponentModel.DataAnnotations;

namespace TestEx_BelGosLes.Models.DTO
{
    public class CompanyDTO
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Enter Name, please !")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Name length is [5;50] !")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Enter Address, please !")]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "Name length is [10;100] !")]
        public string Address { get; set; }


        [Required(ErrorMessage = "Enter Head Full Name, please !")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Head Full Name length is [10;50] !")]
        public string HeadFullName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }


        [StringLength(40, MinimumLength = 9, ErrorMessage = "Phone Number length is [9;40] !")]
        [Required(ErrorMessage = "Enter Phone Number, please !")]
        [RegularExpression(@"[+]{1}[0-9]*", ErrorMessage = "Not Correct Phone Number !")]
        public string PhoneNumber { get; set; }


        [Required(ErrorMessage = "Enter Email, please !")]
        [EmailAddress(ErrorMessage = "Not Correct Email !")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Not Correct Email !")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Email length is [6;50] !")]
        public string Email { get; set; }
    }
}
