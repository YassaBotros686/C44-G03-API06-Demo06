using ECommerce.ServicesAbstraction;
using ECommerce.Shared.DTOS.BasketDTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Controllers
{
    public class BasketController : ApiBaseController
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        } // Register

        #region Get Basket By Id

        [HttpGet]
        public async Task<ActionResult<BasketDTO>> GetBasket(string id) 
        {
            var Basket = await _basketService.GetBasketAsync(id);
            return Ok(Basket);
        }

        #endregion

        #region Create Or Update Basket

        [HttpPost]
        public async Task<ActionResult<BasketDTO>> CreateOrUpdateBasket(BasketDTO basket)
        {
            var Basket = await _basketService.CreateOrUpdateBasketAsync(basket);
            return Ok(Basket);
        }

        #endregion

        #region Delete Basket By Id

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteBasket(string id)
        {
            var Result = await _basketService.DeleteBasketAsync(id);
            return Ok(Result);
        }


        #endregion
    }
}
