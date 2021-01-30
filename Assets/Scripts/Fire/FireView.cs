using System;
using UnityEngine;

namespace Fire
{
    [Serializable]
    public struct Range
    {
        public float min;
        public float max;
    }


    public class FireView : MonoBehaviour
    {
        [Header("Live Controls")]
        //
        [SerializeField, Range(0, 1)] private float fireIntensity;

        [Header("Settings")]
        //
        [SerializeField] private Range scaleRange;

        [SerializeField] private ParticleSystem[] particles;

        [SerializeField] private Color fireAliveLight;
        [SerializeField] private Color fireDeadLight;
        [SerializeField] private Light fireLight;


        private void OnValidate()
        {
            SetFireIntensity(fireIntensity);
        }

        public void SetFireIntensity(float intensity)
        {
            fireIntensity = intensity;
            foreach (var part in particles)
            {
                part.transform.localScale = Vector3.one * Mathf.Lerp(scaleRange.min, scaleRange.max, fireIntensity);
            }

            fireLight.color = Color.Lerp(fireDeadLight, fireAliveLight, fireIntensity);
        }
    }
}