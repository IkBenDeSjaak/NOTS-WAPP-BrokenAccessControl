#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NOTS_WAPP_Demo_BrokenAccessControl.Models;

namespace NOTS_WAPP_Demo_BrokenAccessControl.Data
{
    public class NOTS_WAPP_Demo_BrokenAccessControlContext : DbContext
    {
        public NOTS_WAPP_Demo_BrokenAccessControlContext (DbContextOptions<NOTS_WAPP_Demo_BrokenAccessControlContext> options)
            : base(options)
        {
        }

        public DbSet<NOTS_WAPP_Demo_BrokenAccessControl.Models.Blog> Blog { get; set; }
    }
}
