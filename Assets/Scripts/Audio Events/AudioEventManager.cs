using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioEventManager : MonoBehaviour
{
    public EventSound3D eventSound3DPrefab;
    public AudioClip[] boxAudio = null;
    public AudioClip playerLandsAudio;
    public AudioClip swordAttackAudio;
    public AudioClip deathAudio;
    public AudioClip jumpAudio;
    public AudioClip gruntAudio;
    public AudioClip swooshAudio;
    public AudioClip platformStartAudio;
    public AudioClip platformStopAudio;
    public AudioClip cylinderMoveAudio;

    private UnityAction<Vector3, PlayerController.airState> playerLandsEventListener;
    private UnityAction<Vector3> swordAttackEventListener;
    private UnityAction<Vector3> jumpEventListener;
    private UnityAction<Vector3> damageEventListener;
    private UnityAction<Vector3> deathEventListener;
    private UnityAction<Vector3> swooshEventListener;
    private UnityAction<Vector3> platformStartEventListener;
    private UnityAction<Vector3> platformStopEventListener;
    private UnityAction<Vector3> cylinderMoveEventListener;

    void Awake() {
        playerLandsEventListener = new UnityAction<Vector3, PlayerController.airState>(playerLandsEventHandler);
        swordAttackEventListener = new UnityAction<Vector3>(swordAttackEventHandler);
        jumpEventListener = new UnityAction<Vector3>(jumpEventHandler);
        deathEventListener = new UnityAction<Vector3>(deathEventHandler);
        swooshEventListener = new UnityAction<Vector3>(swooshEventHandler);
        platformStartEventListener = new UnityAction<Vector3>(platformStartEventHandler);
        platformStopEventListener = new UnityAction<Vector3>(platformStopEventHandler);
        cylinderMoveEventListener = new UnityAction<Vector3>(cylinderMoveEventHandler);
    }

    void Start() {
			
    }

    void OnEnable() {
        EventManager.StartListening<PlayerLandsEvent, Vector3, PlayerController.airState>(playerLandsEventListener);
        EventManager.StartListening<AttackEvent, Vector3>(swordAttackEventListener);
        EventManager.StartListening<JumpEvent, Vector3>(jumpEventListener);
        EventManager.StartListening<DeathEvent, Vector3>(deathEventListener);
        EventManager.StartListening<SwooshEvent, Vector3>(swooshEventListener);
        EventManager.StartListening<PlatformStartEvent, Vector3>(platformStartEventListener);
        EventManager.StartListening<PlatformStopEvent, Vector3>(platformStopEventListener);
        EventManager.StartListening<CylinderMoveEvent, Vector3>(cylinderMoveEventListener);

    }

    void OnDisable() {

        EventManager.StopListening<PlayerLandsEvent, Vector3, PlayerController.airState>(playerLandsEventListener);
        EventManager.StopListening<AttackEvent, Vector3>(swordAttackEventListener);
        EventManager.StopListening<JumpEvent, Vector3>(jumpEventListener);
        EventManager.StopListening<DeathEvent, Vector3>(deathEventListener);
        EventManager.StopListening<SwooshEvent, Vector3>(swooshEventListener);
        EventManager.StopListening<PlatformStartEvent, Vector3>(platformStartEventListener);
        EventManager.StopListening<PlatformStopEvent, Vector3>(platformStopEventListener);
        EventManager.StopListening<CylinderMoveEvent, Vector3>(cylinderMoveEventListener);
    }

    void playerLandsEventHandler(Vector3 worldPos, PlayerController.airState state)
    {
        if (!eventSound3DPrefab)
        {
            return;
        }
        
        if (state == PlayerController.airState.Jump)
        {
            
            EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

            snd.audioSrc.clip = this.playerLandsAudio;

            snd.audioSrc.minDistance = 5f;
            snd.audioSrc.maxDistance = 100f;

            snd.audioSrc.Play();
        }
        else if (state == PlayerController.airState.Fall)
        {

            EventSound3D snd2 = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

            snd2.audioSrc.clip = this.gruntAudio;

            snd2.audioSrc.minDistance = 5f;
            snd2.audioSrc.maxDistance = 100f;

            snd2.audioSrc.Play();
        }
    }

    void jumpEventHandler(Vector3 worldPos)
    {
        if (!eventSound3DPrefab)
        {
            return;
        }

        EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

        snd.audioSrc.clip = this.jumpAudio;

        snd.audioSrc.minDistance = 5f;
        snd.audioSrc.maxDistance = 100f;

        snd.audioSrc.Play();
    }

    void deathEventHandler(Vector3 worldPos)
    {
        if (!eventSound3DPrefab)
        {
            return;
        }

        EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

        snd.audioSrc.clip = this.deathAudio;

        snd.audioSrc.minDistance = 5f;
        snd.audioSrc.maxDistance = 100f;

        snd.audioSrc.Play();
    }

    void swordAttackEventHandler(Vector3 worldPos)
    {
        if (!eventSound3DPrefab)
        {
            return;
        }

        EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

        snd.audioSrc.clip = this.swordAttackAudio;

        snd.audioSrc.minDistance = 5f;
        snd.audioSrc.maxDistance = 100f;

        snd.audioSrc.Play();
    }

    void swooshEventHandler(Vector3 worldPos)
    {
        if (!eventSound3DPrefab)
        {
            return;
        }

        EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

        snd.audioSrc.clip = this.swooshAudio;

        snd.audioSrc.minDistance = 1f;
        snd.audioSrc.maxDistance = 2f;

        snd.audioSrc.Play();
    }

    void platformStartEventHandler(Vector3 worldPos)
    {
        if (!eventSound3DPrefab)
        {
            return;
        }

        EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

        snd.audioSrc.clip = this.platformStartAudio;

        snd.audioSrc.minDistance = 1f;
        snd.audioSrc.maxDistance = 2f;

        snd.audioSrc.Play();
    }

    void platformStopEventHandler(Vector3 worldPos)
    {
        if (!eventSound3DPrefab)
        {
            return;
        }

        EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

        snd.audioSrc.clip = this.platformStopAudio;

        snd.audioSrc.minDistance = 1f;
        snd.audioSrc.maxDistance = 2f;

        snd.audioSrc.Play();
    }

    void cylinderMoveEventHandler(Vector3 worldPos)
    {
        if (!eventSound3DPrefab)
        {
            return;
        }

        EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

        snd.audioSrc.clip = this.cylinderMoveAudio;

        snd.audioSrc.minDistance = 1f;
        snd.audioSrc.maxDistance = 2f;

        snd.audioSrc.Play();
        snd.audioSrc.loop = true;
    }
}
