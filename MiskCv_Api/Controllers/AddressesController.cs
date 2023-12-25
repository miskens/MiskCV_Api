using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiskCv_Api.Data;
using MiskCv_Api.Dtos;
using MiskCv_Api.Models;
using MiskCv_Api.Services.Repositories.AddressesRepository;

namespace MiskCv_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly MiskCvDbContext _context;
        private readonly IAddressRepository _addressRepository;

        public AddressesController(MiskCvDbContext context, IAddressRepository addressRepository)
        {
            _context = context;
            _addressRepository = addressRepository;
        }

        #region GET

        // GET: api/Addresses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Address>>> GetAddress()
        {
            var addresses = await _addressRepository.GetAddresses();

            if (addresses == null)
            {
                return NotFound();
            }

            return Ok(addresses);
        }

        // GET: api/Addresses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Address>> GetAddress(int id)
        {
            var address = await _addressRepository.GetAddress(id);

            if (address == null)
            {
                return NotFound();
            }

            return address;
        }

        #endregion

        #region PUT

        // PUT: api/Addresses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAddress(int id, Address address)
        {
            if (id != address.Id)
            {
                return BadRequest();
            }

            await _addressRepository.UpdateAddress(id, address);

            return NoContent();
        }

        #endregion

        #region POST

        // POST: api/Addresses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Address>> PostAddress(Address address)
        {
            var newAddress = await _addressRepository.CreateAddress(address);

            if (newAddress == null) { return Problem("There was a problem adding address"); }

            return CreatedAtAction("GetAddress", new { id = newAddress.Id }, newAddress);
        }

        #endregion

        #region DELETE

        // DELETE: api/Addresses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            if (_context.Address == null)
            {
                return NotFound();
            }
            var address = await _context.Address.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }

            _context.Address.Remove(address);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        #endregion

        #region HELPERS

        private bool AddressExists(int id)
        {
            return (_context.Address?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        #endregion
    }
}
