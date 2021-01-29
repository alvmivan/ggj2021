using System;
using System.Collections.Generic;

namespace Bones.Scripts.Shared.Utils
{
    public class TypedPool
    {
        public delegate IDisposable Finder(Type type);

        private readonly Finder finder;
        private readonly Dictionary<Type, IDisposable> instances = new Dictionary<Type, IDisposable>();

        public TypedPool(Finder finder)
        {
            this.finder = finder;
        }

        public T Get<T>() where T : IDisposable
        {
            var type = typeof(T);
            return (T) Get(type);
        }

        public IDisposable Get(Type type)
        {
            if (!instances.TryGetValue(type, out var instance))
            {
                instance = finder(type);
                instances[type] = instance;
            }

            return instance;
        }

        public void Clear()
        {
            foreach (var instancesValue in instances.Values) instancesValue.Dispose();
            instances.Clear();
        }
    }
}