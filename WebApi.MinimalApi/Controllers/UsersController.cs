using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi.MinimalApi.Domain;
using WebApi.MinimalApi.Models;

namespace WebApi.MinimalApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : Controller
{
    // Чтобы ASP.NET положил что-то в userRepository требуется конфигурация
    private readonly IUserRepository userRepository;
    private readonly IMapper mapper;
    public UsersController(IUserRepository userRepository, IMapper mapper)
    {
        this.userRepository = userRepository;
        this.mapper = mapper;
    }

    [HttpGet("{userId}", Name = nameof(GetUserById))]
    [Produces("application/json", "application/xml")]
    public ActionResult<UserDto> GetUserById([FromRoute] Guid userId)
    {
        var user = userRepository.FindById(userId);

        if (user is null)
            return NotFound();

        var userDto = mapper.Map<UserDto>(user);
        return Ok(userDto);
    }
    
    [HttpPost]
    [Produces("application/json", "application/xml")]

    public IActionResult CreateUser([FromBody] UserToCreateDTO user)
    {

        if (string.IsNullOrEmpty(user.Login))
        {
            return UnprocessableEntity(ModelState);
        }

        var userEnt = mapper.Map<UserEntity>(user);
        
        if (userEnt is null)
            return BadRequest();

        var createdUser = userRepository.Insert(userEnt);
        
        return CreatedAtRoute(
            nameof(GetUserById),
            new { userId = createdUser.Id },
            createdUser.Id);
    }
}