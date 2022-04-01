using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IKAnimation
{
    using DG.Tweening;
    public static class IKUtils 
    {
        public delegate void OnCompleteCallback();
        public static void MoveTo(this Transform tr , Vector3 targetPoint, float transitionTime, float startDelay, OnCompleteCallback callback = null)
        {
            tr.DOMove(targetPoint, transitionTime).SetDelay(startDelay).SetUpdate(UpdateType.Fixed).SetEase(Ease.Linear).OnComplete(()=> callback());
            Debug.Log($"{tr.gameObject.name} is moved to {targetPoint} succesfully.");
        }

        public static void To(this float fromVal, float toVal, float transitionTime, float startDelay, OnCompleteCallback callback = null)
        {
            var oldVal = fromVal;
            DOTween.To(()=> fromVal, x => fromVal = x, toVal, transitionTime).SetDelay(startDelay).SetUpdate(UpdateType.Fixed).SetEase(Ease.Linear).OnComplete(()=> callback());
            Debug.Log($"{oldVal} is smoothly changed to {fromVal} in {transitionTime}.");
        }
    }
}

