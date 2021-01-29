using System;
using System.Collections;
using Bones.Scripts.Architecture.Context;
using Bones.Scripts.Shared.Utils;
using Injector.Core;
using UniRx;
using UnityEngine;

namespace Bones.Scripts.Architecture.ViewManager
{
    public class ViewManagerModule : ScriptModule, ViewManager
    {
        [SerializeField] private ViewNodesNavigation config;
        public Canvas canvas;

        private ViewNode current;
        private TypedPool pool;
        
        [Space(10)] public bool useTransition;
        public GameObject screenClickBLocker;
        private void OnDestroy()
        {
            pool.Clear();
        }

        
        
        public T Show<T>() where T : ViewNode
        {
            var viewNode = pool.Get<T>();
            return (T) ShowNode(viewNode);
        }

        private ViewNode ShowNode(ViewNode viewNode) 
        {
            viewNode.ShowView();
            if (current)
                current.HideView();
            current = viewNode;
            current.RectTransform.anchoredPosition = Vector2.zero;
            viewNode.RectTransform.anchoredPosition = Vector2.zero;
            return viewNode;
        }
        
        public IObservable<bool> GetOutSwipe(string outputKey, float duration = 1, Vector2 direction = default)
        {
            if (!current) return Observable.Return(false);
            var node = config.FromNode(current.GetType(), outputKey);
            if (!node) return Observable.Return(false);
            
            
            
            if (duration <= 0.001f)
            {
                return Observable.Return(GetOut(outputKey));
            }
            
            if (direction == default)
            {
                direction = Vector2.left;
            }
            direction.Normalize();

            node = (ViewNode) pool.Get(node.GetType());
            node.ShowView();
            return SwapWindows(current, node, duration, direction)
                .ToObservable()
                .Do(_ =>
                {
                    if (current)
                        current.HideView();
                    current = node;
                })
                .Select(_=>node!=null);
        }

        public IObservable<T> ShowSwipe<T>(float duration = 1, Vector2 direction = default) where T : ViewNode
        {
            if (duration <= 0.001f)
            {
                return Observable.Return(Show<T>());
            }
            
            if (direction == default)
            {
                direction = Vector2.left;
            }
            direction.Normalize();
            
            var viewNode = pool.Get<T>();
            viewNode.ShowView();
            return SwapWindows(current, viewNode, duration, direction)
                .ToObservable()
                .Do(_ =>
                {
                    if (current)
                        current.HideView();
                    current = viewNode;
                })
                .Select(_=>viewNode);
        }

        

        private IEnumerator SwapWindows(ViewNode currentNode, ViewNode nextNode, float duration, Vector2 direction)
        {
            screenClickBLocker.SetActive(true);
            var rect = currentNode.RectTransform.rect;
            Vector2 nextTargetPosition = Vector2.zero;//nextNode.RectTransform.anchoredPosition;
            Vector2 nextSourcePosition = nextTargetPosition - direction * rect.size;
            Vector2 currentSourcePosition = Vector2.zero;//currentNode.RectTransform.anchoredPosition;
            Vector2 currentTargetPosition = currentSourcePosition+direction * rect.size;
            
            nextNode.transform.position = nextSourcePosition;

            var time = duration;
            while (time >= 0)
            {
                time -= Time.deltaTime;
                var t = 1-(time / duration);
                currentNode.RectTransform.anchoredPosition = Vector3.Lerp(currentSourcePosition, currentTargetPosition, t);
                nextNode.RectTransform.anchoredPosition = Vector3.Lerp(nextSourcePosition, nextTargetPosition, t);
                yield return null;
            }
            screenClickBLocker.SetActive(false);
        }

        public override void Init()
        {
            pool = new TypedPool(Create);
            Injection.Register((ViewManager) this);
            canvas.gameObject.DestroyChildren(screenClickBLocker);
            config.Init();
        }

        public bool GetOut(string outputKey)
        {
            if (!current) return false;
            var node = config.FromNode(current.GetType(), outputKey);
            if (!node) return false;
            ShowNode((ViewNode) pool.Get(node.GetType()));
            return true;
        }
        
        private IDisposable Create(Type type)
        {
            var prefab = config.Find(type);
            var instance = Instantiate(prefab, canvas.transform);
            instance.transform.SetAsFirstSibling();
            instance.Init();
            return instance;
        }
    }
}