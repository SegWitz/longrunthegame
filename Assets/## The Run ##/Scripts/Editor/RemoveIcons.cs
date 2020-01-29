using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RemoveIcons : EditorWindow
{
	[MenuItem("Scene/Remove Icons...")]
	static void OpenWindow()
	{
		GetWindow<RemoveIcons>("Remove Icons");
	}

	void OnGUI()
	{
		if (GUILayout.Button("Remove Icons"))
		{
			Remove();
		}
	}

	void Remove()
	{
		Transform SelectedTransform = Selection.activeGameObject.transform;
		int ChildCount = SelectedTransform.childCount;

		GameObject[] Children = new GameObject[ChildCount];
		for (int c = 0; c < ChildCount; c++)
		{
			Children[c] = SelectedTransform.GetChild(c).gameObject;
		}

		for (int c = 0; c < Children.Length; c++)
		{
			string Name = Children[c].name;
			Vector3 Position = Children[c].transform.position;
			Transform Parent = Children[c].transform.parent;

			GameObject GO = new GameObject(Name);
			GO.transform.parent = Parent;
			GO.transform.position = Position;
		}

		for (int c = 0; c < Children.Length; c++)
		{
			DestroyImmediate(Children[c]);
		}

		if (Children.Length > 0) EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
	}
}