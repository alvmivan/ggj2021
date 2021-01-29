using System;
using System.Collections;
using Bones.Scripts.Architecture.ViewManager;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace CurrentGame
{
    public class CurrentGameMainView : ViewNode
    {
        public Image cover;

        public GameObject quitView;

        public GameObject game;
        private GameObject current;

        private readonly CompositeDisposable disposables = new CompositeDisposable();

        protected override void OnShow()
        {
            disposables.Clear();
            if (current)
            {
                Destroy(current);
            }

            quitView.SetActive(false);

            current = Instantiate(game);
            SetActiveCover(true)
                .ToObservable()
                .Subscribe()
                .AddTo(disposables);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                quitView.SetActive(!quitView.activeSelf);
            }
        }

        IEnumerator SetActiveCover(bool active)
        {
            var alphaDestination = active ? 0f : 1f;
            var color = cover.color;

            var initCol = color;
            initCol.a = 1 - alphaDestination;
            var endCol = color;
            endCol.a = alphaDestination;
            var t = 1f;
            cover.gameObject.SetActive(true);
            while (t > 0)
            {
                t -= Time.deltaTime * 0.5f;
                cover.color = Color.Lerp(endCol, initCol, t);
                yield return null;
            }

            cover.gameObject.SetActive(active);
        }

        protected override void OnHide()
        {
            quitView.SetActive(false);
            disposables.Clear();
            Destroy(current);
            current = null;
        }
    }
}