using Business.Abstract;
using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
	public class AccountReconciliationDetailServiceManager: IAccountReconciliationDetailService
	{
		private readonly IAccountReconciliationDetailsDal _accountReconciliationDetailsDal;
		public AccountReconciliationDetailServiceManager(IAccountReconciliationDetailsDal accountReconciliationDetailsDal)
		{
			_accountReconciliationDetailsDal = accountReconciliationDetailsDal;
		}
	}
}
