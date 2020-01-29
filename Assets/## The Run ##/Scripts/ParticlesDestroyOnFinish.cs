using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticlesDestroyOnFinish : MonoBehaviour
{
	ParticleSystem ps;
	bool OldIsPlaying;

	private void Awake()
	{
		ps = GetComponent<ParticleSystem>();
		OldIsPlaying = false;
	}

	void Update()
	{
		bool IsPlaying = ps.isPlaying;

		if (OldIsPlaying && !IsPlaying)
		{
			Destroy(gameObject);
		}

		OldIsPlaying = IsPlaying;
	}
}