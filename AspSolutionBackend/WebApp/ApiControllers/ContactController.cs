using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.BLL.App;
using Extensions.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicApiDTO.v1.v1.Mappers;
using PublicDTO = PublicApiDTO.v1.v1.OrderModels;


namespace WebApp.ApiControllers
{
    /// <summary>
    /// RESTful api service for Contacts
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ContactController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly ContactMapper _mapper;

        /// <summary>
        /// Constructor for Contacts
        /// </summary>
        /// <param name="bll">Injected bll</param>
        /// <param name="mapper">Injected mapper</param>
        public ContactController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _mapper = new ContactMapper(mapper);
        }

        // GET: api/Contact
        /// <summary>
        /// Gets all restaurantContacts if it has the id, else it gets all user contacts
        /// </summary>
        /// <returns>All the Contact objects</returns>
        [HttpGet]
        [HttpGet("{restaurantId:guid}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PublicDTO.ContactView>>> GetContacts(Guid? restaurantId)
        {
            if (restaurantId == null)
            {
                return Ok((await _bll.Contacts.GetAllAsync(User.GetUserId()!.Value, default)).Select(ContactMapper
                    .MapBllToContactView!));
            }

            return Ok((await _bll.Contacts.GetAllAsync(default, restaurantId!.Value)).Select(ContactMapper
                .MapBllToContactView!));
        }

        // // GET: api/Contact/5
        // /// <summary>
        // /// Gets a singular ContactDTO according to guid
        // /// </summary>
        // /// <param name="id">Guid id value of the Contact</param>
        // /// <returns>A singular Contact object</returns>
        // [HttpGet("{id:guid}")]
        // public async Task<ActionResult<BllAppDTO.Contact>> GetContact(Guid id)
        // {
        //     var contact = await _bll.Contacts.FirstOrDefaultAsync(id);
        //
        //     if (contact == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     return contact;
        // }

        // PUT: api/Contact/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Updates a Contact according to the id
        /// </summary>
        /// <param name="id">Id of the Contact to update</param>
        /// <param name="contact">Updated version of the Contact</param>
        /// <returns>Bad request if update fails, NoContent if succeeds</returns>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> PutContact(Guid id, PublicDTO.Contact contact)
        {
            if (id != contact.Id || !ModelState.IsValid)
            {
                return BadRequest();
            }

            var restaurant =
                await _bll.Restaurants.FirstOrDefaultAsync(contact.RestaurantId!.Value, User.GetUserId()!.Value);
            if (restaurant == null || !restaurant.AppUserId.Equals(User.GetUserId()!.Value))
            {
                var contactBll = await _bll.Contacts.FirstOrDefaultAsync(contact.Id, User.GetUserId()!.Value);
                if (contactBll == null)
                {
                    return BadRequest("Bad User data access");
                }

                var newBll = _bll.Contacts.Update(_mapper.Map(contact)!);
                newBll.AppUserId = User.GetUserId()!.Value;
            }
            else
            {
                _bll.Contacts.Update(_mapper.Map(contact)!);
            }

            await _bll.SaveChangesTask();
            return NoContent();
        }

        // POST: api/Contact
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Creates a Contact object
        /// </summary>
        /// <param name="contact">Contact object to create</param>
        /// <returns>Created Contact object with a Guid id attached to it</returns>
        [HttpPost]
        public async Task<ActionResult<PublicDTO.Contact>> PostContact(PublicDTO.ContactCreate contact)
        {
            var contactBll = ContactMapper.MapContactPublicToBll(contact);
            if (contact.RestaurantId == null || contact.RestaurantId.Equals(default))
            {
                contactBll!.AppUserId = User.GetUserId()!.Value;
            }

            var restaurant =
                await _bll.Restaurants.FirstOrDefaultAsync(contact.RestaurantId!.Value);
            if (restaurant == null || !restaurant!.AppUserId.Equals(User.GetUserId()!.Value))
            {
                return BadRequest();
            }

            var contactPublic = _mapper.Map(_bll.Contacts.Add(contactBll!));
            await _bll.SaveChangesTask();

            return CreatedAtAction("GetContacts", new {id = contactPublic!.Id}, contactPublic);
        }

        // DELETE: api/Contact/5
        /// <summary>
        /// Deletes a contact object according to the given id
        /// </summary>
        /// <param name="id">Guid id to delete with</param>
        /// <returns>NotFound 404 if id doesn't exist and NoContent if deletion was successful</returns>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteContact(Guid id)
        {
            var contact = await _bll.Contacts.FirstOrDefaultAsync(id, User.GetUserId()!.Value);
            if (contact == null)
            {
                return NotFound();
            }


            _bll.Contacts.Remove(contact);
            await _bll.SaveChangesTask();

            return NoContent();
        }
    }
}