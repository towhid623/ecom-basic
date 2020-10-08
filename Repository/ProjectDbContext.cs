using Entities.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace Repository
{
    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext() : base("name = projectconnectionstring")
        {
        }

        public virtual DbSet<ChangeLog> ChangeLog { get; set; }
        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<FndFlexValueSet> FndFlexValueSet { get; set; }
        public virtual DbSet<FndFlexValue> FndFlexValue { get; set; }
        public virtual DbSet<ProductCategory> ProductCategory { get; set; }
        public virtual DbSet<ProductSubCategory> ProductSubCategory { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderItem> OrderItem { get; set; }
        public virtual DbSet<Banner> Banners { get; set; }


        public override int SaveChanges()
        {
            return CustomSaveChange();
        }

        #region SaveChanges Function & Activity Log
        object GetPrimaryKeyValue(DbEntityEntry entry)
        {
            var objectStateEntry = ((IObjectContextAdapter)this).ObjectContext.ObjectStateManager.GetObjectStateEntry(entry.Entity);
            return objectStateEntry.EntityKey.EntityKeyValues[0].Value;
        }
        
        public int CustomSaveChange()
        {
            var changes = from e in this.ChangeTracker.Entries()
                          where e.State != EntityState.Unchanged
                          select e;
            if (changes.Any())
            {
                var id = HttpContext.Current.User.Identity.GetUserId<int>();
                foreach (var change in changes)
                {
                    var entityName = change.Entity.GetType().Name;
                    if (change.State == EntityState.Added)
                    {
                        base.SaveChanges();
                        EntitySetBase setBase = ((IObjectContextAdapter)this).ObjectContext.ObjectStateManager
                            .GetObjectStateEntry(change.Entity).EntitySet;
                        string[] keyNames = setBase.ElementType.KeyMembers.Select(k => k.Name).ToArray();
                        string keyName;
                        if (keyNames != null)
                            keyName = keyNames.FirstOrDefault();
                        else
                            keyName = "(NotFound)";
                        var item = change.Entity;
                        var currentValues = this.Entry(item).CurrentValues;
                        foreach (string propertyName in currentValues.PropertyNames)
                        {
                            var current = currentValues[propertyName];
                            if (current != null)
                            {
                                ChangeLog log = new ChangeLog()
                                {
                                    EntityName = entityName,
                                    PrimaryKeyValue = keyName != "(NotFound)" && this.Entry(item).CurrentValues[keyName] != null ? this.Entry(item).CurrentValues[keyName].ToString() : null,
                                    PropertyName = propertyName,
                                    NewValue = current != null ? current.ToString() : null,
                                    DateChanged = DateTime.Now,
                                    ChangeType = (int)DbChangeType.Add,
                                    CreatedBy = id,
                                    CreationDate = DateTime.Now
                                };
                                ChangeLog.Add(log);
                            }
                        }
                    }
                    else if (change.State == EntityState.Modified)
                    {
                        var primaryKey = GetPrimaryKeyValue(change);
                        var item = change.Entity;
                        var originalValues = this.Entry(item).OriginalValues;
                        var currentValues = this.Entry(item).CurrentValues;
                        foreach (string propertyName in originalValues.PropertyNames)
                        {
                            var original = originalValues[propertyName];
                            var current = currentValues[propertyName];
                            if (!Equals(original, current))
                            {
                                ChangeLog log = new ChangeLog()
                                {
                                    EntityName = entityName,
                                    PrimaryKeyValue = primaryKey.ToString(),
                                    PropertyName = propertyName,
                                    OldValue = original != null ? original.ToString() : null,
                                    NewValue = current != null ? current.ToString() : null,
                                    DateChanged = DateTime.Now,
                                    ChangeType = (int)DbChangeType.Edit,
                                    CreatedBy = id,
                                    CreationDate = DateTime.Now
                                };
                                ChangeLog.Add(log);
                            }
                        }
                    }
                    else if (change.State == EntityState.Deleted)
                    {
                        var primaryKey = GetPrimaryKeyValue(change);
                        var item = change.Entity;
                        var originalValues = this.Entry(item).OriginalValues;
                        foreach (string propertyName in originalValues.PropertyNames)
                        {
                            var original = originalValues[propertyName];
                            if (original != null)
                            {
                                ChangeLog log = new ChangeLog()
                                {
                                    EntityName = entityName,
                                    PrimaryKeyValue = primaryKey != null ? primaryKey.ToString() : null,
                                    PropertyName = propertyName,
                                    OldValue = original != null ? original.ToString() : null,
                                    DateChanged = DateTime.Now,
                                    ChangeType = (int)DbChangeType.Delete,
                                    CreatedBy = id,
                                    CreationDate = DateTime.Now
                                };
                                ChangeLog.Add(log);
                            }
                        }
                    }
                }
            }
            return base.SaveChanges();
        }
        #endregion
    }
}
