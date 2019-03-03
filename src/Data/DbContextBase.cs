using System;
using System.Linq;
using System.Reflection;
using System.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Kaftar.Core.Data
{
    public abstract class DbContextBase:DbContext
    {
        private long UserOfDbContextId { get; }
        private string DefaultSchema { get; }
        private Assembly ModelAssembly { get;}

        public DbContextBase(Assembly modelAssembly, DbContextOptions<DbContextBase> options, string defaultSchema = "dbo")
            :base(options)
        {
            ModelAssembly = modelAssembly;
            DefaultSchema = defaultSchema;
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
                var auditableEntity = changeSet.Entity;

                if (changeSet.Property(p => p.CreateDateTime).IsModified || changeSet.Property(p => p.LastModifiedDateTime).IsModified)
                {
                    throw new InvalidOperationException($"Access Violated. You are not supposed to modify CreateDateTime and LastModifiedDateTime fileds");
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            MapEntitiesToTables(modelBuilder);
        }

        private void MapEntitiesToTables(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(DefaultSchema);

            var entityMethod = typeof(ModelBuilder).GetMethod("Entity");

            var entityTypes = ModelAssembly
            .GetTypes()
            .Where(t => typeof(IEntity).IsAssignableFrom(t));

            foreach (var type in entityTypes)
            {
                entityMethod.MakeGenericMethod(type)
                      .Invoke(modelBuilder, new object[] { });
            }
        }
    }
}