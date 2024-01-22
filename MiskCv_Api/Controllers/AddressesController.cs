namespace MiskCv_Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AddressesController : ControllerBase
{
    private readonly IAddressRepository _addressRepository;
    private readonly IMapper _mapper;
    private readonly IDistributedCachingService _cache;

    public AddressesController(
            IAddressRepository addressRepository,
            IMapper mapper,
            IDistributedCachingService cache)
    {
        _addressRepository = addressRepository;
        _mapper = mapper;
        _cache = cache;
    }

    #region GET

    // GET: api/Addresses
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AddressDto>>> GetAddress(CancellationToken cancellationToken)
    {
        try
        {
            IEnumerable<Address>? addressModels = null;

            var actionName = $"{nameof(GetAddress)}";
            var recordKey = $"{actionName}_AllAddresses";

            addressModels = await _cache.GetRecordAsync<List<Address>>(recordKey, cancellationToken); //CANCELLATION DONE

            if (addressModels == null)
            {
                addressModels = await _addressRepository.GetAddresses();

                if (addressModels != null)
                {
                    await _cache.SetRecordAsync<IEnumerable<Address>>(recordKey, addressModels, cancellationToken);
                }
            }

            if (addressModels == null)
            {
                return NotFound();
            }

            var addresses = _mapper.Map<List<AddressDto>>(addressModels);

            return Ok(addresses);
        }
        catch (OperationCanceledException)
        {
            return StatusCode(499, "Request canceled");
        }

    }

    // GET: api/Addresses/5
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<AddressDto>> GetAddress(int id, CancellationToken cancellationToken)
    {
        Address? addressModel = null;

        var recordKey = $"Address_Id_{id}";

        addressModel = await _cache.GetRecordAsync<Address>(recordKey, cancellationToken);

        if(addressModel == null)
        {
            addressModel = await _addressRepository.GetAddress(id);

            if (addressModel != null)
            {
                await _cache.SetRecordAsync<Address>(recordKey, addressModel, cancellationToken);
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
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> PutAddress([FromBody] AddressUpdateDto addressDto, int id, CancellationToken cancellationToken)
    {
        var addressModel = _mapper.Map<Address>(addressDto);

        if (id != addressModel.Id)
        {
            return BadRequest();
        }

        var result = await _addressRepository.UpdateAddress(id, addressModel, cancellationToken);

        if (result == null)
        {
            return Problem("There was a problem updating address");
        }

        var recordKey = $"Address_Id_{id}";

        await _cache.SetRecordAsync<Address>(recordKey, addressModel, cancellationToken);

        return NoContent();
    }

    #endregion

    #region POST

    // POST: api/Addresses
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Address>> PostAddress([FromBody] AddressCreateDto addressDto, CancellationToken cancellationToken)
    {
        var addressModel = _mapper.Map<Address>(addressDto);

        addressModel = await _addressRepository.CreateAddress(addressModel, cancellationToken);

        if (addressModel == null) 
        { 
            return Problem("There was a problem adding address"); 
        }

        try
        {
            var createdAddress = _mapper.Map<AddressCreatedDto>(addressModel);
            var recordKey = $"Address_Id_{addressModel.Id}";
            await _cache.SetRecordAsync<Address>(recordKey, addressModel, cancellationToken);

            return CreatedAtAction("GetAddress", new { id = createdAddress.Id }, createdAddress);
        }
        catch (Exception ex)
        {
            Console.WriteLine("There was a problem creating skill", ex.Message);

            return Problem(ex.Message);
        }
    }

    #endregion

    #region DELETE

    // DELETE: api/Addresses/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteAddress(int id, CancellationToken cancellationToken)
    {
        var result = await _addressRepository.DeleteAddress(id, cancellationToken);

        if(result == false) { return NotFound(); }

        return NoContent();
    }

    #endregion
}
