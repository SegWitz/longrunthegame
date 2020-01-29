using UnityEngine;

public class PickupManager : MonoBehaviour
{
	[Header("Prefabs")]
	[SerializeField]
	GameObject ShieldPrefab = null;

	[Header("References")]
	[SerializeField]
	Transform SpawnPositionsParent = null;

	[Header("Probabilities")]
	[SerializeField, Range(1, 100)]
	int MoneyProbability = 31;
	[SerializeField, Range(1, 100)]
	int CrystalsProbability = 5;
	[SerializeField, Range(1, 100)]
	int PerksProbability = 3;
	[SerializeField]
	PowerupSpawnInfo[] Powerups = null;

	MoneyData MoneyData;
	CrystalsData CrystalsData;
	PerksData PerksData;
	PowerupsData PowerupsData;

	int SpawnPositionsCount;
	Vector3[] SpawnPositions;
	GameManager _PlayerGameManager;
	public GameManager PlayerGameManager
	{
		get
		{
			if (_PlayerGameManager == null) _PlayerGameManager = GameObject.FindGameObjectWithTag("Player").GetComponent<GameManager>();
			return _PlayerGameManager;
		}
	}
	float _CharacterScale;
	public float CharacterScale { get { return _CharacterScale; } }

	void Start()
	{
		InitializeReferences();
		InitializeSpawnPositions();
		GetCharacterScale();
		SpawnPickups();
		SpawnShields();
	}

	void Update()
	{
		ProcessMagnet();
	}

	void InitializeReferences()
	{
		MoneyData = TheRunGameManager.Instance.MoneyData;
		CrystalsData = TheRunGameManager.Instance.CrystalsData;
		PerksData = TheRunGameManager.Instance.PerksData;
		PowerupsData = TheRunGameManager.Instance.PowerupsData;
	}

	void InitializeSpawnPositions()
	{
		SpawnPositionsCount = SpawnPositionsParent.childCount;
		SpawnPositions = new Vector3[SpawnPositionsCount];
		for (int t = 0; t < SpawnPositionsCount; t++)
		{
			SpawnPositions[t] = SpawnPositionsParent.GetChild(t).position;
			SpawnPositions[t].y = 0;
		}

		//Reorder the list of spawn positions randomly
		for (var i = 0; i < SpawnPositionsCount; i++)
		{
			int j = Random.Range(i, SpawnPositionsCount);
			Vector3 temp = SpawnPositions[i];
			SpawnPositions[i] = SpawnPositions[j];
			SpawnPositions[j] = temp;
		}
	}

	void GetCharacterScale()
	{
		_CharacterScale = PlayerGameManager.transform.localScale.y;
	}

	void SpawnPickups()
	{
		// Money -------------------------------------------------------------------------------------------------

		int CurrentSpawnIndex = 0;
		int MoneyPickupsCount = SpawnPositionsCount * MoneyProbability / 100;
		for (int p = CurrentSpawnIndex; p < MoneyPickupsCount; p++)
		{
			int Index = Random.Range(0, MoneyData.Data.Length);
			Spawn(MoneyData.Data[Index].PickupPrefab, p);
		}

		// Crystals ----------------------------------------------------------------------------------------------

		CurrentSpawnIndex += MoneyPickupsCount;
		int CrystalsPickupsCount = SpawnPositionsCount * CrystalsProbability / 100;
		for (int p = CurrentSpawnIndex; p < CrystalsPickupsCount + CurrentSpawnIndex; p++)
		{
			int Index = Random.Range(0, CrystalsData.Data.Length);
			Spawn(CrystalsData.Data[Index].PickupPrefab, p);
		}

		// Perks -------------------------------------------------------------------------------------------------

		CurrentSpawnIndex += CrystalsPickupsCount;
		int PerksPickupsCount = SpawnPositionsCount * PerksProbability / 100;
		for (int p = CurrentSpawnIndex; p < PerksPickupsCount + CurrentSpawnIndex; p++)
		{
			int Index = Random.Range(0, PerksData.Data.Length);
			Spawn(PerksData.Data[Index].PickupPrefab, p);
		}

		// Power-ups ---------------------------------------------------------------------------------------------

		CurrentSpawnIndex += PerksPickupsCount;

		for (int pw = 0; pw < Powerups.Length; pw++)
		{
			int PowerupPickupsCount = SpawnPositionsCount * Powerups[pw].Probability / 100;
			for (int p = CurrentSpawnIndex; p < PowerupPickupsCount + CurrentSpawnIndex; p++)
			{
				Spawn(PowerupsData.Data[(int)Powerups[pw].PowerupID].PickupPrefab, p);
			}

			CurrentSpawnIndex += PowerupPickupsCount;
		}
	}

	void Spawn(GameObject Prefab, int SpawnPositionIndex)
	{
		if (SpawnPositionIndex >= SpawnPositions.Length) return;

		GameObject Instance = Instantiate(Prefab, SpawnPositions[SpawnPositionIndex], Quaternion.identity);
		Instance.transform.localScale = new Vector3(_CharacterScale, _CharacterScale, _CharacterScale);
		Pickup Pickup = Instance.GetComponent<Pickup>();
		Pickup.Initialize(this);
	}

	#region Magnet

	float MagnetTimer;
	public bool IsMagnetEnabled { get { return MagnetTimer > 0; } }

	public void InitializeMagnet()
	{
		MagnetTimer = 60;
	}

	void ProcessMagnet()
	{
		if (!IsMagnetEnabled) return;


	}

	#endregion

	#region Shields

	Shield Shields;

	void SpawnShields()
	{
		GameObject GO = Instantiate(ShieldPrefab);
		Shields = GO.GetComponent<Shield>();
		Shields.InitializeShield(PlayerGameManager);
	}

	public void ActivateShields()
	{
		Shields.Enable(true);
	}

	#endregion

	#region Invisibility

	//public void ActivateInvisibility()
	//{
	//	PlayerGameManager.SetInvisibility(6);
	//}

	#endregion

	#region Debug

#if UNITY_EDITOR

	void OnDrawGizmos()
	{
		int ChildCount = SpawnPositionsParent.childCount;
		Transform CameraTransform = Camera.current.transform;
		Vector3 CameraPosition = CameraTransform.position;
		Vector3 CameraForward = CameraTransform.forward;

		for (int i = 0; i < ChildCount; i++)
		{
			Transform Child = SpawnPositionsParent.GetChild(i);
			GameObject GO = Child.gameObject;

			if (!GO.activeInHierarchy) continue;

			float Distance = UnityEditor.HandleUtility.GetHandleSize(Child.position);
			if (Distance > 2.5f) continue;

			bool IsSelected = UnityEditor.Selection.Contains(GO);

			Gizmos.color = IsSelected ? Color.cyan : Color.blue;
			Gizmos.DrawWireSphere(Child.position, 0.2f);

			if (Vector3.Dot(CameraForward, (Child.position - CameraPosition).normalized) > 0.75f)
				UnityEditor.Handles.Label(Child.position, Child.name);
		}
	}

#endif

	#endregion
}

[System.Serializable]
public class SpawnInfo
{
	[Range(1, 100)]
	public int Probability;
}

[System.Serializable]
public class PowerupSpawnInfo : SpawnInfo
{
	public PowerupsInfo.PowerupsIDs PowerupID;
}