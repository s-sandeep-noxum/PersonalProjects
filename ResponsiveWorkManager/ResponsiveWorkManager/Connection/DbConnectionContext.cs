using Microsoft.EntityFrameworkCore;
using ResponsiveWorkManager.DataObjects;
using System.Configuration;

namespace ResponsiveWorkManager.Connection
{
	public class DbConnectionContext : DbContext
	{
		public DbSet<Leave> LeaveDetails => Set<Leave>();
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["LocalDB"].ConnectionString);
			base.OnConfiguring(optionsBuilder);
		}
	}
}
