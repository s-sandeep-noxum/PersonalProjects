using Microsoft.EntityFrameworkCore;
using System.Configuration;
using WorkManager.Data;

namespace WorkManager
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
