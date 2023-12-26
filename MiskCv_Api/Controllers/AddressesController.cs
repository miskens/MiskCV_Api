namespace MiskCv_Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AddressesController : ControllerBase
{
    private readonly IAddressRepository _addressRepository;

    public AddressesController(IAddressRepository addressRepository)
    {
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

        var result = await _addressRepository.UpdateAddress(id, address);

        if (result == null)
        {
            return Problem("There was a problem updating address");
        }

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
        var result = await _addressRepository.DeleteAddress(id);

        if(result == false) { return NotFound(); }

        return NoContent();
    }

    #endregion
}
