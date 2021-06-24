using System;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.BLL.App;
using Extensions.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PublicApiDTO.v1.v1.Mappers;
using PublicDTO = PublicApiDTO.v1.v1.OrderModels;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// RESTful api service for RestaurantSubscriptions
    /// </summary>
    ///
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class RestaurantSubscriptionController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly RestaurantSubscriptionMapper _mapper;

        /// <summary>
        /// Constructor for RestaurantSubscription Dependencies
        /// </summary>
        /// <param name="bll">Injected bll</param>
        /// <param name="mapper">Injected Mapper</param>
        public RestaurantSubscriptionController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _mapper = new RestaurantSubscriptionMapper(mapper);
        }

        // GET: api/RestaurantSubscription/5
        /// <summary>
        /// Returns singular RestaurantSubscription, which shows how long subscription lasts.
        /// </summary>
        /// <param name="restaurantId">Id of the restaurant to get the subscription for</param>
        /// <returns>PublicApiDTO Object of the subscription</returns>
        [HttpGet("{restaurantId:guid}")]
        [ProducesResponseType(typeof(PublicDTO.RestaurantSubscription), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PublicDTO.RestaurantSubscription>> GetRestaurantSubscriptions(Guid restaurantId)
        {
            var subscription = await _bll.RestaurantSubscriptions.FirstOrDefaultAsync(restaurantId);

            if (subscription == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map(subscription));
        }

        /// <summary>
        /// Adds a restaurantSubscription to the restauarant in the given object
        /// </summary>
        /// <param name="restaurantSubscription">JSON object of the RestaurantSubscription to add</param>
        /// <returns>JSON created object</returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicDTO.RestaurantSubscription), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PublicDTO.RestaurantSubscription>> PostRestaurantSubscription(
            [FromBody] PublicDTO.RestaurantSubscriptionCreate restaurantSubscription)
        {
            var bllRestaurant = await _bll.Restaurants.FirstOrDefaultAsync(
                restaurantSubscription.RestaurantId, User.GetUserId()!.Value, true);
            if (bllRestaurant == null || bllRestaurant.AppUserId != User.GetUserId()!.Value)
            {
                return BadRequest("Cannot change other people's data");
            }

            var bllSubscription = _bll.RestaurantSubscriptions.Add(RestaurantSubscriptionMapper
                .ApiToBllRestaurantSubscriptionCreate(restaurantSubscription));
            
            await _bll.SaveChangesTask();
            return Created("", _mapper.Map(bllSubscription));
        }
    }
}