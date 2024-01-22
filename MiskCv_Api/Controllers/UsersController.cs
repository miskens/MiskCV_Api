namespace MiskCv_Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IDistributedCachingService _cache;

    public UsersController(
            IUserRepository userRepository,
            IMapper mapper,
            IDistributedCachingService cache)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _cache = cache;
    }

    #region GET

    // GET: api/Users
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUser(CancellationToken cancellationToken)
    {
        IEnumerable<User>? userModels = null;

        var actionName = $"{nameof(GetUser)}";
        string recordKey = $"{actionName}_AllUsers";

        userModels = await _cache.GetRecordAsync<List<User>>(recordKey, cancellationToken);

        if (userModels == null)
        {
            userModels = await _userRepository.GetUsers(cancellationToken);

            if (userModels != null) 
            {
                await _cache.SetRecordAsync<IEnumerable<User>>(recordKey, userModels, cancellationToken);
            }
        }        

        if (userModels == null)
        {
            return NotFound();
        }

        var users = _mapper.Map<List<UserDto>>(userModels);

        return Ok(users);
    }

    // GET: api/Users/5
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<UserDto>> GetUser(int id, CancellationToken cancellationToken)
    {
        User? userModel = null;

        var recordKey = $"User_Id_{ id }";

        userModel = await _cache.GetRecordAsync<User>(recordKey, cancellationToken);

        if(userModel == null)
        {
            userModel = await _userRepository.GetUser(id, cancellationToken);

            if(userModel != null)
            {
                await _cache.SetRecordAsync<User>(recordKey, userModel, cancellationToken);
            }
        }

        if (userModel == null)
        {
            return NotFound();
        }

        var user = _mapper.Map<UserDto>(userModel);

        return user;
    }

    #endregion

    #region PUT

    // PUT: api/Users/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> PutUser([FromBody] UserUpdateDto userDto, int id, CancellationToken cancellationToken)
    {
        var userModel = _mapper.Map<User>(userDto);
        
        if (id != userModel.Id)
        {
            return BadRequest();
        }

        var result = await _userRepository.UpdateUser(id, userModel, cancellationToken);

        if (result == null)
        {
            return Problem("There was a problem updating user");
        }

        var recordKey = $"User_Id_{id}";
        await _cache.SetRecordAsync<User>(recordKey, userModel, cancellationToken);

        return NoContent();
    }

    #endregion

    #region POST

    // POST: api/Users
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserCreatedDto>?> PostUser([FromBody] UserCreateDto userDto, CancellationToken cancellationToken)
    {
        var userModel = _mapper.Map<User>(userDto);

        userModel = await _userRepository.CreateUser(userModel, cancellationToken);

        if (userModel == null) { return null; }

        try
        {
            var createdUser = _mapper.Map<UserCreatedDto>(userModel);
            var recordKey = $"User_Id_{userModel.Id}";
            await _cache.SetRecordAsync<User>(recordKey, userModel, cancellationToken);

            return CreatedAtAction("GetUser", new { id = createdUser.Id }, createdUser);
        }
        catch (Exception ex)
        {
            Console.WriteLine("There was a problem creating user", ex.Message);
            return Problem(ex.Message);
        }
    }

    #endregion

    #region DELETE

    // DELETE: api/Users/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUser(int id, CancellationToken cancellationToken)
    {
        var result = await _userRepository.DeleteUser(id, cancellationToken);
        if (result == false) { return NotFound(); }

        return NoContent();
    }

    #endregion
}
