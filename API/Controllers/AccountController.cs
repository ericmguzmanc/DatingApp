using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
  public class AccountController : BaseApiController
  {
    private readonly DataContext _context;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    public AccountController(DataContext context, ITokenService tokenService, IMapper mapper)
    {
      _mapper = mapper;
      _tokenService = tokenService;
      _context = context;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
      if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");

      var user = _mapper.Map<AppUser>(registerDto);

      // using calls the HMACSHA512 method after call to ensure it's disposal
      // This initializes the hmac instance with a generater key for that hash
      using var hmac = new HMACSHA512();


      user.UserName = registerDto.Username.ToLower();
      user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
      user.PasswordSalt = hmac.Key;



      _context.Users.Add(user);
      await _context.SaveChangesAsync();

      return new UserDto
      {
        Username = user.UserName,
        Token = _tokenService.CreateToken(user),
        KnownAs = user.KnownAs
      };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
      // user await if you make a call to the database
      // SingleOrDefault returns one or default plus throws exception
      var user = await _context.Users
      .Include(p => p.Photos)
      .SingleOrDefaultAsync(u => u.UserName == loginDto.Username);

      if (user is null) return Unauthorized("Invalid Username");

      using var hmac = new HMACSHA512(user.PasswordSalt);
      var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

      for (int i = 0; i < computedHash.Length; i++)
      {
        if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
      }

      return new UserDto
      {
        Username = user.UserName,
        Token = _tokenService.CreateToken(user),
        PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain)?.Url,
        KnownAs = user.KnownAs
      };
    }

    private async Task<bool> UserExists(string username)
    {
      return await _context.Users.AnyAsync(u => u.UserName == username.ToLower());
    }
  }
}