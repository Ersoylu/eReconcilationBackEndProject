﻿using Business.Abstract;
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

		List<Company> ICompanyService.GetList()
		{
			throw new NotImplementedException();
		}
	}
}
