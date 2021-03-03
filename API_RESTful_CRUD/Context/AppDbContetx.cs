using API_RESTful_CRUD.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_RESTful_CRUD.Context
{
    public class AppDbContetx: DbContext
    {
        public AppDbContetx(DbContextOptions<AppDbContetx> options):base(options)
        {

        }

        public DbSet<Producto> Producto { get; set;  }
    }
}
