using UnityEngine;
using System.Collections;

namespace SgLib
{
	[RequireComponent(typeof(AudioSource))]
	public class SoundManager : MonoBehaviour
	{
		[System.Serializable]
		public class Sound
		{
			public AudioClip clip;
			[HideInInspector]
			public int simultaneousPlayCount = 0;
		}

		[Header("Max number allowed of same sounds playing together")]
		public int maxSimultaneousSounds = 7;

		// List of sounds used in this game
		public Sound background;
		public Sound button;
		public Sound cloudHit;
		public Sound coin;
		public Sound gameOver;
		public Sound tick;
		public Sound rewarded;
		public Sound unlock;

		static SoundManager _Instance;
		public static SoundManager Instance
		{
			get
			{
				if (_Instance == null) _Instance = FindObjectOfType<SoundManager>();
				return _Instance;
			}
		}

		enum PlayingState
		{
			Playing,
			Paused,
			Stopped
		}

		public AudioSource AudioSource
		{
			get
			{
				if (_audioSource == null)
				{
					_audioSource = GetComponent<AudioSource>();
				}

				return _audioSource;
			}
		}

		private AudioSource _audioSource;
		private PlayingState musicState = PlayingState.Stopped;


		/// <summary>
		/// Plays the given sound with option to progressively scale down volume of multiple copies of same sound playing at
		/// the same time to eliminate the issue that sound amplitude adds up and becomes too loud.
		/// </summary>
		/// <param name="sound">Sound.</param>
		/// <param name="autoScaleVolume">If set to <c>true</c> auto scale down volume of same sounds played together.</param>
		/// <param name="maxVolumeScale">Max volume scale before scaling down.</param>
		public void PlaySound(Sound sound, bool autoScaleVolume = true, float maxVolumeScale = 1f)
		{
			StartCoroutine(CRPlaySound(sound, autoScaleVolume, maxVolumeScale));
		}

		IEnumerator CRPlaySound(Sound sound, bool autoScaleVolume = true, float maxVolumeScale = 1f)
		{
			if (sound.simultaneousPlayCount >= maxSimultaneousSounds)
			{
				yield break;
			}

			sound.simultaneousPlayCount++;

			float vol = maxVolumeScale;

			// Scale down volume of same sound played subsequently
			if (autoScaleVolume && sound.simultaneousPlayCount > 0)
			{
				vol = vol / (float)(sound.simultaneousPlayCount);
			}

			AudioSource.PlayOneShot(sound.clip, vol);

			// Wait til the sound almost finishes playing then reduce play count
			float delay = sound.clip.length * 0.7f;

			yield return new WaitForSeconds(delay);

			sound.simultaneousPlayCount--;
		}

		/// <summary>
		/// Plays the given music.
		/// </summary>
		/// <param name="music">Music.</param>
		/// <param name="loop">If set to <c>true</c> loop.</param>
		public void PlayMusic(Sound music, bool loop = true)
		{
			AudioSource.clip = music.clip;
			AudioSource.loop = loop;
			AudioSource.Play();
			musicState = PlayingState.Playing;
		}

		/// <summary>
		/// Pauses the music.
		/// </summary>
		public void PauseMusic()
		{
			if (musicState == PlayingState.Playing)
			{
				AudioSource.Pause();
				musicState = PlayingState.Paused;
			}
		}

		/// <summary>
		/// Resumes the music.
		/// </summary>
		public void ResumeMusic()
		{
			if (musicState == PlayingState.Paused)
			{
				AudioSource.UnPause();
				musicState = PlayingState.Playing;
			}
		}

		/// <summary>
		/// Stop music.
		/// </summary>
		public void StopMusic()
		{
			AudioSource.Stop();
			musicState = PlayingState.Stopped;
		}

		void SetMute(bool isMuted)
		{
			AudioSource.mute = isMuted;
		}
	}
}