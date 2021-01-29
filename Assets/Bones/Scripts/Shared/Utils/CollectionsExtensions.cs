using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Bones.Scripts.Shared.Utils
{
    public static class CollectionsExtensions
    {
        public static void DestroyChildren(this GameObject go, params GameObject[] exceptions)
        {
            if (!go || !go.transform) return;
            var objects = go
                .GetComponentsInChildren<Transform>()
                .Where(a => a)
                .Where(a => a.transform)
                .Select(g => g.gameObject)
                .Where(g=> !exceptions.Contains(g))
                .Where(g => g != go);

#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                foreach (var gameObject in objects)
                {
                    Object.DestroyImmediate(gameObject);
                }

                return;
            }
#endif
            foreach (var gameObject in objects) Object.Destroy(gameObject);
        }
    }
}