using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RecieveEPHClient.Model
{
    public partial class SmartButtonContext : DbContext
    {
        public SmartButtonContext()
        {
        }

        public SmartButtonContext(DbContextOptions<SmartButtonContext> options)
            : base(options)
        {
        }

        public virtual DbSet<UserDevices> UserDevices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=pacificsoft.database.windows.net;Database=SmartButton;User Id=soporte;Password=Pacific12Pacific12;");
            }
        }

        
    }
}
