using System;
using System.Collections.Generic;
using System.Text;

namespace Samples.Wrapper
{
    class Enity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    class Wrapper<T>
    {
        private readonly T entity;

        public Wrapper(T entity)
        {
            this.entity = entity;
        }

        public V Get<V>(Func<T, V> getter) => getter(entity);
        public T With(Action<T> builder)
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
