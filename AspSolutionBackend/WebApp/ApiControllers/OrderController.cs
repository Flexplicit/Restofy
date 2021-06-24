using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.BLL.App;
using Extensions.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PublicApiDTO.v1.v1.Mappers;
using BllAppDTO = BLL.App.DTO.OrderModels;
using PublicDTO = PublicApiDTO.v1.v1.OrderModels;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// RESTful api service for Ordering food
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly OrderMapper _mapper;

        /// <summary>
        /// Constructor for Contacts
        /// </summary>
        /// <param name="bll">Injected bll via DI</param>
        /// <param name="mapper">Injected mapper via DI</param>
        public OrderController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _mapper = new OrderMapper(mapper);
        }

        // GET: api/Order
        /// <summary>
        /// Gets all order
        /// </summary>
        /// <returns>All orders</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(IEnumerable<PublicDTO.Order>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PublicDTO.Order>>> GetOrders()
        {
            return Ok(await _bll.Orders.GetAllAsync(User.GetUserId()!.Value));
        }

        /// <summary>
        /// Gets all active orders of a restaurant according to the extracted user id from the jwt
        /// </summary>
        /// <returns>All the active order a restaurant has</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(IEnumerable<PublicDTO.Order>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PublicDTO.Order>>> GetActiveOrders()
        {
            return Ok(await _bll.Orders.GetAllActiveOrdersAsync(User.GetUserId()!.Value));
        }

        /// <summary>
        /// Gets all finished and currently active user orders according to the extracted user id from the jwt
        /// </summary>
        /// <returns>All the active and inactive orders a user has</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PublicDTO.Order>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<PublicDTO.Order>>> GetUserOrders()
        {
            return Ok(await _bll.Orders.GetAllUserOrdersAsync(User.GetUserId()!.Value));
        }

        /// <summary>
        /// Gets all finished and currently active user orders according to the extracted restaurant user id from the jwt
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PublicDTO.Order>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<PublicDTO.Order>>> GetRestaurantOrders()
        {
            return Ok(await _bll.Orders.GetAllRestaurantOrdersAsync(User.GetUserId()!.Value));
        }


        /// <summary>
        /// Creates a new Order
        /// </summary>
        /// <param name="orderCreateDTO">Order object that we create, creates an order as well</param>
        /// <returns>Created order with updated fields</returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicDTO.Order), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PublicDTO.Order>> CreateOrder(PublicDTO.OrderCreate orderCreateDTO)
        {
            if (!orderCreateDTO.FoodInOrderId.Any() || orderCreateDTO.PaymentTypeId == default)
            {
                return BadRequest("default ids");
            }

            var returnOrder = await _bll.Orders.MakeAnOrderAsync(orderCreateDTO.FoodInOrderId, User.GetUserId()!.Value,
                orderCreateDTO.PaymentTypeId, orderCreateDTO.CreditCardId)!;
            if (returnOrder == null)
            {
                return BadRequest("Bad authentication");
                
            }

            await _bll.Bills.GenerateBillAndBillLineForOrder(returnOrder.Id, User.GetUserId()!.Value);
            
            return Ok(_mapper.Map(returnOrder));
        }

        /// <summary>
        /// API for the restaurant owner to confirm an order 
        /// </summary>
        /// <param name="orderConfirmRestaurantDTO">Order confirmation contract with a time till the food is ready</param>
        /// <returns>A fully completed Order object with all the order details</returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicDTO.Order), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PublicDTO.Order>> OrderConfirm(
            PublicDTO.OrderConfirmRestaurant orderConfirmRestaurantDTO)
        {
            var res = await _bll.Orders.ConfirmOrderByRestaurantAsync(orderConfirmRestaurantDTO.OrderId,
                orderConfirmRestaurantDTO.MinutesTillReady, User.GetUserId()!.Value)!;

            return Ok(_mapper.Map(res));
        }


        // GET: api/Order/5
        /// <summary>
        /// Gets a specific order
        /// </summary>
        /// <param name="id">Id of the order</param>
        /// <returns>An Order object</returns>
        [HttpGet("{id:guid}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicDTO.Order), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BllAppDTO.Order>> GetOrder(Guid id)
        {
            var order = await _bll.Orders.FirstOrDefaultAsync(id, User.GetUserId()!.Value);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // PUT: api/Order/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Updates an Order object according to the id 
        /// </summary>
        /// <param name="id">Id of the order to update</param>
        /// <param name="order">Updated version of the order</param>
        /// <returns>Bad request if update fails, NoContent if succeeds</returns>
        [HttpPut("{id:guid}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PutOrder(Guid id, PublicApiDTO.v1.v1.OrderModels.Order order)
        {
            if (id != order.Id)
            {
                return BadRequest("wrong id");
            }

            var getOrder = await _bll.Orders.FirstOrDefaultAsync(order.Id, User.GetUserId()!.Value);
            if (getOrder == null)
            {
                return BadRequest("Bad user id");
            }


            var orderBll = _mapper.Map(order)!;
            orderBll.AppUserId = getOrder!.AppUserId; // because owner is customer, editor is restaurant
            // var orderBll = _bll.Orders.Update(_mapper.Map(order)!);
            _bll.Orders.Update(orderBll);
            await _bll.SaveChangesTask();


            return NoContent();
        }


        // DELETE: api/Order/5
        /// <summary>
        /// Deletes an Order object according to the given id
        /// </summary>
        /// <param name="id">Guid id to delete with</param>
        /// <returns>NotFound 404 if id doesn't exist and NoContent if deletion was successful</returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var order = await _bll.Orders.FirstOrDefaultAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _bll.Orders.Remove(order);
            await _bll.SaveChangesTask();

            return NoContent();
        }
    }
}