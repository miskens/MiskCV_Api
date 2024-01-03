using MapsterMapper;
using Microsoft.Extensions.Caching.Distributed;

namespace MiskCv_Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _cache;

    public UsersController(
            IUserRepository userRepository,
            IMapper mapper,
            IDistributedCache cache)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _cache = cache;
    }

    private IEnumerable<User>? userModels;

    #region GET

    // GET: api/Users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUser()
    {
        userModels = null;

        string recordKey = "Users_" + DateTime.Now.ToString("yyyyMMdd_hhmm");

        userModels = await _cache.GetRecordAsync<List<User>>(recordKey);

        if (userModels == null)
        {
            userModels = await _userRepository.GetUsers();

            if (userModels != null) 
            {
                await _cache.SetRecordAsync<IEnumerable<User>>(recordKey, userModels, null, null);
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
        var userModel = await _userRepository.GetUser(id);

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
    public async Task<IActionResult> PutUser(int id, UserUpdateDto userDto)
    {
        var user = _mapper.Map<User>(userDto);
        
        if (id != user.Id)
        {
            return BadRequest();
        }

        var result = await _userRepository.UpdateUser(id, user);

        if (result == null)
        {
            return Problem("There was a problem updating user");
        }

        return NoContent();
    }

    #endregion

    #region POST

    // POST: api/Users
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<UserCreatedDto>?> PostUser(UserCreateDto userDto)
    {
        var user = _mapper.Map<User>(userDto);

        var newUser = await _userRepository.CreateUser(user);

        if (newUser == null) { return null; }

        try
        {
            var createdUser = _mapper.Map<UserCreatedDto>(newUser);

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
