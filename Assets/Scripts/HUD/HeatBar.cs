using System;
using Injector.Core;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Wolf;

namespace HUD
{
    public class HeatBar : MonoBehaviour
    {
        private WolfController wolf;
        public TextMeshProUGUI heathText;

        private void Start()
        {
            wolf = Injection.Get<WolfController>();
            wolf.currentHeat.Subscribe(HeatUpdated);
        }

        private void HeatUpdated(float heat)
        {
            var celsius = GetCelsius(heat);
            heathText.text = FormatCelsius(celsius);
            heathText.color = celsius > 0 ? Color.white : Color.cyan;
        }

        private string FormatCelsius(float c)
        {
            return c.ToString("0.00") + " C°";
        }

        private static float GetCelsius(float heat)
        {
            float celsius = (heat - 0.12f) * 100f;
            var cInt = (int) celsius;
            celsius = cInt / 100f;
            return celsius;
        }
    }
}