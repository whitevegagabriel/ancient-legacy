using UnityEngine;

namespace Collectables
{
    public class CollectableController : MonoBehaviour, ICheckpoint
    {
        PlayerController playController;
        public AudioClip collectAudioClip;
        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            if (collectAudioClip != null)
            {
                var audioSource = other.gameObject.GetComponentInChildren<AudioSource>();
                audioSource.PlayOneShot(collectAudioClip);
            }
            gameObject.SetActive(false);
        }

        public ICheckpoint.CheckpointData GetCheckPointData()
        {
            return new ICheckpoint.CheckpointData(transform.position);
        }
    }
}