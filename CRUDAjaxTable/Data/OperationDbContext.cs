using System.Data.Entity;
using CRUDAjaxTable.Models;

namespace CRUDAjaxTable.Data
{
    public class OperationDbContext : DbContext
    {
        public OperationDbContext() : base("CrudData")
        {
            this.Configuration.LazyLoadingEnabled = true;
        //    this.Configuration.ProxyCreationEnabled = true;
        }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Operation> Operations { get; set; } 
    }
}