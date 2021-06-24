using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.BLL.App;
using Contracts.DAL.App;
using Microsoft.AspNetCore.Mvc;
using PublicApiDTO.v1.v1.Mappers;
using BllAppDTO = BLL.App.DTO.OrderModels;
using DalAppDTO = DAL.App.DTO.OrderModels;
using PublicDTO = PublicApiDTO.v1;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// RESTful api service for different Payment Types
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class PaymentTypeController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly PaymentTypeMapper _mapper;

        /// <summary>
        /// Constructor for Contacts
        /// </summary>
        /// <param name="bll">Injected bll</param>
        /// <param name="mapper">Mapper</param>
        public PaymentTypeController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _mapper = new PaymentTypeMapper(mapper);
        }

        // GET: api/PaymentType
        /// <summary>
        /// Gets all the available payment types
        /// </summary>
        /// <returns>A list of PaymentType objects</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PublicDTO.v1.OrderModels.PaymentType>>> GetPaymentTypes()
        {
            return Ok((await _bll.PaymentType.GetAllAsync()).Select(x => _mapper.Map(x)));
        }

        // GET: api/PaymentType/5
        /// <summary>
        /// Gets a specific PaymentType by the id
        /// </summary>
        /// <param name="id">Id of the PaymentType object</param>
        /// <returns>A PaymentType object</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PublicDTO.v1.OrderModels.PaymentType>> GetPaymentType(Guid id)
        {
            var paymentType = await _bll.PaymentType.FirstOrDefaultAsync(id);

            if (paymentType == null)
            {
                return NotFound();
            }

            return _mapper.Map(paymentType)!;
        }
    }
}