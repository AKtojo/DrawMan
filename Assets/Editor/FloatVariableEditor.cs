using UnityEngine;
using UnityEditor;

namespace DrawMan.Core.Variables.Editor
{
    [CustomEditor(typeof(FloatVariable))] [CanEditMultipleObjects]
    public class FloatVariableEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject((FloatVariable)target), typeof(FloatVariable), false);
            GUI.enabled = true;

            var floatVar = target as FloatVariable;

            floatVar.Max = EditorGUILayout.FloatField("Max Value", floatVar.Max);

            floatVar.Clamped = EditorGUILayout.Toggle("Clamped", floatVar.Clamped);

            if (floatVar.Clamped)
            {
                floatVar.Min = EditorGUILayout.FloatField("Min Value", floatVar.Min);
            }

            GUI.enabled = false;
            EditorGUILayout.FloatField("Current", floatVar.Value);
            GUI.enabled = true;

            EditorUtility.SetDirty(target);
        }
    }
}
