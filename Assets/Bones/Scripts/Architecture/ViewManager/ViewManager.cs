using System;
using UnityEngine;


namespace Bones.Scripts.Architecture.ViewManager
{
    public interface ViewManager
    {
        bool GetOut(string outPort);
        T Show<T>() where T : ViewNode;
        IObservable<T> ShowSwipe<T>(float duration = .1f, Vector2 direction = default) where T : ViewNode;
        IObservable<bool> GetOutSwipe(string outPort, float duration = .1f, Vector2 direction = default);
    }
}