using MapsterMapper;
using MiskCv_Api.Services.DistributedCacheService;

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
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUser()
    {
        IEnumerable<User>? userModels = null;

        var actionName = $"{nameof(GetUser)}";
        string recordKey = $"{actionName}_AllUsers";

        userModels = await _cache.GetRecordAsync<List<User>>(recordKey);

        if (userModels == null)
        {
            userModels = await _userRepository.GetUsers();

            if (userModels != null) 
            {
                await _cache.SetRecordAsync<IEnumerable<User>>(recordKey, userModels);
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
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        User? userModel = null;

        var recordKey = $"User_Id_{ id }";

        userModel = await _cache.GetRecordAsync<User>(recordKey);

        if(userModel == null)
        {
            userModel = await _userRepository.GetUser(id);

            if(userModel != null)
            {
                await _cache.SetRecordAsync<User>(recordKey, userModel);
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
    public async Task<IActionResult> PutUser([FromBody] UserUpdateDto userDto, int id)
    {
        var userModel = _mapper.Map<User>(userDto);
        
        if (id != userModel.Id)
        {
            return BadRequest();
        }

        var result = await _userRepository.UpdateUser(id, userModel);

        if (result == null)
        {
            return Problem("There was a problem updating user");
        }

        var recordKey = $"User_Id_{id}";
        await _cache.SetRecordAsync<User>(recordKey, userModel);

        return NoContent();
    }

    #endregion

    #region POST

    // POST: api/Users
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<UserCreatedDto>?> PostUser([FromBody] UserCreateDto userDto)
    {
        var userModel = _mapper.Map<User>(userDto);

        userModel = await _userRepository.CreateUser(userModel);

        if (userModel == null) { return null; }

        try
        {
            var createdUser = _mapper.Map<UserCreatedDto>(userModel);
            var recordKey = $"User_Id_{userModel.Id}";
            await _cache.SetRecordAsync<User>(recordKey, userModel);

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
    public async Task<IActionResult> DeleteUser(int id)
    {
        var result = await _userRepository.DeleteUser(id);
        if (result == false) { return NotFound(); }

        return NoContent();
    }

    #endregion
}
