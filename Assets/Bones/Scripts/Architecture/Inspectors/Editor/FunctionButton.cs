using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Bones.Scripts.Architecture.Inspectors.Editor
{
    class MethodData
    {
        public bool HideParameters;
        public ParameterInfo[] MethodParameters;
        public object[] ParametersValues;
    }

    [CustomEditor(typeof(MonoBehaviour), true)]
    public class ButtonOnMB : FunctionButton
    {
    }

    [CustomEditor(typeof(ScriptableObject), true)]
    public class ButtonOnSO : FunctionButton
    {
    }

    public class FunctionButton : UnityEditor.Editor
    {
        private static readonly IReadOnlyList<Type> ValidParameters = new List<Type>
            {typeof(string), typeof(int), typeof(float), typeof(Vector2)};

        private readonly Dictionary<MethodInfo, MethodData> dataForInfo = new Dictionary<MethodInfo, MethodData>();

        private void OnEnable()
        {
            dataForInfo.Clear();
            GetMethods().ForEach(InitMethod);
        }

        private void OnDisable()
        {
            dataForInfo.Clear();
        }

        private void InitMethod(MethodInfo method)
        {
            var parameters = method.GetParameters();
            var methodData = new MethodData
            {
                ParametersValues = parameters
                    .Select(GetDefault)
                    .ToArray(),
                MethodParameters = parameters,
                HideParameters = true
            };
            dataForInfo[method] = methodData;
        }

        private static object GetDefault(ParameterInfo parameter)
        {
            var type = parameter.ParameterType;
            if (type == typeof(string)) return "";
            if (type == typeof(int)) return 0;
            if (type == typeof(float)) return 0.0f;
            return null;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GetMethods().ForEach(DrawMethod);
        }

        private List<MethodInfo> GetMethods()
        {
            return target
                .GetType()
                .GetRuntimeMethods()
                .Where(method => method.GetCustomAttribute<AsButton>() != null)
                .Where(method => method.GetParameters().All(IsValidParameter))
                //.Where(method => !method.IsAbstract)
                .Where(method => !method.IsConstructor)
                .Where(method => !method.IsGenericMethod)
                .Where(method => method.ReturnType == typeof(void))
                .ToList();
        }

        private void DrawMethod(MethodInfo method)
        {
            var parametersLength = method.GetParameters().Length;
            if (parametersLength == 0) DrawParameterLessMethod(method);
            if (parametersLength == 1) DrawSingleParamMethod(method);
            if (parametersLength > 1) DrawMultipleParametersMethod(method);
            EditorGUILayout.Space(15);
        }

        private void DrawParameterLessMethod(MethodInfo method)
        {
            var methodName = method.Name;
            EditorGUILayout.BeginHorizontal();
            var maxWidth = GUILayout.MaxWidth(Mathf.Max(200, methodName.Length * 5));
            DrawMethodButton(method, maxWidth);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(5);
        }

        private void DrawSingleParamMethod(MethodInfo method)
        {
            var methodName = method.Name;
            var maxWidth = GUILayout.MaxWidth(Mathf.Max(200, methodName.Length * 5));
            EditorGUILayout.BeginHorizontal();
            DrawMethodButton(method, maxWidth);
            EditorGUILayout.LabelField(method.GetParameters()[0].Name);
            DrawParameterField(method, 0);
            EditorGUILayout.EndHorizontal();
        }

        private void DrawMethodButton(MethodInfo method, params GUILayoutOption[] options)
        {
            var data = dataForInfo[method];
            if (GUILayout.Button(method.Name, options)) method.Invoke(target, data.ParametersValues);
        }

        private void DrawMultipleParametersMethod(MethodInfo method)
        {
            var methodName = method.Name;
            var maxWidth = GUILayout.MaxWidth(Mathf.Max(200, methodName.Length * 5));
            EditorGUILayout.BeginHorizontal();
            DrawMethodButton(method, maxWidth);
            EditorGUILayout.EndHorizontal();

            var data = dataForInfo[method];
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("parameters:");
            InspectorsUtilities.ExpandButton(ref data.HideParameters);
            EditorGUILayout.EndHorizontal();

            if (!data.HideParameters)
                for (var i = 0; i < data.MethodParameters.Length; ++i)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(data.MethodParameters[i].Name);
                    DrawParameterField(method, i);
                    EditorGUILayout.EndHorizontal();
                }
        }


        private void DrawParameterField(MethodInfo method, int index)
        {
            var data = dataForInfo[method];
            var param = data.MethodParameters[index];

            if (param.ParameterType == typeof(string))
            {
                var value = (string) data.ParametersValues[index];
                value = EditorGUILayout.TextField(value);
                data.ParametersValues[index] = value;
            }

            if (param.ParameterType == typeof(int))
            {
                var value = (int) data.ParametersValues[index];
                value = EditorGUILayout.IntField(value);
                data.ParametersValues[index] = value;
            }

            if (param.ParameterType == typeof(float))
            {
                var value = (float) data.ParametersValues[index];
                var range = param.GetCustomAttribute<ParamRange>();
                value = range != null
                    ? EditorGUILayout.Slider(value, range.min, range.max)
                    : EditorGUILayout.FloatField(value);
                data.ParametersValues[index] = value;
            }
        }

        private bool IsValidParameter(ParameterInfo parameter)
        {
            foreach (var validParameter in ValidParameters)
                if (validParameter == parameter.ParameterType)
                    return true;

            return false;
        }
    }
}