using UnityEngine;
using UnityEditor;
using Environment;

[CustomEditor(typeof(SeasonManager))]
public class SeasonManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {

        serializedObject.Update();

        SerializedProperty sp = serializedObject.FindProperty("seasons");

        for (int i = 0; i < 4; i++)
        {
            string name = ((SeasonManager.Season)i).ToString();
            EditorGUILayout.PropertyField(sp.GetArrayElementAtIndex(i), new GUIContent(name));
        }

        SeasonManager manager = (SeasonManager)target;
        SeasonData data = manager.RuntimeData;

        if (data != null)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Runtime Data");

            SerializedObject so = new SerializedObject(data);
            so.Update();

            EditorGUILayout.PropertyField(so.FindProperty("AvgTemp"));
            EditorGUILayout.PropertyField(so.FindProperty("DayLength"));
            EditorGUILayout.PropertyField(so.FindProperty("SunColor"));

            so.ApplyModifiedProperties();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
