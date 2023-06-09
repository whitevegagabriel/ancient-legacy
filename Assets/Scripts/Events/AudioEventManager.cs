﻿using System.Collections;
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
    public AudioClip blockAudio;
    public AudioClip skeletonDeathAudio;
    public AudioClip fireballThrowAudio;
    public AudioClip fireballWallAudio;

    private UnityAction<Vector3, PlayerController.airState> playerLandsEventListener;
    private UnityAction<Vector3> swordAttackEventListener;
    private UnityAction<Vector3> jumpEventListener;
    private UnityAction<Vector3> damageEventListener;
    private UnityAction<Vector3> deathEventListener;
    private UnityAction<Vector3> swooshEventListener;
    private UnityAction<Vector3> platformStartEventListener;
    private UnityAction<Vector3> platformStopEventListener;
    private UnityAction<Vector3> cylinderMoveEventListener;
    private UnityAction<Vector3> blockEventListener;
    private UnityAction<Vector3> skeletonDeathEventListener;
    private UnityAction<Vector3> hitEventListener;
    private UnityAction<Vector3> fireballWallEventListener;
    private UnityAction<GameObject> fireballThrowEventListener;

    void Awake() {
        playerLandsEventListener = playerLandsEventHandler;
        swordAttackEventListener = swordAttackEventHandler;
        jumpEventListener = jumpEventHandler;
        deathEventListener = deathEventHandler;
        swooshEventListener = swooshEventHandler;
        platformStartEventListener = platformStartEventHandler;
        platformStopEventListener = platformStopEventHandler;
        cylinderMoveEventListener = cylinderMoveEventHandler;
        blockEventListener = blockEventHandler;
        skeletonDeathEventListener = skeletonDeathEventHandler;
        hitEventListener = hitEventHandler;
        fireballThrowEventListener = fireballThrowEventHandler;
        fireballWallEventListener = fireballWallEventHandler;
    }

    void Start() {
			
    }

    void OnEnable() {
        EventManager.StartListening<PlayerLandsEvent, Vector3, PlayerController.airState>(playerLandsEventListener);
        EventManager.StartListening<AttackEvent, Vector3>(swordAttackEventListener);
        EventManager.StartListening<JumpEvent, Vector3>(jumpEventListener);
        EventManager.StartListening<PlayerDeathEvent, Vector3>(deathEventListener);
        EventManager.StartListening<SwooshEvent, Vector3>(swooshEventListener);
        EventManager.StartListening<PlatformStartEvent, Vector3>(platformStartEventListener);
        EventManager.StartListening<PlatformStopEvent, Vector3>(platformStopEventListener);
        EventManager.StartListening<CylinderMoveEvent, Vector3>(cylinderMoveEventListener);
        EventManager.StartListening<BlockEvent, Vector3>(blockEventListener);
        EventManager.StartListening<SkeletonDeathEvent1, Vector3>(skeletonDeathEventListener);
        EventManager.StartListening<HitEvent, Vector3>(hitEventListener);
        EventManager.StartListening<FireballThrowEvent, GameObject>(fireballThrowEventListener);
        EventManager.StartListening<FireballWallEvent, Vector3>(fireballWallEventListener);
    }

    void OnDisable() {

        EventManager.StopListening<PlayerLandsEvent, Vector3, PlayerController.airState>(playerLandsEventListener);
        EventManager.StopListening<AttackEvent, Vector3>(swordAttackEventListener);
        EventManager.StopListening<JumpEvent, Vector3>(jumpEventListener);
        EventManager.StopListening<PlayerDeathEvent, Vector3>(deathEventListener);
        EventManager.StopListening<SwooshEvent, Vector3>(swooshEventListener);
        EventManager.StopListening<PlatformStartEvent, Vector3>(platformStartEventListener);
        EventManager.StopListening<PlatformStopEvent, Vector3>(platformStopEventListener);
        EventManager.StopListening<CylinderMoveEvent, Vector3>(cylinderMoveEventListener);
        EventManager.StopListening<BlockEvent, Vector3>(blockEventListener);
        EventManager.StopListening<SkeletonDeathEvent1, Vector3>(skeletonDeathEventListener);
        EventManager.StopListening<HitEvent, Vector3>(hitEventListener);
        EventManager.StopListening<FireballWallEvent, Vector3>(fireballWallEventListener);
        EventManager.StopListening<FireballThrowEvent, GameObject>(fireballThrowEventListener);
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

            snd.audioSrc.clip = playerLandsAudio;

            snd.audioSrc.minDistance = 5f;
            snd.audioSrc.maxDistance = 100f;

            snd.audioSrc.Play();
        }
        else if (state == PlayerController.airState.Fall)
        {

            EventSound3D snd2 = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

            snd2.audioSrc.clip = gruntAudio;

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

        snd.audioSrc.clip = jumpAudio;

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

        snd.audioSrc.clip = deathAudio;

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

        snd.audioSrc.clip = swordAttackAudio;

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

        snd.audioSrc.clip = swooshAudio;

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

        snd.audioSrc.clip = platformStartAudio;

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

        snd.audioSrc.clip = platformStopAudio;

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

        snd.audioSrc.clip = cylinderMoveAudio;

        snd.audioSrc.minDistance = 1f;
        snd.audioSrc.maxDistance = 2f;

        snd.audioSrc.Play();
        snd.audioSrc.loop = true;
    }

    void blockEventHandler(Vector3 worldPos)
    {
        if (!eventSound3DPrefab)
        {
            return;
        }

        EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

        snd.audioSrc.clip = blockAudio;

        snd.audioSrc.minDistance = 5f;
        snd.audioSrc.maxDistance = 100f;

        snd.audioSrc.Play();
    }
    
    void skeletonDeathEventHandler(Vector3 worldPos)
    {
        if (!eventSound3DPrefab)
        {
            return;
        }

        EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

        snd.audioSrc.clip = skeletonDeathAudio;

        snd.audioSrc.minDistance = 5f;
        snd.audioSrc.maxDistance = 100f;

        snd.audioSrc.Play();
    }


    void hitEventHandler(Vector3 worldPos)
    {
        if (!eventSound3DPrefab)
        {
            return;
        }

        EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

        snd.audioSrc.clip = gruntAudio;

        snd.audioSrc.minDistance = 5f;
        snd.audioSrc.maxDistance = 100f;

        snd.audioSrc.Play();
    }

    void fireballThrowEventHandler(GameObject go)
    {
        if (!eventSound3DPrefab)
        {
            return;
        }

        EventSound3D snd = Instantiate(eventSound3DPrefab, go.transform);

        snd.audioSrc.clip = fireballThrowAudio;

        snd.audioSrc.dopplerLevel = 1;
        snd.audioSrc.minDistance = 2f;
        snd.audioSrc.maxDistance = 50f;
        snd.audioSrc.rolloffMode = AudioRolloffMode.Logarithmic;

        snd.audioSrc.Play();
        snd.audioSrc.loop = true;
    }

    void fireballWallEventHandler(Vector3 worldPos)
    {
        if (!eventSound3DPrefab)
        {
            return;
        }

        EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

        snd.audioSrc.clip = fireballWallAudio;

        snd.audioSrc.minDistance = 2f;
        snd.audioSrc.maxDistance = 100f;

        snd.audioSrc.Play();
    }
}
