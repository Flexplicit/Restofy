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
using PublicApiDTO.v1.v1.Mappers.MappingProfiles;
using PublicDTO = PublicApiDTO.v1.v1.OrderModels;


namespace WebApp.ApiControllers
{
    /// <summary>
    /// RESTful api service for BillLines
    /// /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class BillLineController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly BillLineMapper _mapper;

        /// <summary>
        /// Constructor for addresses
        /// </summary>
        /// <param name="bll">Injected bll</param>
        /// <param name="mapper">Injected mapper</param>
        public BillLineController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _mapper = new BillLineMapper(mapper);
        }

        // GET: api/BillLine
        /// <summary>
        /// Gets all the billLines that a bill has
        /// </summary>
        /// <returns>All the BillLines</returns>
        [HttpGet("{billId:guid}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicDTO.BillLine), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<PublicDTO.BillLine>>> GetBillBillLines(Guid billId)
        {
            return Ok((await _bll.BillLines.GetAllBillLinesAccordingToBillId(
                User.GetUserId()!.Value, billId)).Select(x => _mapper.Map(x)));
        }
    }
}