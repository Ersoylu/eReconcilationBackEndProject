using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
	public class CompanyManager : ICompanyService
	{
		private readonly ICompanyDal _companyDal;
		public CompanyManager(ICompanyDal companyDal)
		{
			_companyDal= companyDal;
		}

		public IResult Add(Company company)
		{
			_companyDal.Add(company);
			return new SuccessResult(Messages.AddedCompany);
		}

		public IResult CompanyExists(Company company)
		{
			var result = _companyDal.Get(c => c.Name == company.Name && c.TaxDepartment == company.TaxDepartment && c.TaxIdNumber == company.TaxIdNumber 
			&& c.IdentityNumber == company.IdentityNumber);
			if (result != null)
			{
				return new ErrorResult(Messages.CompanyAllreadyExists);
			}
			return new SuccessResult();
		}

		public IDataResult<List<Company>> GetList()
		{
			return new SuccesDataResult<List<Company>>(_companyDal.GetList());
		}

		public IResult UserCompanyAdd(int userId, int companyId)
		{
			 _companyDal.UserCompanyAdd(userId, companyId);
			return new SuccessResult();
		}
	}
}
