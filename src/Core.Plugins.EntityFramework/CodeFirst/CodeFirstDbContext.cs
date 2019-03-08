using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Core.Plugins.EntityFramework.CodeFirst.DataAnnotations;
using Core.Plugins.EntityFramework.DbContext;

namespace Core.Plugins.EntityFramework.CodeFirst
{
    public abstract class CodeFirstDbContext : System.Data.Entity.DbContext, IEntityFrameworkDbContext
    {
        public CodeFirstDbContext(string connectionString)
            : base(connectionString) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ConfigureModel(modelBuilder);

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Properties()
                .Having(property => property.GetCustomAttributes(true).OfType<DataAnnotationPropertyAttribute>().ToList())
                .Configure((configuration, attributes) => attributes.ForEach(a => { a.Configure(configuration); }));
        }

        protected virtual void ConfigureModel(DbModelBuilder modelBuilder)
        {
            
        }
    }
}
