using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.BLL.App;
using Extensions.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using PublicApiDTO.v1.v1.Mappers;
using WebApp.ExternalApis;
using BllAppDTO = BLL.App.DTO.OrderModels;
using PublicDTO = PublicApiDTO.v1;
using WebApp.SpecialExternalApiHelpers;


namespace WebApp.ApiControllers
{
    /// <summary>
    ///  Represents a RESTful Restaurant service
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RestaurantController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly RestaurantMapper _mapper;

        /// <summary>
        /// Constructor for Restaurant API
        /// </summary>
        /// <param name="bll">Injected bll DI</param>
        /// <param name="mapper">Injected mapper via DI</param>
        public RestaurantController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _mapper = new RestaurantMapper(mapper);
        }

        // GET: api/Restaurant
        /// <summary>
        /// Returns all restaurants that have an active subscription.
        /// </summary>
        /// <returns>Returns a list of restaurant objects</returns>
        [HttpGet]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PublicDTO.v1.OrderModels.Restaurant>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PublicDTO.v1.OrderModels.Restaurant>>> GetRestaurants()
        {
            var restaurants = (await _bll.Restaurants.GetAllAsync()).Select(x => _mapper.Map(x));
            return Ok(restaurants);
        }

        /// <summary>
        /// Gets all restaurants that belong to the user that requests them, will be authenticated via JWT.
        /// </summary>
        /// <returns>Returns a list of restaurant objects</returns>
        [HttpGet]
        [Route("MyRestaurants")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PublicDTO.v1.OrderModels.Restaurant>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<PublicDTO.v1.OrderModels.Restaurant>>> GetMyRestaurants()
        {
            var restaurants = (await _bll.Restaurants.GetMyRestaurantsAsync(User.GetUserId()!.Value))
                .Select(x => _mapper.Map(x));
            return Ok(restaurants);
        }


        // GET: api/Restaurant/5
        /// <summary>
        ///  Gets a specific restaurant by the id
        /// </summary>
        /// <param name="id">Id of the requested restaurant</param>
        /// <returns>An object of the restaurant</returns>
        [HttpGet("{id:guid}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicDTO.v1.OrderModels.Restaurant), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PublicDTO.v1.OrderModels.Restaurant>> GetRestaurant(Guid id)
        {
            var restaurant = await _bll.Restaurants.FirstOrDefaultAsync(id);
            if (restaurant == null)
            {
                return NotFound("Restaurant not found, help");
            }

            return Ok(_mapper.Map(restaurant));
        }

        // PUT: api/Restaurant/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Updates a specific restaurant
        /// </summary>
        /// <param name="id">Id of the restaurant</param>
        /// <param name="restaurant">Updated object version of the restaurant</param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PutRestaurant(Guid id, PublicDTO.v1.OrderModels.RestaurantEdit restaurant)
        {
            if (id != restaurant.Id)
            {
                return BadRequest("wrong id");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("Non nullable fields are empty");
            }

            var imageService = new GyazoApiService();
            var imagePost = await imageService.UploadImageViaApi(restaurant.Picture!);
            if (!imagePost.IsSuccessfulResponse)
            {
                return BadRequest(imagePost.Message);
            }

            restaurant.Picture = imagePost.Url;


            var bllRestaurantDTO = RestaurantMapper.ApiEditRestaurantToBll(restaurant);
            bllRestaurantDTO!.AppUserId = User.GetUserId()!.Value;
            _bll.Restaurants.Update(bllRestaurantDTO!);
            await _bll.SaveChangesTask();
            return NoContent();
        }

        // POST: api/Restaurant
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Creates a new restaurant
        /// </summary>
        /// <param name="restaurant">Object of the new restaurant</param>
        /// <returns>An object of the newly created restaurant</returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicDTO.v1.OrderModels.Restaurant), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<PublicDTO.v1.OrderModels.Restaurant>> PostRestaurant(
            [FromBody] PublicDTO.v1.OrderModels.RestaurantCreate restaurant)
        {
            var imageService = new GyazoApiService();
            var imagePost = await imageService.UploadImageViaApi(restaurant.Picture!);
            if (!imagePost.IsSuccessfulResponse)
            {
                return BadRequest(imagePost.Message);
            }
            restaurant.Picture = imagePost.Url;
            

            var bllRestaurant = RestaurantMapper.ApiCreateRestaurantToBll(restaurant);
            bllRestaurant!.AppUserId = User.GetUserId()!.Value;
            var result = _bll.Restaurants.Add(bllRestaurant!);
            await _bll.SaveChangesTask();

            var publicRestaurant = _mapper.Map(result);
            return CreatedAtAction("GetRestaurant", new {id = publicRestaurant!.Id}, publicRestaurant);
        }

        // DELETE: api/Restaurant/5
        /// <summary>
        /// Deletes a restaurant, only restaurant owner can delete it
        /// </summary>
        /// <param name="id">Id of the restaurant to delete</param>
        /// <returns>Nothing</returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteRestaurant(Guid id)
        {
            var restaurant = await _bll.Restaurants.FirstOrDefaultAsync(id, User.GetUserId()!.Value, true);
            if (restaurant == null)
            {
                return NotFound();
            }
            
            await _bll.Restaurants.Remove(restaurant, User.GetUserId()!.Value);
            await _bll.SaveChangesTask();

            return NoContent();
        }
    }
}