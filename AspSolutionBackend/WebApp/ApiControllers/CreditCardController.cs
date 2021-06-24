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
using PublicApiDTO.v1.v1.Mappers;
using BllAppDTO = BLL.App.DTO.OrderModels;
using PublicDTO = PublicApiDTO.v1.v1.OrderModels;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// RESTful api service for CreditCards
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class CreditCardController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly CreditCardMapper _mapper;

        /// <summary>
        /// Constructor for addresses
        /// </summary>
        /// <param name="bll">Injected bll</param>
        /// <param name="mapper">Injected Mapper</param>
        public CreditCardController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _mapper = new CreditCardMapper(mapper);
        }

        // GET: api/CreditCard
        /// <summary>
        /// Gets all the Credit cards
        /// </summary>
        /// <returns>All the cards</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(IEnumerable<PublicDTO.Order>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PublicDTO.CreditCard>>> GetCreditCards()
        {
            return Ok((await _bll.CreditCards.GetAllAsync(User.GetUserId()!.Value))
                .Select(CreditCardMapper.MapBllToPublicApiCreditCard));
        }

        // GET: api/CreditCard/5
        /// <summary>
        /// Gets a specific credit card
        /// </summary>
        /// <param name="id">Id of the credit card</param>
        /// <returns>A CreditCard DTO object</returns>
        [HttpGet("{id:guid}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicDTO.Order), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PublicApiDTO.v1.v1.OrderModels.CreditCard>> GetCreditCard(Guid id)
        {
            var creditCard = await _bll.CreditCards.FirstOrDefaultAsync(id, User.GetUserId()!.Value);
            if (creditCard == null)
            {
                return NotFound();
            }

            return CreditCardMapper.MapBllToPublicApiCreditCard(creditCard)!;
        }

        // PUT: api/CreditCard/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Updates a CreditCard object
        /// </summary>
        /// <param name="id">Id of the CreditCard object to update</param>
        /// <param name="creditCard">CreditCard object with the updated values</param>
        /// <returns>Bad request if update fails, NoContent if succeeds</returns>
        [HttpPut("{id:guid}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PutCreditCard(Guid id, PublicApiDTO.v1.v1.OrderModels.CreditCard creditCard)
        {
            if (id != creditCard.Id)
            {
                return BadRequest();
            }

            var bllCreditCard = CreditCardMapper.MapPublicToBllCreditCard(creditCard);
            var checkerCard = await _bll.CreditCards.FirstOrDefaultAsync(creditCard.Id, User.GetUserId()!.Value);
            if (checkerCard == null)
            {
                return BadRequest();
            }

            bllCreditCard.CreditCardInfo!.Id = checkerCard!.CreditCardInfo!.Id;
            bllCreditCard.CreditCardInfoId = checkerCard!.CreditCardInfo!.Id;
            bllCreditCard.AppUserId = User.GetUserId()!.Value;
            _bll.CreditCards.Update(bllCreditCard);
            await _bll.SaveChangesTask();
            return NoContent();
        }

        // POST: api/CreditCard
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Creates a CreditCard object which is linked to the user
        /// </summary>
        /// <param name="creditCard">Object to create</param>
        /// <returns>A created CreditCardDTO object</returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicDTO.Order), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PublicApiDTO.v1.v1.OrderModels.CreditCard>> PostCreditCard(
            PublicApiDTO.v1.v1.OrderModels.CreditCardCreate creditCard)
        {
            var bllResult = CreditCardMapper.MapPublicToBllCreditCardCreate(creditCard);
            CreditCardMapper.MapPublicToBllCreditCardCreate(creditCard);
            bllResult.AppUserId = User.GetUserId()!.Value;
            var result = _bll.CreditCards.Add(bllResult);
            var returnCard = CreditCardMapper.MapBllToPublicApiCreditCard(result);
            await _bll.SaveChangesTask();
            return CreatedAtAction("GetCreditCard", new {id = returnCard!.Id}, returnCard);
        }

        // DELETE: api/CreditCard/5
        /// <summary>
        /// Deletes a CreditCard according to the id
        /// </summary>
        /// <param name="id">Guid id to delete with</param>
        /// <returns>NotFound 404 if id doesn't exist and NoContent if deletion was successful</returns>
        [HttpDelete("{id:guid}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicDTO.Order), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCreditCard(Guid id)
        {
            var creditCard = await _bll.CreditCards.FirstOrDefaultAsync(id, User.GetUserId()!.Value);
            if (creditCard == null)
            {
                return NotFound();
            }

            _bll.CreditCards.Remove(creditCard);
            await _bll.SaveChangesTask();

            return NoContent();
        }
    }
}