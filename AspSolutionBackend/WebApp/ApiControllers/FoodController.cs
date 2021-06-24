using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.BLL.App;
using Extensions.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PublicApiDTO.v1.v1.Mappers;
using PublicApiDTO.v1.v1.OrderModels;
using WebApp.ExternalApis;
using WebApp.SpecialExternalApiHelpers;
using BllAppDTO = BLL.App.DTO.OrderModels;
using DalAppDTO = DAL.App.DTO.OrderModels;
using PublicDTO = PublicApiDTO.v1;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// Represents a RESTful food service
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly IMapper Mapper;
        private readonly FoodMapper _mapper;


        /// <summary>
        /// Constructor for Food API
        /// </summary>
        /// <param name="bll">Injected bll via DI</param>
        /// <param name="mapper">Injected Mapper via DI</param>
        public FoodController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            Mapper = mapper;
            _mapper = new FoodMapper(Mapper);
        }

        // GET: api/v1/Food/GetFoods/8
        /// <summary>
        /// Returns menu of food which is ordered ordered according to the food's food group from a specific restaurant
        /// </summary>
        /// <param name="restaurantId">Restaurant's id from where we're requesting the food</param>
        /// <returns>List of food objects that belong to the corresponding restaurant</returns>
        [HttpGet("{restaurantId:guid}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicDTO.v1.OrderModels.Food), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Food>>> GetFoods(Guid restaurantId)
        {
            var requestRes = (await _bll.Food.GetRestaurantsFoodAsync(restaurantId)).ToList();

            if (!requestRes.Any()) return BadRequest("Id Doesn't exist");

            return Ok(requestRes.Select(FoodMapper.MapFoodFromBllToApi));
        }

        // GET: api/Food/5
        /// <summary>
        /// Gets a specific food object according to the given id
        /// </summary>
        /// <param name="id">Id of the food to return</param>
        /// <returns>Food object</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicDTO.v1.OrderModels.Food), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PublicDTO.v1.OrderModels.Food>> GetFood(Guid id)
        {
            var food = await _bll.Food.FirstOrDefaultAsync(id);
            if (food == null)
            {
                return BadRequest("No food");
            }

            var foodDTO = _mapper.Map(food);
            foodDTO!.CostWithVat = food.Cost!.CostWithVat;
            return foodDTO!;
        }

        // PUT: api/Food/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        ///  Updates the food item in the db
        /// </summary>
        /// <param name="id">id of the food to update</param>
        /// <param name="foodDTO">Updated version of the food</param>
        /// <returns>Nothing</returns>
        [HttpPut("{id:guid}")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(PublicDTO.v1.OrderModels.Food), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update(Guid id, PublicDTO.v1.OrderModels.Food foodDTO)
        {
            if (id != foodDTO.Id)
            {
                return BadRequest("Bad object");
            }

            var imageService = new GyazoApiService();
            var imagePost = await imageService.UploadImageViaApi(foodDTO.Picture!);
            if (!imagePost.IsSuccessfulResponse) return BadRequest(imagePost.Message);
            foodDTO.Picture = imagePost.Url;
            // var costBll = await _bll.Cost.FirstOrDefaultAsync(foodDTO.CostId!.Value);
            // costBll!.CostWithoutVat = foodDTO.CostWithVat;
            var bllDTO = FoodMapper.MapFoodEditPublicToBll(foodDTO);
            var costBll = _bll.Cost.GenerateBllCostFromDecimal(foodDTO.CostWithVat);

            bllDTO.Cost = costBll;
            _bll.Food.Update(bllDTO!);
            await _bll.SaveChangesTask();
            return NoContent();
        }

        // POST: api/Food
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Creates a new food object
        /// </summary>
        /// <param name="foodCreateDTO">Food object to create</param>
        /// <returns>Newly created food object</returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicDTO.v1.OrderModels.Food), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PublicDTO.v1.OrderModels.Food>> Create(
            PublicDTO.v1.OrderModels.FoodCreate foodCreateDTO)
        {
            var imageService = new GyazoApiService();
            var imagePost = await imageService.UploadImageViaApi(foodCreateDTO.Picture!);
            if (!imagePost.IsSuccessfulResponse) return BadRequest(imagePost.Message);
            foodCreateDTO.Picture = imagePost.Url;


            var costBll = _bll.Cost.GenerateBllCostFromDecimal(foodCreateDTO.CostWithVat);
            var bllFood = FoodMapper.MapFoodCreatePublicToBll(foodCreateDTO);
            bllFood.Cost = costBll;
            var result = _bll.Food.Add(bllFood!);
            await _bll.SaveChangesTask();
            var returnFoodDTO = _mapper.Map(result);
            return CreatedAtAction("Create", new {id = returnFoodDTO!.Id}, returnFoodDTO);
        }

        // DELETE: api/v1/Food/Delete/5
        /// <summary>
        /// Deletes a food object according to the given id
        /// </summary>
        /// <param name="id">Id of the food to delete</param>
        /// <returns>Nothing</returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var food = await _bll.Food.FirstOrDefaultAsync(id, User.GetUserId()!.Value, true);
            if (food == null)
            {
                return NotFound();
            }
            
            await _bll.Food.Remove(food);
            await _bll.SaveChangesTask();

            return NoContent();
        }
    }
}