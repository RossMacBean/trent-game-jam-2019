using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public AudioClip[] Clips;
    public float StepDistance = 0.6f;

    public float MaxPitch = 1.2f;
    public float MinPitch = 0.8f;

    public float Volume = 1f;

    private AudioSource _audioSource;

    public void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null) {
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.spatialBlend = 1f; // Full 3D
        }

        _previousPosition = transform.position;
    }

    private Vector3 _previousPosition;
    private float _distanceFromLastFootstep;

    public void Update()
    {
        var position = transform.position;

        // Play the footsteps based on the distance traveled
        _distanceFromLastFootstep += Vector3.Distance(position, _previousPosition);

        if (_distanceFromLastFootstep > StepDistance)
        {
            // Remove any amount of StepDistance multiples, but only play the clip once
            do
            {
                _distanceFromLastFootstep -= StepDistance;
            } while (_distanceFromLastFootstep > StepDistance);

            PlayClip();
        }

        _previousPosition = position;
    }

    private int _lastClipIndex;
    
    private void PlayClip()
    {
        AudioClip clip;
        switch (Clips.Length)
        {
            case 0:
                return;
            case 1:
                clip = Clips[1];
                break;
            default:
                // Don't play the same clip as the previous one
                int index;
                do
                {
                    index = Random.Range(0, Clips.Length);
                } while (index == _lastClipIndex);
                _lastClipIndex = index;
                clip = Clips[index];
                break;
        }

        _audioSource.pitch = Random.Range(MinPitch, MaxPitch);
        _audioSource.PlayOneShot(clip, Volume);
    }
}
