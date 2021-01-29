using System;
using System.Linq;
using Bones.Scripts.Features.Currency.Infrastructure;
using Bones.Scripts.Features.Currency.Services;
using Injector.Core;
using UnityEditor;
using UnityEngine;

namespace Bones.Scripts.Features.Currency.UnityDelivery.Editor
{
    public class CurrenciesInspector : EditorWindow
    {
        [MenuItem(Const.GameNameMenu + "Currency/Currencies")]
        private static void ShowWindow()
        {
            var window = GetWindow<CurrenciesInspector>();
            window.titleContent = new GUIContent("Currencies");
            window.minSize = Vector2.one * 300;
            window.Show();
        }

        private readonly Lazy<LocalCurrencyRepository> repository =
            new Lazy<LocalCurrencyRepository>(() => new LocalCurrencyRepository());

        private LocalCurrencyRepository Repo => repository.Value;


        private void OnGUI()
        {
            GUILayout.Space(10);
            var service = Injection.Get<CurrenciesService>();
            if (service == null || !EditorApplication.isPlaying)
            {
                EditorMode();
            }
            else
            {
                UsingService(service);
            }
        }

        private void UsingService(CurrenciesService service)
        {
            var list = Repo.currencies.Keys.ToList();
            EditorGUILayout.LabelField("WARNING changes will be reflected in game real-time");
            foreach (var key in list)
            {
                var newValue = EditorGUILayout.IntField(key, service[key]);
                // ReSharper disable once RedundantCheckBeforeAssignment
                if (newValue != service[key])
                {
                    service[key] = newValue;
                }
            }
        }

        private string newCurrencyName = "";
        private bool addingCurrency = false;

        private void EditorMode()
        {
            Repo.Load();
            var list = Repo.currencies.ToList();
            foreach (var pair in list)
            {
                EditorGUILayout.BeginHorizontal();
                var newValue = EditorGUILayout.IntField(pair.Key, pair.Value);
                if (newValue != pair.Value)
                {
                    Repo.currencies[pair.Key] = newValue;
                }

                if (GUILayout.Button("[X]", GUILayout.MinWidth(10)))
                {
                    Repo.currencies.Remove(pair.Key);
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space(25);

            if (GUILayout.Button(addingCurrency ? "  x" : "  > (add new currency)", GUIStyle.none))
            {
                addingCurrency = !addingCurrency;
            }

            if (addingCurrency)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Space(10);
                newCurrencyName = EditorGUILayout.TextField("New Field", newCurrencyName);
                if (!string.IsNullOrEmpty(newCurrencyName) && newCurrencyName.Length > 1 &&
                    !Repo.currencies.ContainsKey(newCurrencyName) && GUILayout.Button("[ADD]", GUILayout.MinWidth(10)))
                {
                    Repo.currencies[newCurrencyName] = 0;
                }

                EditorGUILayout.EndHorizontal();
            }

            Repo.Save();
        }
    }
}