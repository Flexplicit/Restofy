using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.BLL.App;
using Contracts.DAL.App;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicApiDTO.v1.v1.Mappers;
using BllAppDTO = BLL.App.DTO.OrderModels;
using DalAppDTO = DAL.App.DTO.OrderModels;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// RESTful api service for FoodGroups
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class FoodGroupController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly FoodGroupMapper _mapper;

        /// <summary>
        /// Constructor for FoodGroup API
        /// </summary>
        /// <param name="bll">Injected bll</param>
        /// <param name="mapper">Mapper</param>
        public FoodGroupController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _mapper = new FoodGroupMapper(mapper);
        }

        // GET: api/FoodGroup
        /// <summary>
        /// Gets all the available FoodGroups
        /// </summary>
        /// <returns>All the available FoodGroupDTOs</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PublicApiDTO.v1.v1.OrderModels.FoodGroup>>> GetFoodGroups()
        {
            return Ok((await _bll.FoodGroup.GetAllAsync()).Select(x => _mapper.Map(x)));
        }

        // GET: api/FoodGroup/5
        /// <summary>
        /// Gets a specific FoodGroup object
        /// </summary>
        /// <param name="id">Guid id value of the FoodGroup</param>
        /// <returns>A singular FoodGroup object</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PublicApiDTO.v1.v1.OrderModels.FoodGroup>> GetFoodGroup(Guid id)
        {
            var foodGroup = await _bll.FoodGroup.FirstOrDefaultAsync(id);

            if (foodGroup == null)
            {
                return NotFound();
            }

            return _mapper.Map(foodGroup)!;
        }
    }
}