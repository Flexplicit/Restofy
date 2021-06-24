using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.APP.EF;
using PublicApiDTO.v1.v1.Mappers;
using BllAppDTO = BLL.App.DTO.OrderModels;
using DalAppDTO = DAL.App.DTO.OrderModels;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// RESTful api service for Food cost
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/")]
    [ApiController]
    public class CostController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly CostMapper _mapper;

        /// <summary>
        /// Constructor for addresses
        /// </summary>
        /// <param name="bll">Injected BLL</param>
        /// <param name="mapper">Injected Mapper</param>
        public CostController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _mapper = new CostMapper(mapper);
        }

        // GET: api/Cost/5
        /// <summary>
        ///  Gets a specific cost according to the foodId
        /// </summary>
        /// <param name="foodId">foodId</param>
        /// <returns>A cost dto object</returns>
        [HttpGet("{foodId:guid}")]
        public async Task<ActionResult<PublicApiDTO.v1.v1.OrderModels.Cost>> GetCostByFoodId(Guid foodId)
        {
            var cost = await _bll.Cost.FirstOrDefaultAsync(foodId);

            if (cost == null)
            {
                return NotFound();
            }

            return _mapper.Map(cost)!;
        }
    }
}