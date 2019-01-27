using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class AmbientAudioVolume : MonoBehaviour
{
	[SerializeField] bool StartPlayOnExit = false;
	[SerializeField] float FadeSpeed = 0.5f;

	bool mFadeIn = false;

	AudioSource[] AmbientSounds;

	private BoxCollider mVolume = null;

    // Start is called before the first frame update
    void Start()
    {
		mVolume = FindObjectOfType<BoxCollider>();
		mVolume.isTrigger = true;
		
		AmbientSounds = gameObject.GetComponents<AudioSource>();

		for (int i = 0; i < AmbientSounds.Length; ++i)
		{
			AmbientSounds[i].Play();
			AmbientSounds[i].volume = 0;
		}

		bool PlayerFound = false;
		Collider[] OverlappingObjects = Physics.OverlapBox(transform.position, mVolume.size / 2);
		foreach (Collider collider in OverlappingObjects)
		{
			if (collider.GetComponent<RigidBodyPlayerController>())
			{
				PlayerFound = true;
			}
		}

		if (PlayerFound)
		{
			if (StartPlayOnExit)
			{
				FadeOut();
			}
			else
			{
				// fade in the sound.
				FadeIn();
			}			
		}
		else
		{
			if (StartPlayOnExit)
			{
				FadeIn();
			}
			else
			{
				FadeOut();
			}
		}
	}

	void FadeIn()
	{
		mFadeIn = true;	
	}

	void FadeOut()
	{
		mFadeIn = false;
	}

    // Update is called once per frame
    void Update()
    {
		for (int i = 0; i < AmbientSounds.Length; ++i)
		{
			if (mFadeIn)
			{
				AmbientSounds[i].volume = Mathf.Clamp(AmbientSounds[i].volume + (FadeSpeed * Time.deltaTime), 0, 1);
			}
			else
			{
				AmbientSounds[i].volume = Mathf.Clamp(AmbientSounds[i].volume - (FadeSpeed * Time.deltaTime), 0, 1);
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		RigidBodyPlayerController Player = other.GetComponent<RigidBodyPlayerController>();
		if (Player != null)
		{
			for (int i = 0; i < AmbientSounds.Length; ++i)
			{
				if (StartPlayOnExit)
				{
					FadeOut();
				}
				else
				{
					FadeIn();
				}
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		RigidBodyPlayerController Player = other.GetComponent<RigidBodyPlayerController>();
		if (Player != null)
		{
			for (int i = 0; i < AmbientSounds.Length; ++i)
			{
				if (StartPlayOnExit)
				{
					FadeIn();
				}
				else
				{
					FadeOut();
				}
			}
		}
	}
}
