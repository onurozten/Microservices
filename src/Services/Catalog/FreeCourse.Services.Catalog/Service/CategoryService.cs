using AutoMapper;
using FreeCourse.Services.Catalog.Dtos;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Services.Catalog.Settings;
using FreeCourse.Shared.Dtos;
using MongoDB.Driver;

namespace FreeCourse.Services.Catalog.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;

        public CategoryService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            
            _mapper = mapper;
        }

        public async Task<Response<List<CategoryDto>>> GetAllAsync()
        {
            var categories = await _categoryCollection.Find(category => true).ToListAsync();

            var dtos = _mapper.Map<List<CategoryDto>>(categories);
            return Response<List<CategoryDto>>.Success(dtos, 200);
        }


        public async Task<Response<CategoryDto>> CreateAsync(CategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            await _categoryCollection.InsertOneAsync(category);

            var dto = _mapper.Map<CategoryDto>(category);

            return Response<CategoryDto>.Success(dto, 200);
        }

        public async Task<Response<CategoryDto>> GetByIdAsync(string id)
        {
            var category = await _categoryCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (category == null)
                return Response<CategoryDto>.Fail("Category not found!", 404);

            var dto = _mapper.Map<CategoryDto>(category);

            return Response<CategoryDto>.Success(dto, 200);
        }

        public Task<Response<CategoryDto>> CreateAsync(CategoryCreateDto courseCreateDto)
        {
            throw new NotImplementedException();
        }

        public Task<Response<NoContent>> UpdateAsync(CourseUpdateDto courseUpdateDto)
        {
            throw new NotImplementedException();
        }

        public Task<Response<NoContent>> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }
    }

}


