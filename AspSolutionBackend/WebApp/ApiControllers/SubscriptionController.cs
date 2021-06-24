using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.BLL.App;
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
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SubscriptionController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly IMapper _mapper;
        private readonly SubscriptionMapper _subscriptionMapper;

        /// <summary>
        /// Constructor for Subscription APIs
        /// </summary>
        /// <param name="bll">Injected bll</param>
        /// <param name="mapper">Injected Mapper</param>
        public SubscriptionController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _mapper = mapper;
            _subscriptionMapper = new SubscriptionMapper(mapper);
        }

        // GET: api/Subscription
        /// <summary>
        /// Retrieves all available subscriptions from the database
        /// </summary>
        /// <returns>Returns all available subscriptions</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PublicDTO.Subscription>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<PublicDTO.Subscription>>> GetSubscriptions()
        {
            return Ok((await _bll.Subscriptions.GetAllAsync()).Select(x => _subscriptionMapper.Map(x)));
        }

        // GET: api/Subscription/5
        /// <summary>
        /// Returns singular Subscription with all the data it has
        /// </summary>
        /// <param name="id">Id of the subscription to retrieve</param>
        /// <returns>PublicApiDTO Object of the subscription</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(PublicDTO.Subscription), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PublicDTO.Subscription>> GetSubscription(Guid id)
        {
            var subscription = await _bll.Subscriptions.FirstOrDefaultAsync(id);

            if (subscription == null)
            {
                return NotFound();
            }
            return Ok(_subscriptionMapper.Map(subscription)!);
        }
    }
}