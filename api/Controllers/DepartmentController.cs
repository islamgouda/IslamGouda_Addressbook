using api.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interface;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : BaseApiController
    {
        private readonly IGenericRepository<Department> _DepartmentgenericRepository;
        private readonly Context _contxt;
        private readonly IMapper _mapper;
        public DepartmentController(IGenericRepository<Department> DepartmentgenericRepository, IMapper mapper)
        {
            _DepartmentgenericRepository = DepartmentgenericRepository;
            _mapper = mapper;
        }
       // [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Jobs = await _DepartmentgenericRepository.GetAllAsync();
            return Ok(Jobs);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int Id)
        {
            var department = await _DepartmentgenericRepository.GetByIdAsync(Id);
            if(department != null) {
                return Ok(department);
            }
            else
            {
                return BadRequest();
            }
           
        }
        [HttpPost]
        public async Task<IActionResult> Add(DepartmentDto Department)
        {

            await _DepartmentgenericRepository.Add(_mapper.Map<Department>(Department));
            var result = await _DepartmentgenericRepository.SaveEntitiesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Edit(DepartmentDto Department)
        {

            _DepartmentgenericRepository.Update(_mapper.Map<Department>(Department));
            var result = await _DepartmentgenericRepository.SaveEntitiesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int Id)
        {

            await _DepartmentgenericRepository.DeleteWhere(e => e.Id == Id);
            var result = await _DepartmentgenericRepository.SaveEntitiesAsync();
            return Ok();
        }
    }
}
