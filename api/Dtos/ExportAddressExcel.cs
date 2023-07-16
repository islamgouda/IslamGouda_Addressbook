using System.ComponentModel;

namespace api.Dtos
{
    public class ExportAddressExcel
    {
        [DisplayName("FullName")]
        public string FullName { get; set; }
        [DisplayName("Job Id")] 
        public int JobId { get; set; }
        [DisplayName("Department Id")] 
        public int DepartmentId { get; set; }
        [DisplayName("Department Name")] 
        public string? DepartmentName { get; set; }
        [DisplayName("Job Name")] 
        public string? JobName { get; set; }
        [DisplayName("MobileNumber")] 
        public string MobileNumber { get; set; }
        [DisplayName("DateOfBirth")] 
        public DateTime DateOfBirth { get; set; }
        [DisplayName("Address")] 
        public string Address { get; set; }
        [DisplayName("Email")]

        public string Email { get; set; }
        [DisplayName("Password")] 
        public string Password { get; set; }
        [DisplayName("Photo")] 
        public string Photo { get; set; }
        [DisplayName("Age")] 
        public int Age { get; set; }
    }
}
