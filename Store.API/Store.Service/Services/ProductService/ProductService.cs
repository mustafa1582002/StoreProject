using AutoMapper;
using Store.Data.Entities;
using Store.Repository.Interfaces;
using Store.Repository.Specification.Product;
using Store.Service.Dtos;
using Store.Service.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<BrandTypeDetailsDto>> GetAllBrandsAsync()
        {
            var brands = await _unitOfWork.Repository<ProductBrand, int>().GetAllAsync();
            var mapperBrands = _mapper.Map<IReadOnlyList<BrandTypeDetailsDto>>(brands);
            return mapperBrands;
        }

        public async Task<PaginatedResultDto<ProductDetailsDto>> GetAllProductAsync(ProductSpecification input)
        {
            var specs = new ProductWithSpecification(input);

            var products = await _unitOfWork.Repository<Product, int>().GetAllWithSpecificationAsync(specs);

            var countspecs = new ProductWithFilterAndCountSpecification(input);

            var count = await _unitOfWork.Repository<Product, int>().CountSpecificationAsync(countspecs);

            var mapperProducts = _mapper.Map<IReadOnlyList<ProductDetailsDto>>(products);

            return new PaginatedResultDto<ProductDetailsDto>(input.PageIndex, input.PageSize, count, mapperProducts);
        }

        public async Task<IReadOnlyList<BrandTypeDetailsDto>> GetAllTypesAsync()
        {
            var brands = await _unitOfWork.Repository<ProductType, int>().GetAllAsync();
            var mapperBrands = _mapper.Map<IReadOnlyList<BrandTypeDetailsDto>>(brands);
            return mapperBrands;
        }

        public async Task<ProductDetailsDto> GetProductByIdAsync(int? id)
        {
            if (id is null)
                throw new Exception("Id is null");

            var specs = new ProductWithSpecification(id);

            var product = await _unitOfWork.Repository<Product, int>().GetWithSpecificationByIdAsync(specs);

            if (product is null)
                throw new Exception("Product NotFound");

            var mapperProducts = _mapper.Map<ProductDetailsDto>(product);
            return mapperProducts;
        }
    }
}
