﻿using FreeCourse.Services.Discount.Services;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.Discount.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController : CustomCountrollerBase
    {
        private readonly IDiscountService _discountService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public DiscountsController(IDiscountService discountService, ISharedIdentityService sharedIdentityService)
        {
            _discountService = discountService;
            _sharedIdentityService = sharedIdentityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return CreateActionResultInstance(await _discountService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return CreateActionResultInstance(await _discountService.GetById(id));
        }

        [HttpGet("[action]/{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var userId = _sharedIdentityService.GetUserId;
            return CreateActionResultInstance(await _discountService.GetByCodeAndUserId(code, userId));
        }


        [HttpPost]
        public async Task<IActionResult> Save(Models.Discount discount)
        {
            //var userId = _sharedIdentityService.GetUserId;
            return CreateActionResultInstance(await _discountService.Save(discount));
        }


        [HttpPut]
        public async Task<IActionResult> Update(Models.Discount discount)
        {
            //var userId = _sharedIdentityService.GetUserId;
            return CreateActionResultInstance(await _discountService.Update(discount));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            //var userId = _sharedIdentityService.GetUserId;
            return CreateActionResultInstance(await _discountService.Delete(id));
        }


    }
}
