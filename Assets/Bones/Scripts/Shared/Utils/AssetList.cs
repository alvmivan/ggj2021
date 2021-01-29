using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bones.Scripts.Shared.Utils
{
    public class AssetList<T> : ScriptableObject, IReadOnlyList<T>
    {
        public List<T> elements = new List<T>();

        public IEnumerator<T> GetEnumerator() => elements.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) elements).GetEnumerator();
        public int Count => elements.Count;
        public T this[int index] => elements[index];
    }
}