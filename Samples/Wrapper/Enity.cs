using ImmutableNet;
using Samples.Why;
using System;
using System.Collections.Generic;
using System.Text;

namespace Samples.WrapperMy
{
    class Enity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    class Immutable<T>
    {
        private readonly T entity;

        public Immutable(T entity)
        {
            this.entity = entity;
        }

        public V Get<V>(Func<T, V> getter) => getter(entity);
        public T Modify(Action<T> builder)
        {
            var result = Clone();
            builder(result);
            return result;
        }

        private T Clone()
        {
            throw new NotImplementedException();
        }
    }


}
