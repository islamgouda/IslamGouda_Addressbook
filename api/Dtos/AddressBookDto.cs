namespace api.Dtos
{
    public class AddressBookDto
    {
        public string? Id { get; set; }
        public string FullName { get; set; }
        public int JobId { get; set; }
        public int DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public string? JobName { get; set; }
        public string MobileNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Photo { get; set; }
        public int Age { get; set; }
       // public IFormFile? Imag { get; set; }
    }
}
