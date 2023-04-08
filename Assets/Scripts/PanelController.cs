using System;
using System.Linq;
using StateManagement;
using UnityEngine;
using Walkways;

public class PanelController : MonoBehaviour
{
    
    public string keyName;
    
    private bool _playerInRange;
    private Canvas canvas;

    private void Start()
    {
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
            } else {
                canvas.enabled = true;
            }
        }
        else
        {
            ControlledMovingPlatform.isMoving = false;
            canvas.enabled = false;
        }
        _playerInRange = newPlayerInRange;
    }
}
