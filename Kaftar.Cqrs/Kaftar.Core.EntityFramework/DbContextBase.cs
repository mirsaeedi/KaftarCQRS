using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Transactions;
using Kaftar.Core.Models;

namespace Kaftar.Core.EntityFramework
{
    public abstract class DbContextBase:DbContext
    {
        public string DefaultSchema { get; private set; }
        public long UserOfDbContextId { get; set; }
        public Assembly ModelAssembly { get; private set; }
        public DbContextBase(Assembly modelAssembly,string connectionString, string defaultSchema = "dbo")
            :base(connectionString)
        {
            ModelAssembly = modelAssembly;
            DefaultSchema = defaultSchema;
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public override int SaveChanges()
        {
            var changesCount = 0;

            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                FillAuditables();

                RaiseBeforeCrudEvents();

                changesCount = base.SaveChanges();

                RaiseAfterCrudEvents();

                RaiseTransactionalEvents(new ReadOnlyDataContext(this), scope);

                scope.Complete();
            }

            RaiseNonTransactionalEvents();

            return changesCount;
        }

        private void RaiseAfterCrudEvents()
        {
            foreach (var changeSet in ChangeTracker.Entries())
            {
                var type = changeSet.Entity.GetType();

                if (changeSet.State == EntityState.Modified)
                {
                    //ResolveUpdateEventHandlers
                }
                else if (changeSet.State == EntityState.Added)
                {

                }
                else if (changeSet.State == EntityState.Deleted)
                {

                }
            }
        }

        private void RaiseBeforeCrudEvents()
        {
            foreach (var changeSet in ChangeTracker.Entries())
            {
                var type = changeSet.Entity.GetType();

                if (changeSet.State == EntityState.Modified)
                {

                }
                else if (changeSet.State == EntityState.Added)
                {

                }
                else if (changeSet.State == EntityState.Deleted)
                {

                }
            }
        }

        private void RaiseTransactionalEvents(IReadOnlyDataContext readOnlyDbContext, TransactionScope scope)
        {
            
        }

        private void FillAuditables()
        {
            foreach (var changeSet in ChangeTracker.Entries<IAuditableEntity>())
            {
                var auditableEntity = changeSet.Entity as IAuditableEntity;

                if (changeSet.Property(p => p.CreateDateTime).IsModified || changeSet.Property(p => p.LastModifiedDateTime).IsModified)
                {
                    throw new DbEntityValidationException("Access Violated");
                }

                if (changeSet.State == EntityState.Modified)
                {
                    auditableEntity.LastModifierId = UserOfDbContextId;
                }
                else if (changeSet.State == EntityState.Added)
                {
                    auditableEntity.CreateDateTime = DateTime.Now;
                }
            }
        }

        private void RaiseNonTransactionalEvents()
        {
           
        }

        private void RaiseTransactionalEvents()
        {
            
        }

        private void RaiseEvents()
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            MapEntitiesToTables(modelBuilder);
        }

        private void MapEntitiesToTables(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(DefaultSchema);

            var entityMethod = typeof(DbModelBuilder).GetMethod("Entity");

            var entityTypes =
             ModelAssembly
            .GetTypes()
            .Where(t => typeof(IEntity).IsAssignableFrom(t));

            foreach (var type in entityTypes)
            {
                entityMethod.MakeGenericMethod(type)
                      .Invoke(modelBuilder, new object[] { });
            }

            modelBuilder.Properties<long>()
                   .Where(p => p.Name == "Id")
                   .Configure(p => p.IsKey());

            modelBuilder.Properties<DateTime>()
            .Configure(c => c.HasColumnType("datetime2"));

            modelBuilder.Properties<DateTime?>()
            .Configure(c => c.HasColumnType("datetime2"));

            modelBuilder.Types()
                .Configure(c => c.ToTable(c.ClrType.Name));

            ModelConfiguration(modelBuilder);

        }

        public virtual void ModelConfiguration(DbModelBuilder modelBuilder)
        {
            
        }
    }
}