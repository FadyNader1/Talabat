using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities.Products;
using Talabat.Core.Interfaces;
using Talabat.Core.Specifications;
using Talabat.DTO;
using Talabat.Errors;
using Talabat.Repository.Specifications;

namespace Talabat.Controllers
{
    /// <summary>
    /// Controller for managing product operations such as retrieving, adding, updating, and deleting products.
    /// Provides API endpoints for product-related actions.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class Productcontroller : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="Productcontroller"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work for database operations.</param>
        /// <param name="mapper">AutoMapper instance for mapping entities and DTOs.</param>
        public Productcontroller(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        /// <summary>
        /// Retrieves all products with optional filtering and searching.
        /// </summary>
        /// <param name="productParams">Filtering and searching parameters (such as brand, type, or name).</param>
        /// <returns>A list of products as <see cref="ProductDTO"/>.</returns>
        [HttpGet("GetAllProducts")]
        [ProducesResponseType(typeof(ApiHandleError), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<ProductDTO>>> GetAllProducts([FromQuery] ProductParams productParams)
        {
            var spec = new ProductSpecification(productParams);
            var product = await unitOfWork.Repository<Product>().GetAllSpecificationAsync(spec);
            if (product is null)
                return NotFound(new Errors.ApiHandleError(404));
            var productsmapp = mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDTO>>(product);
            return Ok(productsmapp);
        }
        /// <summary>
        /// Retrieves a specific product by its identifier.
        /// </summary>
        /// <param name="id">The product identifier.</param>
        /// <returns>The product data as <see cref="ProductDTO"/>.</returns>
        [HttpGet("GetProductById")]
        [ProducesResponseType(typeof(ApiHandleError), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            var spec = new ProductSpecification(id);
            var product = await unitOfWork.Repository<Product>().GetByIdSpecificationAsync(spec);
            if (product is null)
                return NotFound(new Errors.ApiHandleError(404));
            var productmapp = mapper.Map<Product, ProductDTO>(product);
            return Ok(productmapp);
        }
        /// <summary>
        /// Adds a new product to the system.
        /// </summary>
        /// <param name="productDTO">The product data to add.</param>
        /// <returns>The added product data.</returns>
        [HttpPost("AddProduct")]
        [ProducesResponseType(typeof(ApiHandleError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status201Created)]
        public async Task<ActionResult<ProductDTO>> AddProduct(ProductDTO productDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiHandleError(400, "Invalid Model State"));
            var product = mapper.Map<ProductDTO, Product>(productDTO);
            try
            {
                var result = unitOfWork.Repository<Product>().AddAsync(product);
                if (result is null)
                    return BadRequest(new Errors.ApiHandleError(400, "Failed to add product"));
                await unitOfWork.Complete();

            }
            catch (Exception ex)
            {
                return BadRequest(new ApiHandleError(400, ex.Message));
            }

            var productmapp = mapper.Map<Product, ProductDTO>(product);
            return Ok(productmapp);
        }
       
        /// <summary>
        /// Updates an existing product in the system.
        /// </summary>
        /// <param name="id">The identifier of the product to update.</param>
        /// <param name="productDTO">The new product data.</param>
        /// <returns>The updated product data.</returns>
        [HttpPut("UpdateProduct")]
        [ProducesResponseType(typeof(ApiHandleError),StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductDTO>> UpdateProduct(int id,ProductDTO productDTO)
        {
            // Validate the model state before proceeding with the update
            if (!ModelState.IsValid)
                return BadRequest(new ApiHandleError(400, "Invalid Model State"));

            var spec = new ProductSpecification(id);
            var product = await unitOfWork.Repository<Product>().GetByIdSpecificationAsync(spec);
            if (product is null)
                return NotFound(new ApiHandleError(404));
            product.Name = productDTO.Name;
            product.Description = productDTO.Description;
            product.PictureUrl = productDTO.PictureUrl;
            product.Price = productDTO.Price;
            product.ProductBrand_Id = productDTO.ProductBrand_Id;
            product.ProductType_Id = productDTO.ProductType_Id;
            product.ProductBrand.Name = productDTO.ProductBrand_Name;
            product.ProductType.Name = productDTO.ProductType_Name;
            try
            {
                unitOfWork.Repository<Product>().Update(product);
                await unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiHandleError(400, ex.Message));
            }

            var productmapp = mapper.Map<Product, ProductDTO>(product);
            return Ok(productmapp);
        }
        /// <summary>
        /// Deletes a product by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the product to delete.</param>
        /// <returns>A confirmation message upon successful deletion.</returns>
        [HttpDelete("DeleteProduct")]
        [ProducesResponseType(typeof(ApiHandleError), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<ActionResult<string>> DeleteProduct(int id)
        {
            var spec = new ProductSpecification(id);
            var product = unitOfWork.Repository<Product>().GetByIdSpecificationAsync(spec).Result;
            if (product is null)
                return NotFound(new ApiHandleError(404));
            try
            {
                unitOfWork.Repository<Product>().Delete(product);
                await unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiHandleError(400, ex.Message));
            }
            return $"this Product {product.Name} Deleted Successfully";
        }


    }
}