using Business.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;
	public AuthController (IAuthService authService)
	{
		_authService = authService;
	}

		[HttpPost("Register")]
		public IActionResult Register(UserAndCompanyRegisterDto userAndCompanyRegister)
		{

			var userExits = _authService.UserExits(userAndCompanyRegister.UserForRegister.Email);
			if (!userExits.Success)
			{
				return BadRequest(userExits.Message);
			}
			var companyExists= _authService.CompanyExits(userAndCompanyRegister.company);
			if (!companyExists.Success) 
			{
				return BadRequest(userExits.Message);
			}

			var registerResult = _authService.Register(userAndCompanyRegister.UserForRegister, userAndCompanyRegister.UserForRegister.Password, userAndCompanyRegister.company);

			var result = _authService.CreateAccessToken(registerResult.Data, registerResult.Data.CompanyId);
			if (result.Success)
			{
				return Ok(result.Data);
			}
			
			return BadRequest(registerResult.Message);
		}
				[HttpPost("RegisterSecondAccount")]
				public IActionResult RegisterSecondAccount(UserForRegister userForRegister, int companyId)
				{
					var userExits = _authService.UserExits(userForRegister.Email);
					if (!userExits.Success)
					{
						return BadRequest(userExits.Message);
					}
					var registerResult = _authService.RegisterSecondAccount(userForRegister, userForRegister.Password);
					var result = _authService.CreateAccessToken(registerResult.Data, 0);
					if (result.Success)
					{
						return Ok(result.Data);
					}
					//if (registerResult.Success)
					//{
					//	return Ok(registerResult);
					//}
					return BadRequest(registerResult.Message);
				}
		[HttpPost("Login")]
		public IActionResult Login(UserForLogin userForLogin)
		{
			var userToLogin = _authService.Login(userForLogin);
			if (!userToLogin.Success)
			{
				return BadRequest(userToLogin.Message);
			}
			var result = _authService.CreateAccessToken(userToLogin.Data, 0);
			if (result.Success)
			{
				return Ok(result.Data);
			}
			return BadRequest(result.Message);
		}
	}

}
