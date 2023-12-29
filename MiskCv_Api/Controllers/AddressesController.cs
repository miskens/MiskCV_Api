using MapsterMapper;

namespace MiskCv_Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AddressesController : ControllerBase
{
    private readonly IAddressRepository _addressRepository;
    private readonly IMapper _mapper;

    public AddressesController(
            IAddressRepository addressRepository,
            IMapper mapper)
    {
        _addressRepository = addressRepository;
        _mapper = mapper;
    }

    #region GET

    // GET: api/Addresses
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AddressDto>>> GetAddress()
    {
        var addressModels = await _addressRepository.GetAddresses();

        if (addressModels == null)
        {
            return NotFound();
        }

        var addresses = _mapper.Map<List<AddressDto>>(addressModels);

        return Ok(addresses);
    }

    // GET: api/Addresses/5
    [HttpGet("{id}")]
    public async Task<ActionResult<AddressDto>> GetAddress(int id)
    {
        var addressModel = await _addressRepository.GetAddress(id);

        if (addressModel == null)
        {
            return NotFound();
        }

        var address = _mapper.Map<AddressDto>(addressModel);

        return address;
    }

    #endregion

    #region PUT

    // PUT: api/Addresses/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAddress(int id, AddressUpdateDto addressDto)
    {
        var address = _mapper.Map<Address>(addressDto);

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
    public async Task<ActionResult<Address>> PostAddress(AddressCreateDto addressDto)
    {
        var address = _mapper.Map<Address>(addressDto);

        var newAddress = await _addressRepository.CreateAddress(address);

        if (newAddress == null) { return Problem("There was a problem adding address"); }

        var createdAddress = _mapper.Map<AddressCreatedDto>(newAddress);

        return CreatedAtAction("GetAddress", new { id = createdAddress.Id }, createdAddress);
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
