using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class AddressBook:IdentityUser
    {
        public AddressBook():base()
        {
            
        }
        public string FullName { get; set; }
        public int JobId { get; set; }
        public int? DepartmentId { get; set; }
        public string MobileNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Photo { get; set; }
        public int Age { get; set; }
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }
        [ForeignKey("JobId")]
        public virtual Job Job { get; set; }
    }
}
