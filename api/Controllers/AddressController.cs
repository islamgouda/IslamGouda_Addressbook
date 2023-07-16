using api.Dtos;
using api.Helper;
using AutoMapper;
using Core.Entities;
using Core.Interface;
using LinqKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly UserManager<AddressBook> _useManager;
        private readonly IMapper _mapper;
        private IWebHostEnvironment _webHostEnvironment;
        private IGenericRepository<Department> _DepartmentgenericRepository;
        private IGenericRepository<Job> _JobgenericRepository;
        public AddressController(IGenericRepository<Department> genericRepository, UserManager<AddressBook> userManager, IMapper mapper, IWebHostEnvironment webHostEnvironment, IGenericRepository<Job> jobgenericRepository)
        {
            _useManager = userManager;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _DepartmentgenericRepository = genericRepository;
            _JobgenericRepository = jobgenericRepository;

        }
        [HttpGet]
        public IActionResult Get()
        {
            var list = _useManager.Users.ToList();
            if (list != null)
            {
                var data = _mapper.Map<List<AddressBookDto>>(list);
                foreach (var item in data)
                {
                    if (item.DepartmentId != null)
                    {
                        item.DepartmentName = _DepartmentgenericRepository.GetByIdAsync(item.DepartmentId).Result.Name;
                    }
                    if (item.JobId != null)
                    {
                        item.JobName = _JobgenericRepository.GetByIdAsync(item.JobId).Result.Name;
                    }
                    if (item.Photo != null)
                    {
                        item.Photo = Path.Combine("https://localhost:7051/", item.Photo);
                    }

                }
                return Ok(data);
            }

            return NotFound();
        }
        [HttpGet("GetAllPage")]
        [Authorize]
        public async Task<PageResultList<AddressBookDto>> GetAllPage([FromQuery] string data)
        {
            var searchModel = JsonConvert.DeserializeObject<PageSearch>(data);
            var predicate = PredicateBuilder.New<AddressBookDto>(true);
            if (searchModel.data != null)
            {
                if (!string.IsNullOrEmpty(searchModel.data.Name))
                {
                    predicate = predicate.And(p =>
                    p.FullName.Contains(searchModel.data.Name.ToLower()) ||
                    p.Address.Contains(searchModel.data.Name.ToLower())
                    );
                }
            }
            var users = _useManager.Users.ToList();
            var Adddata = _mapper.Map<List<AddressBookDto>>(users);
            if (users != null)
            {
                //var data = _mapper.Map<List<AddressBookDto>>(list);
                foreach (var item in Adddata)
                {
                    if (item.DepartmentId != null)
                    {
                        item.DepartmentName = _DepartmentgenericRepository.GetByIdAsync(item.DepartmentId).Result.Name;
                    }
                    if (item.JobId != null)
                    {
                        item.JobName = _JobgenericRepository.GetByIdAsync(item.JobId).Result.Name;
                    }
                    if (item.Photo != null)
                    {
                        item.Photo = Path.Combine("https://localhost:7051/", item.Photo);
                    }

                }
                // return Ok(data);
            }
            var query = Adddata.Where(predicate);
            var Totalcount = query.Count();
            var fulldata = query
                   .Skip((searchModel.Page) * searchModel.PageSize)
                   .Take(searchModel.PageSize);
           // var mapped=_mapper.Map<List<AddressBookDto>>(fulldata);
            
            return new PageResultList<AddressBookDto>
            {
                PageSize = searchModel.PageSize,
                CurrentPage = searchModel.Page,
                TotalCount = Totalcount,
                Data = (List<AddressBookDto>)fulldata
            };
        }
        [HttpGet("GetById")]
        public IActionResult GetById(string Id)
        {
            var list = _useManager.Users.FirstOrDefault(e => e.Id == Id);
            if (list != null)
            {
                // list.Photo = FileHelper.LoadImage(list.Photo);
                return Ok(_mapper.Map<AddressBookDto>(list));
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddAdress(AddressBookDto addressBookDto)
        {
            addressBookDto.Photo = FileHelper.SaveStudentImageFromBase64(addressBookDto.Photo);
            var addressBook = _mapper.Map<AddressBook>(addressBookDto);
            addressBook.Id = Guid.NewGuid().ToString();
            //addressBook.Photo = UploadedFile(addressBookDto);
            var res = await _useManager.CreateAsync(addressBook);
            if (res.Succeeded)
            {
                return Ok(addressBook);
            }
            else
            {
                return BadRequest();
            }

        }
        [HttpPut]
        public async Task<IActionResult> EditAdress(AddressBookDto addressBookDto)
        {
            // FileHelper fileHelper=new FileHelper
            addressBookDto.Photo = FileHelper.SaveStudentImageFromBase64(addressBookDto.Photo);
            var old = _useManager.Users.FirstOrDefault(e => e.Id == addressBookDto.Id);
            old.PhoneNumber = addressBookDto.MobileNumber;
            old.FullName = addressBookDto.FullName;
            old.JobId = addressBookDto.JobId;
            old.DepartmentId = addressBookDto.DepartmentId;
            old.MobileNumber = addressBookDto.MobileNumber;
            old.DateOfBirth = addressBookDto.DateOfBirth;
            old.Address = addressBookDto.Address;

            old.Email = addressBookDto.Email;
            old.Password = addressBookDto.Password;
            old.Age = addressBookDto.Age;
            old.UserName = addressBookDto.FullName;
            old.NormalizedUserName = addressBookDto.FullName;
            old.EmailConfirmed = true;
            old.PhoneNumber = addressBookDto.MobileNumber;
            old.PhoneNumberConfirmed = true;
            old.Photo = addressBookDto.Photo;
            // _mapper.Map<AddressBook>(addressBookDto);
            //addressBook.Photo = UploadedFile(addressBookDto);

            var res = await _useManager.UpdateAsync(old);
            if (res.Succeeded)
            {
                return Ok(old);
            }
            else
            {
                return BadRequest();
            }

        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string Id)
        {
            var res = await _useManager.DeleteAsync(_useManager.Users.FirstOrDefault(e => e.Id == Id));
            if (res.Succeeded)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }

        }
        [HttpGet]
        [Route("UploadStudentData")]
        public async Task<ActionResult> UploadStudentData()
        {
            ///  var dataKey = User.GetAuthDataKeyFromUser();
            // if (dataKey != null && !string.IsNullOrEmpty(dataKey))
            var data = await _useManager.Users.ToListAsync();
            // var result = await _studentService.UploadStudentData(File, dataKey);
            var dto = _mapper.Map<List<ExportAddressExcel>>(data);

            var result = FileExcel.ExportEntityToByte(dto);
            if (result != null)
            {
                return this.File(
                        fileContents: result,
                        contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileDownloadName: "Data.xlsx"
                    );
            }

            return Ok(
                new
                {
                    Success = false,
                    Message = "Error uploading file"
                }
                );
        }


        /*
          public async Task<PageResultList<AddressBook>> GetAllPage([FromQuery] string data)
        {
            var searchModel = JsonConvert.DeserializeObject<PageSearch>(data);
            var predicate = PredicateBuilder.New<AddressBook>(true);
            if (searchModel.data != null)
            {
                if (!string.IsNullOrEmpty(searchModel.data.Name))
                {
                    predicate = predicate.And(p =>
                    p.FullName.Contains(searchModel.data.Name.ToLower()) ||
                    p.Address.Contains(searchModel.data.Name.ToLower())
                    );
                }
            }
            var query = _useManager.Users.Where(predicate);
            var Totalcount = query.Count();
            var fulldata = await query
                   .Skip((searchModel.Page) * searchModel.PageSize)
                   .Take(searchModel.PageSize).ToListAsync();
            var mapped=_mapper.Map<List<AddressBookDto>>(fulldata);
            if (fulldata != null)
            {
                //var data = _mapper.Map<List<AddressBookDto>>(list);
                foreach (var item in mapped)
                {
                    if (item.DepartmentId != null)
                    {
                        item.DepartmentName = _DepartmentgenericRepository.GetByIdAsync(item.DepartmentId).Result.Name;
                    }
                    if (item.JobId != null)
                    {
                        item.JobName = _JobgenericRepository.GetByIdAsync(item.JobId).Result.Name;
                    }
                    if (item.Photo != null)
                    {
                        item.Photo = Path.Combine("https://localhost:7051/", item.Photo);
                    }

                }
               // return Ok(data);
            }
            return new PageResultList<AddressBook>
            {
                PageSize = searchModel.PageSize,
                CurrentPage = searchModel.Page,
                TotalCount = Totalcount,
                Data = fulldata
            };
        }
         
         */
    }
}
