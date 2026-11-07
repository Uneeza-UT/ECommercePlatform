using AutoMapper;
using ECommercePlatform.Application.Contracts.Persistence;
using ECommercePlatform.Application.Contracts.Services;
using ECommercePlatform.Application.DTOs.Common;
using ECommercePlatform.Application.DTOs.Product;
using ECommercePlatform.Application.Enums;
using ECommercePlatform.Application.Exceptions;
using ECommercePlatform.Application.Validations.Product;
using ECommercePlatform.Domain;
using System.Linq.Expressions;

namespace ECommercePlatform.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductService(IMapper mapper,
            IProductRepository productRepository,
            ICategoryRepository categoryRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<List<GetProductsDto>> GetAsync(int categoryId, PaginationDto dto)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            
            if (category == null || category.IsDeleted)
            {
                throw new NotFoundException(nameof(Category), categoryId);
            }

            var products = await _productRepository.GetAllByCategoryIdAsync(categoryId, dto.PageNumber, dto.PageSize);
            return _mapper.Map<List<GetProductsDto>>(products);
        }



        public async Task<GetProductsDto> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null || product.IsDeleted)
            {
                throw new NotFoundException(nameof(Product), id);
            }

            return _mapper.Map<GetProductsDto>(product);
        }



        public async Task<int> CreateAsync(int categoryId, CreateProductDto dto)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);

            if (category == null || category.IsDeleted)
            {
                throw new NotFoundException(nameof(Category), categoryId);
            }

            // Validate the dto
            var validator = new CreateProductValidator();
            var validationResult = await validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                throw new BadRequestException("Invalid entity. ", validationResult);
            }

            // Ensure Product name is unique
            var isNameUnique = await _productRepository.IsNameUnique(dto.Name);

            if (!isNameUnique)
            {
                throw new ConflictException("Product Name already exists.");
            }


            var product = _mapper.Map<Product>(dto);
            product.CategoryId = categoryId;
            await _productRepository.CreateAsync(product);
            return product.Id;
        }


        public async Task UpdateAsync(int id, UpdateProductDto dto)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null || product.IsDeleted)
            {
                throw new NotFoundException(nameof(Product), id);
            }

            // Validate the dto
            var validator = new UpdateProductValidator();
            var validationResult = await validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                throw new BadRequestException("Invalid entity. ", validationResult);
            }

            // Ensure Product name is unique
            var isNameUnique = await _productRepository.IsNameUnique(dto.Name, id);

            if (!isNameUnique)
            {
                throw new ConflictException("Product Name already exists.");
            }

            _mapper.Map(dto, product);
            product.UpdatedAt = DateTime.UtcNow;

            await _productRepository.SaveChangesAsync();
        }



        public async Task DeleteAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null || product.IsDeleted)
            {
                throw new NotFoundException(nameof(Product), id);
            }

            product.IsDeleted = true;

            await _productRepository.SaveChangesAsync();
        }


        public async Task<List<GetProductsDto>> SearchAsync(SearchDto dto, PaginationDto paginationDto)
        {
            IReadOnlyList<Product> products;
            string searchTerm = dto.SearchTerm?.Trim();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                products = await _productRepository.GetAllAsync(paginationDto.PageNumber, paginationDto.PageSize);
            }

            products = await _productRepository.SearchAsync(searchTerm, paginationDto.PageNumber, paginationDto.PageSize);
            return _mapper.Map<List<GetProductsDto>>(products);
        }


        public async Task<List<GetProductsDto>> SortAsync(SortDto dto, PaginationDto paginationDto)
        {
            bool descending;
            Expression<Func<Product, object>> sortExpression;

            switch (dto.SortBy)
            {
                case SortOption.PriceLowestToHighest:
                    descending = false;
                    sortExpression = p => p.Price;
                    break;


                case SortOption.PriceHighestToLowest:
                    descending = false;
                    sortExpression = p => p.Price;
                    break;


                case SortOption.Ascending:
                    descending = false;
                    sortExpression = p => p.Name;
                    break;

                case SortOption.Descending:
                    descending = true;
                    sortExpression = p => p.Name;
                    break;

                case SortOption.Latest:
                    descending = true;
                    sortExpression = p => p.CreatedAt;
                    break;

                case SortOption.Oldest:
                    descending = false;
                    sortExpression = p => p.CreatedAt;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(dto.SortBy),
                        dto.SortBy,
                        "Invalid sort option.");
            }

            var products = await _productRepository
                .SortAsync(
                    descending,
                    sortExpression,
                    paginationDto.PageNumber,
                    paginationDto.PageSize);

            return _mapper.Map<List<GetProductsDto>>(products);
        }


        public async Task<List<GetProductsDto>> FilterAsync(FilterDto dto, PaginationDto paginationDto)
        {
            var products = await _productRepository
                .FilterAsync(
                    dto.FilterBy,
                    paginationDto.PageNumber,
                    paginationDto.PageSize);

            return _mapper.Map<List<GetProductsDto>>(products);
        }
    }
}
