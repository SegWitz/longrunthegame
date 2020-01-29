using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RevertPrefabs : EditorWindow
{
	bool PreserveScale = true;
	bool RoundScale = true;
	bool Reconnect = false;

	[MenuItem("Scene/Revert Selected Prefabs...")]
	static void OpenWindow()
	{
		GetWindow<RevertPrefabs>("Revert Prefabs");
	}

	void OnGUI()
	{
		PreserveScale = EditorGUILayout.Toggle("Preserve scale", PreserveScale);
		if (PreserveScale)
		{
			EditorGUI.indentLevel = 1;
			RoundScale = EditorGUILayout.Toggle("Round scale", RoundScale);
			EditorGUI.indentLevel = 0;
		}
		Reconnect = EditorGUILayout.Toggle("Try to reconnect to prefab", Reconnect);
		EditorGUILayout.Space();

		if (GUILayout.Button("Revert"))
		{
			Revert();
		}
	}

	void Revert()
	{
		bool Error = false;

		GameObject[] selection = Selection.gameObjects;

		if (selection.Length > 0)
		{
			for (int n = 0; n < selection.Length; n++)
			{
				Vector3 Scale = Vector3.one;
				if (PreserveScale)
				{
					Scale = selection[n].transform.localScale;
					if (RoundScale)
					{
						Scale *= 100;
						Scale = new Vector3(Mathf.Round(Scale.x), Mathf.Round(Scale.y), Mathf.Round(Scale.z));
						Scale *= 0.01f;
					}
				}

				bool Result = PrefabUtility.RevertPrefabInstance(selection[n]);

				if (!Result && Reconnect)
				{
					bool Result2 = PrefabUtility.ReconnectToLastPrefab(selection[n]);
					if (!Result2)
					{
						Debug.LogError("This isn't a valid prefab: " + selection[n].name, selection[n]);
						Error = true;
						continue;
					}
				}

				if (PreserveScale) selection[n].transform.localScale = Scale;
			}

			EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

			if (!Error) Debug.Log("All prefabs reverted successfully.");
		}
		else
		{
			Debug.Log("Cannot revert to prefab, nothing selected.");
		}
	}
}