using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Hashing;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using Core.Utilities.Security.JWT;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
	public class AuthManager : IAuthService
	{
		private readonly IUserService _userService;
		private readonly ITokenHelper _tokenHelper;
		private readonly ICompanyService _companyService;
		private readonly IMailParameterService _mailParameterService;
		private readonly IMailService _mailService;
		private readonly IMailTemplateService _mailTemplateService;

		public AuthManager(IUserService userService, ITokenHelper tokenHelper, ICompanyService companyService, IMailParameterService mailParameterService
			, IMailService mailService, IMailTemplateService mailTemplateService)
		{
			_userService= userService;
			_tokenHelper = tokenHelper;
			_companyService = companyService;
			_mailParameterService = mailParameterService;
			_mailService = mailService;
			_mailTemplateService = mailTemplateService;
		}

		public AuthManager(IUserService userService, ITokenHelper tokenHelper, ICompanyService companyService, 
			IMailParameterService mailParameterService, IMailService mailService) 
		{
			_mailParameterService = mailParameterService;
			_mailService = mailService;
			_userService = userService;
			_tokenHelper = tokenHelper;
			_companyService = companyService;
		}

		public IResult CompanyExits(Company company)
		{
			var result = _companyService.CompanyExists(company);
			if (result.Success == false)
			{
				return new ErrorResult(Messages.CompanyAllreadyExists);
			}
			return new SuccessResult();
		}

		public IDataResult<AccessToken> CreateAccessToken(User user,int companyId)
		{
			var claims = _userService.GetClaims(user, companyId);
			var accessToken = _tokenHelper.CreateToken(user, claims, companyId);
			return new SuccesDataResult<AccessToken>(accessToken);
		}

		public IDataResult<User> Login(UserForLogin userForLogin)
		{
			var userToCheck = _userService.GetByMail(userForLogin.Email);
			if (userToCheck == null) 
			{
				return new ErrorDataResult<User>(Messages.UserNotFound);
			}
			if (!HashingHelper.VerifyPasswordHash(userForLogin.Password,userToCheck.PasswordHash,userToCheck.PasswordSalt))
			{
				return new ErrorDataResult<User>(Messages.PasswordError);
			}
			return new SuccesDataResult<User>(userToCheck, Messages.SuccessfulLogin);
		}

		public IDataResult<UserCompanyDto> Register(UserForRegister userForRegister, string password, Company company)
		{
			byte[] passwordHash, passwordSalt;
			HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);
			var user = new User()
			{
				Email = userForRegister.Email,
				AddedAt = DateTime.Now,
				IsActive = true,
				MailConfirm= false,
				MailConfirmDate= DateTime.Now,
				MailConfirmValue= Guid.NewGuid().ToString(),
				PasswordHash=passwordHash,
				PasswordSalt=passwordSalt,
				Name=userForRegister.Name,
			};

			_userService.Add(user);
			_companyService.Add(company);

			_companyService.UserCompanyAdd(user.Id, company.Id);

			UserCompanyDto userCompanyDto = new UserCompanyDto()
			{
				Id = user.Id,
				Name = user.Name,
				Email = user.Email,
				AddedAt = user.AddedAt,
				CompanyId = company.Id,
				IsActive = true,
				MailConfirm = user.MailConfirm,
				MailConfirmDate = user.MailConfirmDate,
				MailConfirmValue = user.MailConfirmValue,
				PasswordHash = user.PasswordHash,
				PasswordSalt = user.PasswordSalt

			};

			string subject = "user confrim register";
			string body = "User registration is now on the system. For complete the registration click on the link below.";
			string link = "https://localhost:7258";
			string linkDescription = "for confirmation of the registration click on the link below";

			var mailTemplate = _mailTemplateService.GetByTemplateName("Registration", 4);
			string templateBody = mailTemplate.Data.Value;
			templateBody = templateBody.Replace("{{title}}", subject);
			templateBody = templateBody.Replace("{{message}}", body);
			templateBody = templateBody.Replace("{{link}}", link);
			templateBody = templateBody.Replace("{{LinkDescription}}", linkDescription);

			var mailParameter = _mailParameterService.Get(4);
			SendMailDto sendMailDto = new SendMailDto()
			{
				mailParameter = mailParameter.Data,
				email = user.Email,
				subject = "user confrim register",
				body = templateBody,
			};

			_mailService.SendMail(sendMailDto);

			return new SuccesDataResult<UserCompanyDto>(userCompanyDto, Messages.UserRegistered);
		}

		public IDataResult<User> RegisterSecondAccount(UserForRegister userForRegister, string password)
		{
			byte[] passwordHash, passwordSalt;
			HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);
			var user = new User()
			{
				Email = userForRegister.Email,
				AddedAt = DateTime.Now,
				IsActive = true,
				MailConfirm = false,
				MailConfirmDate = DateTime.Now,
				MailConfirmValue = Guid.NewGuid().ToString(),
				PasswordHash = passwordHash,
				PasswordSalt = passwordSalt,
				Name = userForRegister.Name,
			};
			_userService.Add(user);
			return new SuccesDataResult<User>(user, Messages.UserRegistered);
		}

		public IResult UserExits(string email)
		{
			if (_userService.GetByMail(email)!=null)
			{
				return new ErrorResult(Messages.UserAlreadyExits);
			}
			return new SuccessResult();
		}
	}
}
