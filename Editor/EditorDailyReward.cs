using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(DailyReward))]
public class EditorDailyReward : Editor
{
    private DailyReward it;

    public void OnEnable()
    {
        it = (DailyReward)target;
    }
    public override void OnInspectorGUI()
    {
        DrawElement(serializedObject.FindProperty("notInternetConnection"), new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter }, "Not Internet Connection!");

        DrawElement(serializedObject.FindProperty("httpError"), new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter }, "Http Error!");

        DrawElement(serializedObject.FindProperty("rewardEarnedToday"), new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter }, "Reward Earned Today!");

        ItemSlotGUI();

        if (GUI.changed) SetObjectDirty(it.gameObject);
    }

    private void DrawElement(SerializedProperty serializedEvent, GUIStyle style, string LabelField)
    {
        EditorGUILayout.BeginVertical("HelpBox");
        EditorGUILayout.LabelField(LabelField, style);
        serializedObject.Update();
        EditorGUILayout.BeginVertical("Box");
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(serializedEvent, new GUIContent(LabelField));
        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.EndVertical();
        GUILayout.Space(10);
    }

    private void ItemSlotGUI()
    {
        EditorGUILayout.BeginVertical("HelpBox");
        EditorGUILayout.LabelField("Every day's event!", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter });
        if (it.eventsDays.Count - 1 > 0)
        {
            int day = 1;
            serializedObject.Update();
            for (int i = 0; i < it.eventsDays.Count - 1; i++)
            {
                EditorGUILayout.BeginVertical("Box");
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("X", GUILayout.Height(25), GUILayout.Width(25)))
                {
                    it.eventsDays.RemoveAt(i);
                    return;
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("eventsDays").GetArrayElementAtIndex(i), new GUIContent($"Event Day {day}"));
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
                GUILayout.Space(10);
                day++;
            }
            serializedObject.ApplyModifiedProperties();
        }
        if (GUILayout.Button("Add Day", GUILayout.Height(30)))
            it.eventsDays.Add(null);
        EditorGUILayout.EndVertical();
    }

    public static void SetObjectDirty(GameObject obj)
    {
        EditorUtility.SetDirty(obj);
        EditorSceneManager.MarkSceneDirty(obj.scene);
    }
}