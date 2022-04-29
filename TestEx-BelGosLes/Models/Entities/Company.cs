using System;

namespace TestEx_BelGosLes.Models.Entities
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string HeadFullName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string PhoneNumber { get; set; }
        public string  Email { get; set; }
    }
}
