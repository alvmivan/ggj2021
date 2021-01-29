using System;
using UnityEngine;

namespace Bones.Scripts.Architecture.ViewManager
{
    public abstract class ViewNode : MonoBehaviour, IDisposable
    {
        
        public RectTransform RectTransform => transform as RectTransform;
        public void Dispose()
        {
            OnDispose();
            if (this && gameObject)
                Destroy(gameObject);
        }

        public void Init()
        {
            OnInit();
        }

        public void ShowView()
        {
            gameObject.SetActive(true);
            OnShow();
        }

        public void HideView()
        {
            OnHide();
            gameObject.SetActive(false);
        }


        protected virtual void OnInit()
        {
        }

        protected virtual void OnDispose()
        {
        }

        protected virtual void OnShow()
        {
        }

        protected virtual void OnHide()
        {
        }
    }
}