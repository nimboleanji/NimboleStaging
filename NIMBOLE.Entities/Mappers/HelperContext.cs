using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIMBOLE.Models.Mappers
{
    public static class HelperContext
    {
        /// <summary>
        /// Attaches entity graph to context using entity id to determinate if entity is new or modified.
        /// If Id is zero then entity is treated as NEW and otherwise it is treated as modified.
        /// If we want to save more than just root entity than child types must be supplied.
        /// If entity in graph is not root nor of child type it will be attached but not saved
        /// (it will be treated as unchanged).
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="rootEntity">The root entity.</param>
        /// <param name="childTypes">The child types that should be saved with root entity.</param>
        public static void AttachByIdValue<TEntity>(this DbContext context, TEntity rootEntity, HashSet<Type> childTypes)
            where TEntity : class, IEntity
        {
            // mark root entity as added
            // this action adds whole graph and marks each entity in it as added
            context.Set<TEntity>().Add(rootEntity);
            // in case root entity has id value mark it as modified (otherwise it stays added)
            if (rootEntity.Id != 0)
            {
                context.Entry(rootEntity).State = EntityState.Modified;
            }
            // traverse all entities in context (hopefully they are all part of graph we just attached)
            foreach (var entry in context.ChangeTracker.Entries<IEntity>())
            {
                // we are only interested in graph we have just attached
                // and we know they are all marked as Added 
                // and we will ignore root entity because it is already resolved correctly
                if (entry.State == EntityState.Added && entry.Entity != rootEntity)
                {
                    // if no child types are defined for saving then just mark all entities as unchanged)
                    if (childTypes == null || childTypes.Count == 0)
                    {
                        entry.State = EntityState.Unchanged;
                    }
                    else
                    {
                        // request object type from context because we might got reference to dynamic proxy
                        // and we wouldn't want to handle Type of dynamic proxy
                        Type entityType = ObjectContext.GetObjectType(entry.Entity.GetType());
                        // if type is not child type than it should not be saved so mark it as unchanged
                        if (!childTypes.Contains(entityType))
                        {
                            entry.State = EntityState.Unchanged;
                        }
                        else if (entry.Entity.Id != 0)
                        {
                            // if entity should be saved with root entity
                            // than if it has id mark it as modified 
                            // else leave it marked as added
                            entry.State = EntityState.Modified;
                        }
                    }
                }
            }
        }  
    }
}
