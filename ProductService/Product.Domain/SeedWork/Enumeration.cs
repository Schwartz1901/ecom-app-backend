using System;
using System.Collections.Generic;
using System.Linq;

namespace Product.Domain.SeedWork
{
    public abstract class Enumeration : IComparable
    {
        public string Name { get; private set; }
        public int Id { get; private set; }

        protected Enumeration(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString() => Name;

        public static IEnumerable<T> GetAll<T>() where T : Enumeration
        {
            return typeof(T)
                .GetFields(System.Reflection.BindingFlags.Public |
                           System.Reflection.BindingFlags.Static |
                           System.Reflection.BindingFlags.DeclaredOnly)
                .Select(f => f.GetValue(null))
                .Cast<T>();
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Enumeration otherValue) return false;

            return Id.Equals(otherValue.Id);
        }

        public override int GetHashCode() => Id.GetHashCode();

        public static bool operator ==(Enumeration left, Enumeration right) => Equals(left, right);
        public static bool operator !=(Enumeration left, Enumeration right) => !(left == right);

        public int CompareTo(object? other) => Id.CompareTo(((Enumeration)other!).Id);
    }
}
