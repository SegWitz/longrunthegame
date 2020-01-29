using UnityEngine;

public class Inventory : MonoBehaviour
{
	[SerializeField]
	protected Transform CellsParent = null;
	[SerializeField]
	protected GameObject CellPrefab = null;
	[SerializeField]
	Animator AnimatorComponent = null;

	[Space]
	[SerializeField]
	bool IsShownByDefault = false;

	protected InventoryCell[] Cells;

	protected bool HasBeenInitialized;

	protected virtual void OnEnable()
	{
		if (!HasBeenInitialized)
		{
			if (IsShownByDefault) Show(true);
			HasBeenInitialized = true;
		}

		LoadInventory();
	}

	protected void InstantiateCells(int Instances)
	{
		Cells = new InventoryCell[Instances];

		for (int c = 0; c < Instances; c++)
		{
			Cells[c] = Instantiate(CellPrefab).GetComponent<InventoryCell>();
			Cells[c].transform.SetParent(CellsParent, false);
		}
	}

	public virtual void LoadInventory()
	{

	}

	#region Transitions

	int ShowID = Animator.StringToHash("Show");
	int HideID = Animator.StringToHash("Hide");

	public void Show(bool Value)
	{
		if (Value)
			AnimatorComponent.Play(ShowID);
		else
			AnimatorComponent.Play(HideID);
	}

	#endregion
}