using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour, IAudioManager
{
	[Header("Audio Player")] 
	
	[SerializeField] 
	private AudioSource carAudioSource;
	
	[SerializeField] 
	private AudioSource sfxAudioSource;
	
	[SerializeField] 
	private AudioSource bgmAudioSource;
	
	[Header("Random SFX Pitch")]
	
	[SerializeField] 
	private float lowPitchRange = .95f;
	
	[SerializeField] 
	private float highPitchRange = 1.05f;
	
	private static AudioManager Instance = null;
	
	[Header("Game BGMs")]

	[SerializeField] 
	private AudioClip mainMenuBGM;
	
	[SerializeField] 
	private AudioClip mainGameBGM;

	[Header("User Interface SFX")] 
	
	[SerializeField] 
	private AudioClip[] clickSFX;

	[Header("Car Interaction")] 
	
	[SerializeField]
	private AudioClip carSpeedingUp;
	
	[SerializeField]
	private AudioClip[] carEngineRunning;
	
	[SerializeField]
	private AudioClip carFallingIntoTrap;
	
	[SerializeField]
	private AudioClip[] carSmashingBreak;
	
	[SerializeField]
	private AudioClip carEngineTurningOff;
	
	[Header("Stage SFX")]
	
	[SerializeField]
	private AudioClip trafficLightCountdown;
	
	[SerializeField]
	private AudioClip winning;

	[Header("Stage Editor SFX")] 
	
	[SerializeField]
	private AudioClip objectPlacement;
	
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
	
	private void PlayCarAudio(AudioClip clip)
	{
		carAudioSource.clip = clip;
		carAudioSource.Play();
	}

	private void PlaySfx(AudioClip clip)
	{
		sfxAudioSource.PlayOneShot(clip);
	}
	
	private void PlayBgm(AudioClip clip)
	{
		bgmAudioSource.clip = clip;
		bgmAudioSource.loop = true;
		bgmAudioSource.Play();
	}

	private void PlayRandomSfx(AudioClip[] clips)
	{
		int randomIndex = Random.Range(0, clips.Length);
		float randomPitch = Random.Range(lowPitchRange, highPitchRange);
		sfxAudioSource.pitch = randomPitch;
		sfxAudioSource.clip = clips[randomIndex];
		sfxAudioSource.Play();
	}
	
	private void PlayRandomCarSfx(AudioClip[] clips)
	{
		int randomIndex = Random.Range(0, clips.Length);
		carAudioSource.clip = clips[randomIndex];
		carAudioSource.Play();
	}

	public void StopBGM()
	{
		bgmAudioSource.Stop();
	}
	
	private void UnloadCarAudioSource()
	{
		carAudioSource.Stop();
	}
	
	public void PlayMainMenuBgm()
	{
		PlayBgm(mainMenuBGM);
	}
	
	public void PlayMainGameBgm()
	{
		PlayBgm(mainGameBGM);
	}

	public void PlayButtonClickSfx()
	{
		PlayRandomSfx(clickSFX);
	}
	
	public void PlayCarSpeedingUpSfx()
	{
		UnloadCarAudioSource();
		carAudioSource.loop = true;
		PlayCarAudio(carSpeedingUp);
	}
	
	public void PlayCarEngineRunningSfx()
	{
		UnloadCarAudioSource();
		carAudioSource.loop = true;
		PlayRandomCarSfx(carEngineRunning);
	}
	
	public void PlayCarCrashSfx()
	{
		UnloadCarAudioSource();
		carAudioSource.loop = false;
		PlayRandomCarSfx(carSmashingBreak);
	}
	
	public void PlayCarFallingIntoTrap()
	{
		UnloadCarAudioSource();
		carAudioSource.loop = false;
		PlayCarAudio(carFallingIntoTrap);
	}

	public void PlayCarEngineTurningOffSfx()
	{
		UnloadCarAudioSource();
		carAudioSource.loop = false;
		PlayCarAudio(carEngineTurningOff);
	}
	
	public void PlayTrafficLightCountdownSfx()
	{
		Invoke(nameof(PlayTrafficLightCountdownSfxAsync),1);
	}
	
	private void PlayTrafficLightCountdownSfxAsync()
	{
		PlaySfx(trafficLightCountdown);
	}
	
	public void PlayWinningSfx()
	{
		PlaySfx(winning);
	}
	
	public void PlayObjectPlacementSfx()
	{
		PlaySfx(objectPlacement);
	}
}
