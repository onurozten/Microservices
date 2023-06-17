using FreeCourse.Shared.Services;
using FreeCourse.Web.Models;
using FreeCourse.Web.Models.Catalog;
using FreeCourse.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace FreeCourse.Web.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly ICatalogService _catalogService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public CoursesController(ICatalogService catalogService, ISharedIdentityService sharedIdentityService)
        {
            _catalogService = catalogService;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _catalogService.GetAllCoursesByUserIdAsync(_sharedIdentityService.GetUserId);
            return View(courses);
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _catalogService.GetAllCategoryAsync();
            ViewBag.categoryList = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseCreateInput courseCreateInput)
        {

            if (!ModelState.IsValid)
            {
                var categories = await _catalogService.GetAllCategoryAsync();
                ViewBag.categoryList = new SelectList(categories, "Id", "Name");
                return View();
            }

            courseCreateInput.UserId = _sharedIdentityService.GetUserId;
            var result = await  _catalogService.CreateCourseAsync(courseCreateInput);

            return RedirectToAction(nameof(Index));

        }


        public async Task<IActionResult> Update(string id)
        {
            var course = await _catalogService.GetByCourseIdAsync(id);
            var categories = await _catalogService.GetAllCategoryAsync();

            if (course == null)
                return RedirectToAction(nameof(Index));

            ViewBag.categoryList = new SelectList(categories, "Id", "Name", course.CategoryId);

            CourseUpdateInput courseUpdateInput = new()
            {
                Id = course.Id,
                CategoryId = course.CategoryId,
                Name = course.Name,
                Price = course.Price,
                Feature = course.Feature,
                UserId = course.UserId,
                Picture = course.Picture,
                Description = course.Description
            };

            return View(courseUpdateInput);
        }


        [HttpPost]
        public async Task<IActionResult> Update(CourseUpdateInput courseCreateInput)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _catalogService.GetAllCategoryAsync();
                ViewBag.categoryList = new SelectList(categories, "Id", "Name", courseCreateInput.CategoryId);
                return View(courseCreateInput);
            }

            courseCreateInput.UserId = _sharedIdentityService.GetUserId;
            var result = await _catalogService.UpdateCourseAsync(courseCreateInput);

            return RedirectToAction(nameof(Index));

        }



        
        public async Task<IActionResult> Delete(string  id)
        {
            await _catalogService.DeleteCourseAsync(id);

            return RedirectToAction(nameof(Index));

        }


    }
}