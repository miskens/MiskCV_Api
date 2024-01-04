using MapsterMapper;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.Extensions.Caching.Distributed;

namespace MiskCv_Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AddressesController : ControllerBase
{
    private readonly IAddressRepository _addressRepository;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _cache;

    public AddressesController(
            IAddressRepository addressRepository,
            IMapper mapper,
            IDistributedCache cache)
    {
        _addressRepository = addressRepository;
        _mapper = mapper;
        _cache = cache;
    }

    #region GET

    // GET: api/Addresses
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AddressDto>>> GetAddress()
    {
        IEnumerable<Address>? addressModels = null;

        var actionName = $"{nameof(GetAddress)}";
        var recordKey = $"{actionName}_AllAddresses";

        addressModels = await _cache.GetRecordAsync<List<Address>>(recordKey);

        if(addressModels == null)
        {
            addressModels = await _addressRepository.GetAddresses();

            if (addressModels != null)
            {
                await _cache.SetRecordAsync<IEnumerable<Address>>(recordKey, addressModels);
            }
        }

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
        Address? addressModel = null;

        var recordKey = $"Address_Id_{id}";

        addressModel = await _cache.GetRecordAsync<Address>(recordKey);

        if(addressModel == null)
        {
            addressModel = await _addressRepository.GetAddress(id);

            if (addressModel != null)
            {
                await _cache.SetRecordAsync<Address>(recordKey, addressModel);
            }
        }

        if (addressModel == null)
        {
            return NotFound();
        }

        var addressDto = _mapper.Map<AddressDto>(addressModel);

        return addressDto;
    }

    #endregion

    #region PUT

    // PUT: api/Addresses/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAddress(int id, AddressUpdateDto addressDto)
    {
        var addressModel = _mapper.Map<Address>(addressDto);

        if (id != addressModel.Id)
        {
            return BadRequest();
        }

        var result = await _addressRepository.UpdateAddress(id, addressModel);

        if (result == null)
        {
            return Problem("There was a problem updating address");
        }

        var recordKey = $"Address_Id_{id}";

        await _cache.SetRecordAsync<Address>(recordKey, addressModel);

        return NoContent();
    }

    #endregion

    #region POST

    // POST: api/Addresses
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Address>> PostAddress(AddressCreateDto addressDto)
    {
        var addressModel = _mapper.Map<Address>(addressDto);

        var newAddress = await _addressRepository.CreateAddress(addressModel);

        if (newAddress == null) 
        { 
            return Problem("There was a problem adding address"); 
        }

        var recordKey = $"Address_Id_{ newAddress.Id }";

        await _cache.SetRecordAsync<Address>(recordKey, newAddress);

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
