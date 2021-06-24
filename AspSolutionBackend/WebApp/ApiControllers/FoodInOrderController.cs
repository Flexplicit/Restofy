using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.BLL.App;
using Contracts.DAL.App;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PublicApiDTO.v1.v1.Mappers;
using BllAppDTO = BLL.App.DTO.OrderModels;
using DalAppDTO = DAL.App.DTO.OrderModels;
using PublicDTO = PublicApiDTO.v1.v1.OrderModels;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// RESTful api service for FoodInOrder objects
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class FoodInOrderController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly FoodInOrderMapper _mapper;

        /// <summary>
        /// Constructor for FoodInOrder API
        /// </summary>
        /// <param name="bll">Injected bll</param>
        /// <param name="mapper">Injected mapper</param>
        public FoodInOrderController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _mapper = new FoodInOrderMapper(mapper);
        }

        // GET: api/FoodInOrder
        // /// <summary>
        // /// Gets all the FoodInOrder objects
        // /// </summary>
        // /// <returns>All the FoodInOrder objects</returns>
        // [HttpGet]
        // public async Task<ActionResult<IEnumerable<PublicApiDTO.v1.v1.OrderModels.FoodInOrder>>> GetFoodInOrders()
        // {
        //     return Ok((await _bll.FoodInOrders.GetAllAsync())
        //         .Select(x => _mapper.Map(x)));
        // }


        /// <summary>
        /// Gets food in specified order
        /// </summary>
        /// <param name="id">order id</param>
        /// <returns>FoodInOrder object with extra food info</returns>
        [HttpGet("{id:guid}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PublicDTO.FoodInOrder>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<PublicApiDTO.v1.v1.OrderModels.FoodInOrderWithFood>>>
            GetFoodInOrderByOrderId(Guid id)
        {
            var foodInOrder = (await _bll.FoodInOrders.GetFoodInOrderAccordingToOrderIdAsync(id))
                .Select(FoodInOrderMapper.BllToApiFoodInOrderExtra).ToList();
            return Ok(foodInOrder);
        }

        // PUT: api/FoodInOrder/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Updates a FoodInOrder object according to the id
        /// </summary>
        /// <param name="id">Id of the FoodInOrder object to update</param>
        /// <param name="foodInOrder">Updated version of the FoodInOrder object</param>
        /// <returns>Bad request if update fails, NoContent if succeeds</returns>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(PublicApiDTO.v1.v1.OrderModels.FoodInOrder), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PutFoodInOrder(Guid id, PublicApiDTO.v1.v1.OrderModels.FoodInOrder foodInOrder)
        {
            if (id != foodInOrder.Id)
            {
                return BadRequest();
            }

            _bll.FoodInOrders.Update(_mapper.Map(foodInOrder)!);


            await _bll.SaveChangesTask();

            return NoContent();
        }

        // POST: api/FoodInOrder
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Creates a FoodInOrder object
        /// </summary>
        /// <param name="foodInOrder">FoodInOrder object to create</param>
        /// <returns>Created FoodInOrder object with a Guid id attached to it</returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PublicApiDTO.v1.v1.OrderModels.FoodInOrder>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<PublicApiDTO.v1.v1.OrderModels.FoodInOrder>>> PostFoodInOrder(
            PublicDTO.FoodInOrderCreate[] foodInOrder)
        {
            if (foodInOrder.Any(orderItem => orderItem.FoodId == default || orderItem.Amount == 0) &&
                foodInOrder.Length > 0)
            {
                return BadRequest("Amount and FoodId is required");
            }

            var bllItems = foodInOrder.Select(cartItem =>
                _bll.FoodInOrders.Add(FoodInOrderMapper.ApiFoodInOrderCreateToBll(cartItem))).ToList();
            await _bll.SaveChangesTask();

            var publicApiItems = bllItems
                .Select(cartItem => _mapper.Map(cartItem))
                .ToList();

            return CreatedAtAction("PostFoodInOrder", publicApiItems);
        }

        // DELETE: api/FoodInOrder/5
        /// <summary>
        /// Deletes a FoodInOrder object according to the given id
        /// </summary>
        /// <param name="id">Guid id to delete with</param>
        /// <returns>NotFound 404 if id doesn't exist and NoContent if deletion was successful</returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteFoodInOrder(Guid id)
        {
            var foodInOrder = await _bll.FoodInOrders.FirstOrDefaultAsync(id);
            if (foodInOrder == null)
            {
                return NotFound();
            }

            _bll.FoodInOrders.Remove(foodInOrder);
            await _bll.SaveChangesTask();

            return NoContent();
        }
    }
}