using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Pickup : MonoBehaviour
{
	[Header("Internal References")]
	[SerializeField]
	Transform MeshTransform = null;
	[SerializeField]
	VisibilityNotifier Visibility = null;

	[Header("External References")]
	[SerializeField]
	GameObject CollectEffectPrefab = null;

	SphereCollider Collider;
	PickupCollectEffect Effect;

	protected PickupManager Manager;

	void Awake()
	{
		Collider = GetComponent<SphereCollider>();
		Collider.isTrigger = true;

		Visibility.BecameVisible += Visibility_BecameVisible;
	}

	void Start()
	{
		Effect = Instantiate(CollectEffectPrefab).GetComponent<PickupCollectEffect>();

		OnStart();
	}

	void OnDestroy()
	{
		Visibility.BecameVisible -= Visibility_BecameVisible;
	}

	void Update()
	{
		ProcessRotation();
		Process();
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player")) Collect(other.GetComponent<GameManager>());
	}

	void Visibility_BecameVisible(bool Visible)
	{
		enabled = Visible;
	}

	void ProcessRotation()
	{
		MeshTransform.Rotate(0, 90 * Time.deltaTime, 0);
	}

	public void Initialize(PickupManager Manager)
	{
		this.Manager = Manager;
	}

	protected virtual void OnStart()
	{

	}

	protected virtual void Collect(GameManager PlayerGameManager)
	{
		Effect.transform.position = MeshTransform.position;
		Effect.PlayEffect();
		Destroy(gameObject);
	}

	protected virtual void Process()
	{

	}
}