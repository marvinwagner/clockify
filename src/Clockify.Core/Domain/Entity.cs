using Clockify.Core.Messages;
using FluentValidation;
using System;
using System.Collections.Generic;

namespace Clockify.Core.Domain
{
    public abstract class Entity<T> where T : class
    {
        public Guid Id { get; set; }

        private List<Event> _notifications;
        public IReadOnlyCollection<Event> Notifications => _notifications?.AsReadOnly();

        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        protected void Validate(T entity, AbstractValidator<T> validator) 
            => validator.ValidateAndThrow(entity);

        public void AddEvent(Event @event)
        {
            _notifications ??= new List<Event>();
            _notifications.Add(@event);
        }

        public void RemoveEvent(Event @event)
        {
            _notifications?.Remove(@event);
        }

        public void ClearEvents()
        {
            _notifications?.Clear();
        }

        public virtual bool IsValid() 
        {
            return true;
        }

        public override bool Equals(object obj)
        {
            var compareTo = obj as Entity<T>;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return Id.Equals(compareTo.Id);
        }

        public static bool operator ==(Entity<T> a, Entity<T> b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null)) return true;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity<T> a, Entity<T> b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"{GetType().Name} [Id={Id}]";
        }
    }
}
