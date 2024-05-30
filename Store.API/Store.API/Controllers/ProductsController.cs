using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.API.Helper;
using Store.Repository.Specification.Product;
using Store.Service.Dtos;
using Store.Service.HandleResponse;
using Store.Service.Helper;
using Store.Service.Services.ProductService;

namespace Store.API.Controllers
{
    [Authorize]
    public class ProductsController : BaseController
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        [Cache(90)]
        public async Task<ActionResult<IReadOnlyList<BrandTypeDetailsDto>>> GetAllBrands()
        => Ok(await _productService.GetAllBrandsAsync());
        [HttpGet]
        [Cache(90)]
        public async Task<ActionResult<IReadOnlyList<BrandTypeDetailsDto>>> GetAllTypes()
        => Ok(await _productService.GetAllTypesAsync());
        [HttpGet]
        [Cache(90)]
        public async Task<ActionResult<PaginatedResultDto<ProductDetailsDto>>> GetAllProducts([FromQuery] ProductSpecification input)
        {
            return Ok(await _productService.GetAllProductAsync(input));
        }
        [HttpGet]
        [Cache(90)]
        public async Task<ActionResult<ProductDetailsDto>> GetProductById(int? id)
        {
            if (id == null)
                return BadRequest(new CustomException(400,"Id is null"));

            var product =await _productService.GetProductByIdAsync(id);

            if(product is null)
                return NotFound(new CustomException(404));
            return Ok(product);
        }
    }
}
