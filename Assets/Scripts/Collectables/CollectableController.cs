using UnityEngine;

namespace Collectables
{
    public class CollectableController : MonoBehaviour
    {
        PlayerController playController;
        public AudioClip collectAudioClip;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                var audioSource = other.gameObject.GetComponentInChildren<AudioSource>();
                audioSource.clip = collectAudioClip;
                audioSource.Play();
                gameObject.SetActive(false);
            }
        }
    }
}