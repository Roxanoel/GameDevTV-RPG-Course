using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class RandomSoundPlayer : MonoBehaviour
    {
        [SerializeField] AudioClip[] audioClips;
        [Header("Parameters")]
        [SerializeField] float volumeScale = 0.75f;
        AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlayRandomSFX()
        {
            int randomIndex = Random.Range(0, audioClips.Length);
            audioSource.PlayOneShot(audioClips[randomIndex], volumeScale);
        }
    }
}
