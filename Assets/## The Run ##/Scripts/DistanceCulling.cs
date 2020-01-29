using UnityEngine;

[RequireComponent(typeof(Camera))]
public class DistanceCulling : MonoBehaviour
{
	[SerializeField]
	float FireLayerDistance = 10f;
	[SerializeField]
	float SmallPropDistance = 15f;
	[SerializeField]
	float MediumPropDistance = 30f;
	[SerializeField]
	float BigPropDistance = 60f;
	[SerializeField]
	float PickupDistance = 60f;

	void Start()
	{
		Camera camera = GetComponent<Camera>();
		float[] distances = new float[32];
		distances[8] = FireLayerDistance;
		distances[9] = SmallPropDistance;
		distances[10] = MediumPropDistance;
		distances[11] = BigPropDistance;
		distances[12] = PickupDistance;
		camera.layerCullDistances = distances;
	}
}