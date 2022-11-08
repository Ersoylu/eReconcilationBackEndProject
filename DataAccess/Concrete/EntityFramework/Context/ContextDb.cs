using Core.Entities.Concrete;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework.Context
{
	public class ContextDb : DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(@"Server=DESKTOP-94IFRKV\SQLEXPRESS;Database=eReconcilationDb;Integrated Security=true");
		}

		public DbSet<AccountReconciliation> accountReconciliations { get; set; }
		public DbSet<AccountReconciliationDetail> accountReconciliationDetails { get; set; }
		public DbSet<BaBsReconciliation> baBsReconciliations { get; set; }
		public DbSet<BaBsReconciliationDetail> baBsReconciliationDetails { get; set; }
		public DbSet<Company> companies { get; set; }
		public DbSet<Currency> currencies { get; set; }
		public DbSet<CurrencyAccount> currencyAccounts { get; set; }
		public DbSet<MailParameter> mailParameters { get; set; }
		public DbSet<OperationClaim> operationClaims { get; set; }
		public DbSet<User> users { get; set; }
		public DbSet<UserCompany> userCompanies {get; set;}
		public DbSet<UserOperationClaim> userOperationClaims {get; set;}

	}
}
//CRUD - Create, Read, Update, Delete
