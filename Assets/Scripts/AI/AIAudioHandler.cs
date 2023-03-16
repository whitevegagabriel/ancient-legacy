using UnityEngine;
using UnityEngine.Events;

namespace AI
{
    public class AIAudioHandler : MonoBehaviour
    {
        public AudioClip radialAttackAudio;
        public AudioClip bossPunchAudio;
        public AudioClip orbAttackAudio;
        public AudioClip orbDeathAudio;
    
        private AudioSource[] _audioSources;
    
        private void Start()
        {
            EventManager.StartListening<RadialAttackEvent>(RadialAttackEventHandler);
            EventManager.StartListening<BossPunchEvent>(BossPunchEventHandler);
            EventManager.StartListening<OrbAttackEvent>(OrbAttackEventHandler);
            EventManager.StartListening<OrbDeathEvent>(OrbDeathEventHandler);
        
            _audioSources = GetComponents<AudioSource>();
        }

        private void RadialAttackEventHandler()
        {
            PlayClipOnFirstAvailableAudioSource(radialAttackAudio);
        }
    
        private void BossPunchEventHandler()
        {
            PlayClipOnFirstAvailableAudioSource(bossPunchAudio);
        }
    
        private void OrbAttackEventHandler()
        {
            PlayClipOnFirstAvailableAudioSource(orbAttackAudio);
        }

        private void OrbDeathEventHandler()
        {
            PlayClipOnFirstAvailableAudioSource(orbDeathAudio);
        }
    
        private void PlayClipOnFirstAvailableAudioSource(AudioClip clip) {
            foreach (AudioSource audioSource in _audioSources) {
                if (!audioSource.isPlaying) {
                    audioSource.clip = clip;
                    audioSource.Play();
                    return;
                }
            }
        }
    
        public class RadialAttackEvent : UnityEvent {}
    
        public class BossPunchEvent : UnityEvent {}
    
        public class OrbAttackEvent : UnityEvent {}
    
        public class OrbDeathEvent : UnityEvent {}
    }
}
