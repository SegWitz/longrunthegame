using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PickupCollectEffect : MonoBehaviour
{
	[SerializeField]
	ParticleSystem PS = null;

	AudioSource Audio;

	void Awake()
	{
		Audio = GetComponent<AudioSource>();
		enabled = false;
	}

	void Update()
	{
		if (!PS.isPlaying) Destroy(gameObject);
	}

	public void PlayEffect()
	{
		Audio.Play();
		PS.Play();
		enabled = true;
	}
}