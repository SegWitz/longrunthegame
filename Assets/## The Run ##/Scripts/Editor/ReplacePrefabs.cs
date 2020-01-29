using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReplacePrefabs : EditorWindow
{
	GameObject Prefab;
	bool PreserveScale = true;
	bool RoundScale = true;
	bool PreserveRotation = true;
	Vector3 PositionOffset = Vector3.zero;
	Vector3 RotationOffset = Vector3.zero;
	bool DeleteOriginal = true;

	[MenuItem("Scene/Replace Selected Prefabs...")]
	static void OpenWindow()
	{
		GetWindow<ReplacePrefabs>("Replace Prefabs");
	}

	void OnGUI()
	{
		Prefab = (GameObject)EditorGUILayout.ObjectField("New Prefab", Prefab, typeof(GameObject), false);

		EditorGUILayout.Space();

		PreserveScale = EditorGUILayout.Toggle("Preserve scale", PreserveScale);
		if (PreserveScale)
		{
			EditorGUI.indentLevel = 1;
			RoundScale = EditorGUILayout.Toggle("Round scale", RoundScale);
			EditorGUI.indentLevel = 0;
		}
		PreserveRotation = EditorGUILayout.Toggle("Preserve Rotation", PreserveRotation);

		EditorGUILayout.Space();

		PositionOffset = EditorGUILayout.Vector3Field("Position Offset", PositionOffset);
		RotationOffset = EditorGUILayout.Vector3Field("Rotation Offset", RotationOffset);

		EditorGUILayout.Space();

		DeleteOriginal = EditorGUILayout.Toggle("Delete Original", DeleteOriginal);

		EditorGUILayout.Space();

		if (GUILayout.Button("Replace"))
		{
			if (Prefab == null)
			{
				Debug.LogError("No prefab has been specified.");
				return;
			}

			Replace();
		}
	}

	void Replace()
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

				Vector3 Position = selection[n].transform.localPosition + (selection[n].transform.localRotation * PositionOffset);
				Quaternion Rotation = selection[n].transform.localRotation * Quaternion.Euler(RotationOffset);
				Transform Parent = selection[n].transform.parent;

				if (DeleteOriginal) DestroyImmediate(selection[n]);

				GameObject Instance = PrefabUtility.InstantiatePrefab(Prefab) as GameObject;
				Instance.transform.parent = Parent;
				Instance.transform.localPosition = Position;
				Instance.transform.localRotation = Rotation;
				if (PreserveScale) Instance.transform.localScale = Scale;
			}

			EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

			if (!Error) Debug.Log("All prefabs replaced successfully.");
		}
		else
		{
			Debug.Log("Cannot replace prefabs, nothing selected.");
		}
	}
}