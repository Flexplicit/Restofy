using System;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Mvc;
using Extensions.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using PublicApiDTO.v1.v1.Mappers;
using PublicDTO = PublicApiDTO.v1;


namespace WebApp.ApiControllers
{
    /// <summary>
    /// RESTful api service for Bills
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly BillMapper _mapper;

        /// <summary>
        /// Constructor for addresses
        /// </summary>
        /// <param name="bll">Injected bll</param>
        /// <param name="mapper">Injected mapper</param>
        public BillController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _mapper = new BillMapper(mapper);
        }


        // GET: api/Bill/5
        /// <summary>
        /// Gets a singular billDTO according to order id
        /// </summary>
        /// <param name="orderId">orderId to get the bill from db</param>
        /// <returns>A singular bill object</returns>
        [HttpGet("{orderId:guid}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicDTO.v1.OrderModels.Bill), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PublicDTO.v1.OrderModels.Bill>> GetBillByOrderId(Guid orderId)
        {
            var bill = await _bll.Bills.GetBillAccordingToOrderId(orderId, User.GetUserId()!.Value);

            if (bill == null)
            {
                return NotFound();
            }

            return _mapper.Map(bill)!;
        }
    }
}