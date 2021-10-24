using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class AudioContainer
{
	public string name;

	public AudioClip clip;

	[Range(0f, 1f)]
	public float volume = 1;
}

public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;

	public AudioSource audioSource;

	public AudioContainer[] containers;

	void Awake() => instance = this;

	public static void play(string name)
	{
		AudioContainer container = new AudioContainer();

		int containersLength = instance.containers.Length;

		for(int i = 0; i < containersLength; i++)
		{
			if(instance.containers[i].name == name)
				container = instance.containers[i];
		}

		instance.audioSource.clip = container.clip;
		instance.audioSource.volume = container.volume;

		instance.audioSource.Play();
	}
}
