using AutoMapper;
using Core.Entities;
using Core.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly UserManager<AddressBook> _useManager;
        private readonly IMapper _mapper;
        private IWebHostEnvironment _webHostEnvironment;
       
        public FilesController(UserManager<AddressBook> userManager, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _useManager = userManager;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
           

        }
        [HttpPost, DisableRequestSizeLimit]
        private string UploadedFile()
        {
            var address = Request.Form.Files[0];
            string uniqueFileName = null;

            if (address != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, $"images/adressbooks");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + address.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    address.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
