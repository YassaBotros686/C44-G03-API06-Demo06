using ECommerce.Presentation.Attributes;
using ECommerce.ServicesAbstraction;
using ECommerce.Shared;
using ECommerce.Shared.DTOS.ProductDtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Controllers
{
    public class ProductsController : ApiBaseController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        #region Get All Products

        [HttpGet]
        [RedisCache]
        // BaseURL/api/Products/
        public async Task<ActionResult<PaginatedResult<ProductDTO>>> GetAllProducts([FromQuery] ProductQueryParams queryParams)
        {
            var Products = await _productService.GetAllProductsAsync(queryParams);
            return Ok(Products);
        }

        #endregion

        #region Get Product By Id

        [HttpGet("{id}")]
        // BaseURL/api/Products/1
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            var Result = await _productService.GetProductByIdAsync(id);
            return HandleResult<ProductDTO>(Result);
        }

        #endregion

        #region Get All Brands

        [HttpGet("Brands")]
        // BaseURL/api/Products/Brands
        public async Task<ActionResult<IEnumerable<BrandDTO>>> GetAllBrands()
        {
            var Brands = await _productService.GetAllBrandsAsync();
            return Ok(Brands);
        }

        #endregion

        #region Get All Types

        [HttpGet("Types")]
        // BaseURL/api/Products/Types
        public async Task<ActionResult<IEnumerable<TypeDTO>>> GetAllTypes()
        {
            var Types = await _productService.GetAllTypesAsync();
            return Ok(Types);
        }

        #endregion
    }
}
