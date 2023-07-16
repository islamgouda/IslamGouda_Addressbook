using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interface;
using Core.Specifications;
using api.Dtos;
using AutoMapper;
using api.Helper;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseApiController
    {
        private readonly Context _context;
        private readonly IProductRepository _productRepository;
      //  private readonly IGenericRepository<Product> _ProductgenericRepository;
       // private readonly IGenericRepository<Productype> _TypegenericRepository;
        //private readonly IGenericRepository<ProductBrand> _BrandgenericRepository;
        //private readonly IMapper _mapper;
       /* public ProductController(IMapper mapper, Context context, IProductRepository productRepository, IGenericRepository<Product> productgenericRepository, IGenericRepository<Productype> typegenericRepository, IGenericRepository<ProductBrand> brandgenericRepository)
        {
            _context = context;
            _productRepository = productRepository;
            _ProductgenericRepository = productgenericRepository;
            _TypegenericRepository = typegenericRepository;

            _BrandgenericRepository = brandgenericRepository;
            _mapper=mapper;

        }*/
     /*   [HttpGet("{id}")]
        public async Task<IActionResult> getProduct(int id)
        {
            var specific =new ProductWithTypesAndBrandSpecifications(id);
           
          var c= await _ProductgenericRepository.GetEntityWithSpecification(specific);
            
            var mapped = _mapper.Map<ProductToReturnDto>(c);
            return Ok(mapped);
          
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> getAll([FromQuery]ProductSpecParams productSpecParams)
        {
            var Specification = new ProductWithTypesAndBrandSpecifications(productSpecParams);
            var products = await _ProductgenericRepository.ListAsync(Specification);
            //await _ProductgenericRepository.GetAllAsync();
            //await _productRepository.GetAllProductsAsync();
           
            var countspec = new ProductWithFilterForCountSpecification(productSpecParams);
            var totalItems =await _ProductgenericRepository.CountAsync(countspec);
         var data=_mapper.Map<IEnumerable<ProductToReturnDto>>(products);
            var returndata = new Pagination<ProductToReturnDto>(productSpecParams.PageIndex,productSpecParams.PageSize,totalItems, (IReadOnlyList<ProductToReturnDto>)data);
         return Ok(returndata);
        }
        [HttpGet("GetALLBrands")]
        public async Task<IActionResult> getALLBrands()
        {
           var brands= await _BrandgenericRepository.GetAllAsync();
                //await _productRepository.GetAllBrandsAsync();
            return Ok(brands);
        }
        [HttpGet("GetALLTypes")]
        public async Task<IActionResult> getALLTypes()
        {
            var brands = await _TypegenericRepository.GetAllAsync();
                //await _productRepository.GetAllProductTypesAsync();
            return Ok(brands);
        }*/
    }
}
