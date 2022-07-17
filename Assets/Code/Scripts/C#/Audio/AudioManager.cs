using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour, IAudioManager
{
	[Header("Audio Player")] 
	
	[SerializeField] private AudioSource sfxAudioSource;
	
	[SerializeField] private AudioSource bgmAudioSource;
	
	[Header("Random SFX Pitch")]
	
	[SerializeField] private float lowPitchRange = .95f;
	
	[SerializeField] private float highPitchRange = 1.05f;
	
	public static AudioManager Instance = null;
	
	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}
		DontDestroyOnLoad (gameObject);
	}

	public void PlaySFX(AudioClip clip)
	{
		sfxAudioSource.PlayOneShot(clip);
	}
	
	public void PlayBGM(AudioClip clip)
	{
		bgmAudioSource.clip = clip;
		bgmAudioSource.loop = true;
		bgmAudioSource.Play();
	}

	public void PlayRandomSFX(params AudioClip[] clips)
	{
		int randomIndex = Random.Range(0, clips.Length);
		float randomPitch = Random.Range(lowPitchRange, highPitchRange);
		sfxAudioSource.pitch = randomPitch;
		sfxAudioSource.clip = clips[randomIndex];
		sfxAudioSource.Play();
	}

	public void StopBGM()
	{
		bgmAudioSource.Stop();
	}
}
