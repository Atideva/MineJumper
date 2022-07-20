#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Gameplay
{
    [CustomEditor(typeof(CurveList))]
    public class CurveListEditor : Editor
    {
        private Vector2 _scroll;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            var script = (CurveList) target;
            _scroll = EditorGUILayout.BeginScrollView(_scroll, GUILayout.MaxHeight(300));

            for (var i = 0; i < script.Curves.Count; i++)
            {
                EditorGUILayout.BeginHorizontal("box");
                EditorGUILayout.LabelField("Curve " + i);

                var chance = (int) (script.GetChance(i) * 100f);

                EditorGUILayout.LabelField(chance + "%");
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
        }
    }
}

#endif