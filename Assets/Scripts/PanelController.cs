using System;
using System.Linq;
using StateManagement;
using UnityEngine;
using Walkways;

public class PanelController : MonoBehaviour
{
    
    public string keyName;
    
    private bool _playerInRange;
    private Animator _animator;
    private Canvas canvas;
    private static readonly int IsOn = Animator.StringToHash("IsOn");

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        canvas = GetComponentInChildren<Canvas>();
        canvas.enabled = false;
    }
    
    private void Update()
    {
        HandleNewPlayerInRange(Physics.OverlapSphere(transform.position, 2f)
                    .Any(c => c.CompareTag("Player")));
    }
    
    private void HandleNewPlayerInRange(bool newPlayerInRange)
    {
        if (newPlayerInRange == _playerInRange) return;
        
        if (newPlayerInRange)
        {
            if (PlayerInventory.HasItem(keyName))
            {
                ControlledMovingPlatform.isMoving = true;
                _animator.SetBool(IsOn, true);
            } else {
                canvas.enabled = true;
            }
        }
        else
        {
            ControlledMovingPlatform.isMoving = false;
            _animator.SetBool(IsOn, false);
            canvas.enabled = false;
        }
        _playerInRange = newPlayerInRange;
    }
}
