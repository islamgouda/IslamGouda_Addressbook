using api.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interface;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : BaseApiController
    {
        private readonly IGenericRepository<Job> _JobgenericRepository;
        private readonly Context _contxt;
        private readonly IMapper _mapper;
        public JobController(IGenericRepository<Job> JobgenericRepository, IMapper mapper)
        {
           _JobgenericRepository = JobgenericRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Jobs=await _JobgenericRepository.GetAllAsync();
            return Ok(Jobs);
        }
        [HttpPost]
        public async Task<IActionResult> Add(JobDto job)
        {

           await _JobgenericRepository.Add(_mapper.Map<Job>(job));
            var result = await _JobgenericRepository.SaveEntitiesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Edit(JobDto job)
        {

             _JobgenericRepository.Update(_mapper.Map<Job>(job));
            var result = await _JobgenericRepository.SaveEntitiesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int Id)
        {

           await _JobgenericRepository.DeleteWhere(e=>e.Id==Id);
            var result = await _JobgenericRepository.SaveEntitiesAsync();
            return Ok();
        }
    }
}
