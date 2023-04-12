using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICheckpoint
{
    public CheckpointData GetCheckPointData();

    public class CheckpointData {
        
        public CheckpointData(Vector3 respawnPoint) {
            this.respawnPoint = respawnPoint;
        }
        
        public Vector3 respawnPoint { get; }
    }
}
