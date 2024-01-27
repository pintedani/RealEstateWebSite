using Imobiliare.Entities;
using Imobiliare.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Imobiliare.Repositories
{
    public static class EntityEventFactory
    {
        /// <summary>
        /// Creates the entity event.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="entityState">State of the entity.</param>
        /// <param name="unitOfWork">The corresponding unit of work.</param>
        /// <param name="changedProperties">The changed properties.</param>
        /// <returns>
        /// The created entity event.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if the entity state is not known.</exception>
        public static EntityChange Create(Entity entity, EntityState entityState, IUnitOfWork unitOfWork, IReadOnlyDictionary<string, PropertyChange> changedProperties)
        {
            EntityChange @event = null;
            switch (entityState)
            {
                case EntityState.Unchanged:
                    break;
                case EntityState.Added:
                    @event = (EntityChange)CreateEvent(typeof(EntityAdded<>), entity.GetType(), entity, unitOfWork, changedProperties);
                    break;
                case EntityState.Deleted:
                    @event = (EntityChange)CreateEvent(typeof(EntityDeleted<>), entity.GetType(), entity, unitOfWork, changedProperties);
                    break;
                case EntityState.Modified:
                    @event = (EntityChange)CreateEvent(typeof(EntityUpdated<>), entity.GetType(), entity, unitOfWork, changedProperties);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(entityState));
            }

            return @event;
        }

        /// <summary>
        /// Creates the entity relationship event.
        /// </summary>
        /// <param name="entity1">The entity1.</param>
        /// <param name="entity2">The entity2.</param>
        /// <param name="entityState">State of the entity.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="relationshipName">Name of the relationship.</param>
        /// <returns>
        /// The created entity event.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if the entity state is not known.</exception>
        //public static EntityRelationshipChange Create(Entity entity1, Entity entity2, EntityState entityState, IUnitOfWork unitOfWork, string relationshipName)
        //{
        //    EntityRelationshipChange @event = null;
        //    switch (entityState)
        //    {
        //        case EntityState.Unchanged:
        //            break;
        //        case EntityState.Added:
        //            @event = (EntityRelationshipChange)CreateEvent(typeof(EntityRelationshipAddedEvent<,>), entity1.GetType(), entity1, entity2.GetType(), entity2, unitOfWork, relationshipName);
        //            break;
        //        case EntityState.Deleted:
        //            @event = (EntityRelationshipChange)CreateEvent(typeof(EntityRelationshipDeletedEvent<,>), entity1.GetType(), entity1, entity2.GetType(), entity2, unitOfWork, relationshipName);
        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException(nameof(entityState));
        //    }

        //    return @event;
        //}

        /// <summary>
        /// Dynamically creates an entity event.
        /// </summary>
        /// <param name="genericEventType">Type of the generic event.</param>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="unitOfWork">The related unit of work.</param>
        /// <param name="changedProperties">The changed properties.</param>
        /// <returns>
        /// The concrete entity event.
        /// </returns>
        private static object CreateEvent(Type genericEventType, Type entityType, Entity entity, IUnitOfWork unitOfWork, IReadOnlyDictionary<string, PropertyChange> changedProperties)
        {
            //TODO: check if Microsoft.EntityFrameworkCore has dynamic proxies like old EF 6
            Type[] typeArgs = { entityType.Namespace == "Microsoft.EntityFrameworkCore.DynamicProxies" ? entityType.BaseType : entityType };
            var concreteType = genericEventType.MakeGenericType(typeArgs);
            return Activator.CreateInstance(concreteType, entity, unitOfWork, changedProperties);
        }

        /// <summary>
        /// Dynamically creates an entity relationship event.
        /// </summary>
        /// <param name="genericEventType">Type of the generic event.</param>
        /// <param name="entityType1">The entity type1.</param>
        /// <param name="entity1">The entity1.</param>
        /// <param name="entityType2">The entity type2.</param>
        /// <param name="entity2">The entity2.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="relationshipName">Name of the relationship.</param>
        /// <returns>
        /// The concrete entity event.
        /// </returns>
        private static object CreateEvent(Type genericEventType, Type entityType1, Entity entity1, Type entityType2, Entity entity2, IUnitOfWork unitOfWork, string relationshipName)
        {
            Type[] typeArgs =
            {
        entityType1.Namespace == "Microsoft.EntityFrameworkCore.DynamicProxies" ? entityType1.BaseType : entityType1,
        entityType2.Namespace == "Microsoft.EntityFrameworkCore.DynamicProxies" ? entityType2.BaseType : entityType2
      };

            var concreteType = genericEventType.MakeGenericType(typeArgs);
            return Activator.CreateInstance(concreteType, entity1, entity2, unitOfWork, relationshipName);
        }
    }

    /// <summary>
    /// Base class for all Entity events sent via the message bus.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public abstract class EntityEvent<TEntity> : EntityChange where TEntity : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityEvent{TEntity}" /> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="unitOfWork">The related unit of work.</param>
        /// <param name="changedProperties">The changed properties.</param>
        protected EntityEvent(TEntity entity, IUnitOfWork unitOfWork, IReadOnlyDictionary<string, PropertyChange> changedProperties)
        {
            this.Entity = entity;
            this.Object = entity;
            this.ChangedProperties = changedProperties;
        }

        /// <summary>
        /// Gets the entity.
        /// </summary>
        public TEntity Entity { get; private set; }

        /// <summary>
        /// Gets or sets the related unit of work.
        /// </summary>
        public IUnitOfWork UnitOfWork { get; protected set; }

        /// <summary>
        /// Gets or sets the state of the entity.
        /// </summary>
        public EntityState EntityState { get; protected set; }
    }

    /// <summary>
    /// Entity event that is sent via message bus when an Entity has been deleted.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class EntityDeleted<TEntity> : EntityEvent<TEntity> where TEntity : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityDeleted{TEntity}" /> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="unitOfWork">The related unit of work.</param>
        /// <param name="changedProperties">The changed properties.</param>
        public EntityDeleted(TEntity entity, IUnitOfWork unitOfWork, IReadOnlyDictionary<string, PropertyChange> changedProperties)
          : base(entity, unitOfWork, changedProperties)
        {
            this.EntityState = EntityState.Deleted;
        }
    }

    /// <summary>
    /// Entity event that is sent via message bus when an Entity has been added.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class EntityAdded<TEntity> : EntityEvent<TEntity> where TEntity : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityAdded{TEntity}" /> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="unitOfWork">The related unit of work.</param>
        /// <param name="changedProperties">The changed properties.</param>
        public EntityAdded(TEntity entity, IUnitOfWork unitOfWork, IReadOnlyDictionary<string, PropertyChange> changedProperties)
          : base(entity, unitOfWork, changedProperties)
        {
            this.EntityState = EntityState.Added;
        }
    }

    /// <summary>
    /// Entity event that is sent via message bus when an existing Entity has been updated.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class EntityUpdated<TEntity> : EntityEvent<TEntity> where TEntity : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityUpdated{TEntity}" /> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="unitOfWork">The related unit of work.</param>
        /// <param name="changedProperties">The changed properties.</param>
        public EntityUpdated(TEntity entity, IUnitOfWork unitOfWork, IReadOnlyDictionary<string, PropertyChange> changedProperties)
          : base(entity, unitOfWork, changedProperties)
        {
            this.EntityState = EntityState.Modified;
        }
    }
}