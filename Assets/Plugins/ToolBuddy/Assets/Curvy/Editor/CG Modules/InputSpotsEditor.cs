// =====================================================================
// Copyright 2013-2022 ToolBuddy
// All rights reserved
// 
// http://www.toolbuddy.net
// =====================================================================

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using FluffyUnderware.Curvy.Generator;
using FluffyUnderware.Curvy.Generator.Modules;
using UnityEditorInternal;

namespace FluffyUnderware.CurvyEditor.Generator.Modules
{
    [CustomEditor(typeof(InputSpots))]
    public class InputSpotsEditor : CGModuleEditor<InputSpots>
    {

        protected override void SetupArrayEx(DevToolsEditor.DTFieldNode node, DevTools.ArrayExAttribute attribute)
        {
            base.SetupArrayEx(node, attribute);
            node.ArrayEx.elementHeight *= 4;
            node.ArrayEx.drawElementCallback = OnSpotGUI;
            node.ArrayEx.onAddCallback = (ReorderableList l) =>
            {
                int spotsCount = Target.Spots.Count;

                //value of -1 means nothing selected previously
                var selectedIndex = l.index;
                //when deleting all entries, while having element 0 selected, l.index would be equal to 0, so to handle this:
                selectedIndex = selectedIndex < spotsCount ? selectedIndex : -1;

                CGSpot newSpot;
                int insertionIndex;
                if (selectedIndex < 0)
                {
                    newSpot = new CGSpot(0, Vector3.zero, Quaternion.identity, Vector3.one);
                    insertionIndex = Mathf.Max(spotsCount - 1, 0);
                }
                else
                {
                    newSpot = Target.Spots[selectedIndex];
                    insertionIndex = selectedIndex;
                }

                Target.Spots.Insert(insertionIndex, newSpot);
                EditorUtility.SetDirty(Target);
            };
        }


        void OnSpotGUI(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty prop = serializedObject.FindProperty(string.Format("m_Spots.Array.data[{0}]", index.ToString()));
            rect.height = EditorGUIUtility.singleLineHeight;
            if (prop != null)
            {
                EditorGUIUtility.labelWidth = 40;
                Rect r = new Rect(rect);
                GUI.Label(new Rect(r.x, r.y, 30, r.height), "#" + index.ToString());
                EditorGUI.PropertyField(new Rect(r.x + 30, r.y, 115, r.height), prop.FindPropertyRelative("m_Index"));

                EditorGUIUtility.labelWidth = 55;
                r.y += r.height + 1;
                EditorGUI.PropertyField(r, prop.FindPropertyRelative("m_Position"));
                r.y += r.height + 1;
                EditorGUI.PropertyField(r, prop.FindPropertyRelative("m_Rotation"));
                r.y += r.height + 1;
                EditorGUI.PropertyField(r, prop.FindPropertyRelative("m_Scale"));
                if (serializedObject.ApplyModifiedProperties())
                    Target.Dirty = true;
            }
        }

        Vector2 scroll;


        public override void OnInspectorGUI()
        {
            scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.Height(210));
            base.OnInspectorGUI();
            EditorGUILayout.EndScrollView();
            if (GUILayout.Button("Clear") && EditorUtility.DisplayDialog("Clear List", "Are you sure?", "Yes", "No"))
                Target.Spots.Clear();
        }

    }

}