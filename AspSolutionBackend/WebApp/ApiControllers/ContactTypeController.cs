using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.BLL.App;
using Contracts.DAL.App;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PublicApiDTO.v1.v1.Mappers;
using BllAppDTO = BLL.App.DTO.OrderModels;
using DalAppDTO = DAL.App.DTO.OrderModels;


namespace WebApp.ApiControllers
{
    /// <summary>
    /// RESTful api service for Bills
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ContactTypeController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly ContactTypeMapper _mapper;

        /// <summary>
        /// Constructor for addresses
        /// </summary>
        /// <param name="bll">Injected bll</param>
        /// <param name="mapper">Injected mapper</param>
        public ContactTypeController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _mapper = new ContactTypeMapper(mapper);
        }

        // GET: api/ContactType
        /// <summary>
        /// Gets all the ContactTypes
        /// </summary>
        /// <returns>All the ContactType objects</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PublicApiDTO.v1.v1.OrderModels.ContactType>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PublicApiDTO.v1.v1.OrderModels.ContactType>>> GetContactTypes()
        {
            return Ok((await _bll.ContactType.GetAllAsync()).Select(x => _mapper.Map(x)));
        }
    }
}