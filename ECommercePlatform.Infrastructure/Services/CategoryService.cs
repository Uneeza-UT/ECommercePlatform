using AutoMapper;
using ECommercePlatform.Application.Contracts.Persistence;
using ECommercePlatform.Application.Contracts.Services;
using ECommercePlatform.Application.DTOs.Category;
using ECommercePlatform.Application.DTOs.Common;
using ECommercePlatform.Application.Exceptions;
using ECommercePlatform.Application.Validations.Category;
using ECommercePlatform.Domain;

namespace ECommercePlatform.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(IMapper mapper,
            ICategoryRepository categoryRepository)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public async Task<List<GetCategoryDto>> GetAsync(PaginationDto dto)
        {
            var categories = await _categoryRepository.GetAllAsync(dto.PageNumber, dto.PageSize);
            return _mapper.Map<List<GetCategoryDto>>(categories);
        }



        public async Task<GetCategoryDto> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdWithProductsAsync(id);

            if (category == null)
            {
                throw new NotFoundException(nameof(Category), id);
            }

            return _mapper.Map<GetCategoryDto>(category);
        }

        

        public async Task<int> CreateAsync(CreateCategoryDto dto)
        {
            // Validate the dto
            var validator = new CreateCategoryValidator();
            var validationResult = await validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                throw new BadRequestException("Invalid entity. ", validationResult);
            }

            // Ensure Category name is unique
            var isNameUnique = await _categoryRepository.IsNameUnique(dto.Name);

            if (!isNameUnique)
            {
                throw new ConflictException("Category Name already exists.");
            }


            var category = _mapper.Map<Category>(dto);
            await _categoryRepository.CreateAsync(category);
            return category.Id;
        }


        public async Task UpdateAsync(int id, UpdateCategoryDto dto)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null || category.IsDeleted)
            {
                throw new NotFoundException(nameof(Category), id);
            }

            // Validate the dto
            var validator = new UpdateCategoryValidator();
            var validationResult = await validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                throw new BadRequestException("Invalid entity. ", validationResult);
            }

            // Ensure Category name is unique
            var isNameUnique = await _categoryRepository.IsNameUnique(dto.Name, id);

            if (!isNameUnique)
            {
                throw new ConflictException("Category Name already exists.");
            }

            category.Name = dto.Name;
            category.UpdatedAt = DateTime.UtcNow;

            await _categoryRepository.SaveChangesAsync();
        }



        public async Task DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null || category.IsDeleted)
            {
                throw new NotFoundException(nameof(Category), id);
            }

            category.IsDeleted = true;

            await _categoryRepository.SaveChangesAsync();
        }
    }
}
