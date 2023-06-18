using FreeCourse.Web.Exceptions;
using FreeCourse.Web.Models;
using FreeCourse.Web.Models.Orders;
using FreeCourse.Web.Services.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FreeCourse.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly ICatalogService _catalogService;
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;

        public OrderController(ICatalogService catalogService, IBasketService basketService,
            IOrderService orderService)
        {
            _catalogService = catalogService;
            _basketService = basketService;
            _orderService = orderService;
        }

        public async Task<IActionResult> CheckOut()
        {
            var basket = await _basketService.Get();
            ViewBag.basket = basket;
            return View(new CheckoutInfoInput());
        }

        [HttpPost]
        public async Task<IActionResult> CheckOut(CheckoutInfoInput checkoutInfoInput)
        {
            // 1. yol senkron iletişim
            //var orderStatus = await _orderService.CreateOrder(checkoutInfoInput);
            // 2. yol asenkron iletişim

            var orderStatus = await _orderService.SuspendOrder(checkoutInfoInput);
            if (!orderStatus.IsSuccessful)
            {

                var basket = await _basketService.Get();
                ViewBag.basket = basket;
                ViewBag.error = orderStatus.Error;
                return View();
            }
            // 1. yol senkron iletişim
            //return RedirectToAction(nameof(SuccessfulCheckout), new {orderId = orderStatus.OrderId});

            // 2. yol asenkron iletişim
            return RedirectToAction(nameof(SuccessfulCheckout), new { orderId = new Random().Next(1,1000) });
        }


        public IActionResult SuccessfulCheckout(int orderId)
        {
            ViewBag.orderId = orderId;
            return View();
        }



        public async Task<IActionResult> CheckoutHistory(int orderId)
        {
            var orders = await _orderService.GetOrder();
            return View(orders);
        }


    }
}