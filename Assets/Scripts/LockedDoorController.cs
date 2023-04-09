using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StateManagement;
using UnityEngine;

public class LockedDoorController : MonoBehaviour
{
    public string keyName;

    private Animator _animator;
    private static readonly int IsOpen = Animator.StringToHash("IsOpen");
    
    private bool _playerInRange;
    private Canvas canvas;
    private BoxCollider boxCollider;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        canvas = GetComponentInChildren<Canvas>();
        canvas.enabled = false;
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleNewPlayerInRange(Physics.OverlapSphere(transform.position, 5f)
            .Any(c => c.CompareTag("Player")));
    }
    
    private void HandleNewPlayerInRange(bool newPlayerInRange)
    {
        if (newPlayerInRange == _playerInRange) return;
        
        if (newPlayerInRange)
        {
            if (PlayerInventory.HasItem(keyName))
            {
                _animator.SetBool(IsOpen, true);
                boxCollider.enabled = false;
            } else {
                canvas.enabled = true;
            }
        }
        else
        {
            _animator.SetBool(IsOpen, false);
            boxCollider.enabled = true;
            canvas.enabled = false;
        }
        _playerInRange = newPlayerInRange;
    }
}
