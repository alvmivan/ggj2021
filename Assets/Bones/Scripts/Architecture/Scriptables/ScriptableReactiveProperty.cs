using System;
using UniRx;
using UnityEngine;

namespace Bones.Scripts.Architecture.Scriptables
{
    public class ScriptableReactiveProperty<T> : ScriptableObject, IReadOnlyReactiveProperty<T>
    {
        [SerializeField] private T inspectorValue;
        private ReactiveProperty<T> property = new ReactiveProperty<T>();

        private void Awake()
        {
            property = new ReactiveProperty<T>(inspectorValue);
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return property.Subscribe();
        }

        public T Value
        {
            get => property.Value;
            set => property.Value = value;
        }

        public bool HasValue => property.HasValue;
    }
}