using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveWithoutAffectingChildren : EditorWindow
{
	Vector3 NewPosition = Vector3.zero;

	[MenuItem("Scene/Move Without Affecting Children...")]
	static void OpenWindow()
	{
		GetWindow<MoveWithoutAffectingChildren>("Move");
	}

	void OnGUI()
	{
		NewPosition = EditorGUILayout.Vector3Field("New Position", NewPosition);

		EditorGUILayout.Space();

		if (GUILayout.Button("Move"))
		{
			Move();
		}
	}

	void Move()
	{
		GameObject[] selection = Selection.gameObjects;

		for (int s = 0; s < selection.Length; s++)
		{
			Vector3[] ChildWorldPositions = new Vector3[selection[s].transform.childCount];

			for (int c = 0; c < ChildWorldPositions.Length; c++)
			{
				ChildWorldPositions[c] = selection[s].transform.GetChild(c).position;
			}

			selection[s].transform.position = NewPosition;

			for (int c = 0; c < ChildWorldPositions.Length; c++)
			{
				selection[s].transform.GetChild(c).position = ChildWorldPositions[c];
			}
		}

		if (selection.Length > 0) EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
	}
}