using UnityEngine;

public class BP_FootstepSounds : MonoBehaviour
{
	public float audioPositionAdjust = -0.5f;               // Used to tweak the local y position of the AudioSources which get spawned and attached to the character.
	public AudioClip defaultFootstepSound;                  // Play the following if we are walking on a texture that is not assigned a footstep sound.
	public float defaultPitchMin = 0.75f;                   // Minimum pitch applied to the 'random.range' for variation in the footstep sound.
	public float defaultPitchMax = 1.0f;                    // Same as above but for the maximum value in the 'random.range'.

	AnimationBlending Blending;

	AudioSource[] AudioSources;
	int CurrentAudioSource = 0;

	void Awake()
	{
		Blending = GetComponent<AnimationBlending>();

		AudioSources = new AudioSource[2];
		AudioSources[0] = InstantiateAudioSource("Footstep Audio Source 1");
		AudioSources[1] = InstantiateAudioSource("Footstep Audio Source 2");
	}

	AudioSource InstantiateAudioSource(string Name)
	{
		GameObject GO = new GameObject();
		GO.transform.parent = gameObject.transform;
		GO.transform.localPosition = new Vector3(0f, audioPositionAdjust, 0f);
		GO.name = Name;
		AudioSource AS = GO.AddComponent<AudioSource>();
		AS.playOnAwake = false;

		return AS;
	}

	//This gets called from animation events
	void PlayFootstep(int Direction)
	{
		Vector2 MovementDirection = Blending.MovementDirection;
		float D = Vector2.Dot(MovementDirection, Vector2.up);

		if (D > 0.3333f && Direction < 1) return;
		if (D < 0.3333f && D > -0.3333f && Direction != 0) return;
		if (D < -0.3333f && Direction > 1) return;

		AudioSources[CurrentAudioSource].pitch = Random.Range(defaultPitchMin, defaultPitchMax);
		AudioSources[CurrentAudioSource].clip = defaultFootstepSound;
		AudioSources[CurrentAudioSource].Play();

		CurrentAudioSource ^= 1;
	}
}