using Core.Entities;
using Core.Interface;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly Context _context;

        public ProductRepository()
        {
        }

        public ProductRepository(Context context) {
        _context = context;
        }

      /*  public async Task<IReadOnlyList<ProductBrand>> GetAllBrandsAsync()
        {
            return await _context.ProductBrands.ToListAsync();
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products.Include(e=>e.productBrand)
                .Include(e=>e.ProductType).ToListAsync();
        }

        public async Task<IReadOnlyList<Productype>> GetAllProductTypesAsync()
        {
            return  await _context.ProductTypes.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
           return await _context.Products.Include(e => e.productBrand)
                .Include(e => e.ProductType).FirstOrDefaultAsync(p => p.Id == id);
        }
      */
    }
}
