﻿using AutoMapper;
using FreeCourse.Services.Catalog.Dtos;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Services.Catalog.Settings;
using FreeCourse.Shared.Dtos;
using MASS=MassTransit;
using MongoDB.Driver;
using FreeCourse.Shared.Messages;

namespace FreeCourse.Services.Catalog.Service
{
    public class CourseService : ICourseService
    {
        private readonly IMongoCollection<Course> _courseCollection;
        private readonly IMapper _mapper;
        private readonly MASS.IPublishEndpoint _publishEndpoint;
        private readonly IMongoCollection<Category> _categoryCollection;

        public CourseService(IMapper mapper, IDatabaseSettings databaseSettings, MASS.IPublishEndpoint publishEndpoint)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _courseCollection = database.GetCollection<Course>(databaseSettings.CourseCollectionName);
            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);

            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Response<List<CourseDto>>> GetAllAsync()
        {
            var courses = await _courseCollection.Find(course => true).ToListAsync();

            if (courses.Any())
            {
                foreach (var course in courses)
                {
                    course.Category = await _categoryCollection.Find(x => x.Id == course.CategoryId).FirstOrDefaultAsync();
                }
            }
            else
            {
                courses = new List<Course>();
            }

            var dtos = _mapper.Map<List<CourseDto>>(courses);
            return Response<List<CourseDto>>.Success(dtos, 200);
        }


        public async Task<Response<CourseDto>> GetByIdAsync(string id)
        {
            var course = await _courseCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (course == null)
                return Response<CourseDto>.Fail("Course not found!", 404);

            course.Category = await _categoryCollection.Find(x => x.Id == course.CategoryId).FirstAsync();

            var dto = _mapper.Map<CourseDto>(course);

            return Response<CourseDto>.Success(dto, 200);
        }

        public async Task<Response<List<CourseDto>>> GetAllByUserIdAsync(string userId)
        {
            var courses = await _courseCollection.Find(x => x.UserId == userId).ToListAsync();

            if (courses.Any())
            {
                foreach (var course in courses)
                {
                    course.Category = await _categoryCollection.Find(x => x.Id == course.CategoryId).FirstAsync();
                }
            }
            else
            {
                courses = new List<Course>();
            }

            var dto = _mapper.Map<List<CourseDto>>(courses);

            return Response<List<CourseDto>>.Success(dto, 200);
        }

        public async Task<Response<CourseDto>> CreateAsync(CourseCreateDto courseCreateDto)
        {
            var newCourse = _mapper.Map<Course>(courseCreateDto);
            newCourse.CreatedTime = DateTime.Now;

            await _courseCollection.InsertOneAsync(newCourse);

            var dto = _mapper.Map<CourseDto>(newCourse);

            return Response<CourseDto>.Success(dto, 200);
        }


        public async Task<Response<NoContent>> UpdateAsync(CourseUpdateDto courseUpdateDto)
        {
            var updatedCourse = _mapper.Map<Course>(courseUpdateDto);

            var result = await _courseCollection.FindOneAndReplaceAsync(x => x.Id == courseUpdateDto.Id, updatedCourse);

            if (result == null)
                return Response<NoContent>.Fail("An error occured on update", 404);

            await _publishEndpoint.Publish<CourseNameChangedEvent>(new CourseNameChangedEvent 
                { CourseId = courseUpdateDto.Id, UpdatedName = courseUpdateDto.Name });


            await _publishEndpoint.Publish<CourseNameOrPriceChangedEvent>(new CourseNameOrPriceChangedEvent
            { CourseId = courseUpdateDto.Id, UpdatedName = courseUpdateDto.Name, UpdatedPrice = courseUpdateDto.Price });

            return Response<NoContent>.Success(204);
        }

        public async Task<Response<NoContent>> DeleteAsync(string id)
        {
            var result = await _courseCollection.DeleteOneAsync(x => x.Id == id);

            if (result.DeletedCount > 0)
            {
                return Response<NoContent>.Success(204);

            }

            return Response<NoContent>.Fail("Course not found", 404);
        }

    }

}


