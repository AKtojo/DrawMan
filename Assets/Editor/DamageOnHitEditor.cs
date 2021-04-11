using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace DrawMan.DamageSystem.Editor
{
    [CustomEditor(typeof(DamageOnHit))] [CanEditMultipleObjects]
    public class DamageOnHitEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((DamageOnHit)target), typeof(DamageOnHit), false);
            GUI.enabled = true;

            var damager = target as DamageOnHit;
            
            LayerMask mask = EditorGUILayout.MaskField("Damageable Layers", InternalEditorUtility.LayerMaskToConcatenatedLayersMask(damager.Damageable), InternalEditorUtility.layers);
            damager.Damageable = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(mask);

            damager.KillInstantly = EditorGUILayout.Toggle("Kill Instantly", damager.KillInstantly);;

            if (!damager.KillInstantly)
            {
                damager.Damage = EditorGUILayout.FloatField("Damage Dealt", damager.Damage);
            }
        }
    }
}
