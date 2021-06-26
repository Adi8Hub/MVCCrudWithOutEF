using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MVCCrudWithoutEF.Models;

namespace MVCCrudWithoutEF.Data
{
    public class MVCCrudWithoutEFContext : DbContext
    {
        public MVCCrudWithoutEFContext (DbContextOptions<MVCCrudWithoutEFContext> options)
            : base(options)
        {
        }

        public DbSet<MVCCrudWithoutEF.Models.BookViewModel> BookViewModel { get; set; }
    }
}
