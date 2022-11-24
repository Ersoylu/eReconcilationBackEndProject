using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results.Abstract;
using Entities.Concrete;

namespace Business.Abstract
{
	public interface ICompanyService
	{
		//Log
		//Validatiob
		//Transcaption
		//Användare rollen
		IResult Add(Company company);
		IDataResult<List<Company>>GetList();
		IResult CompanyExists(Company company);
		IResult UserCompanyAdd(int userId, int companyId);
	}
}
