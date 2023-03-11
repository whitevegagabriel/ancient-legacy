using UnityEngine;

public class Targetable : MonoBehaviour
{
    AI.BossAI _bossAI;
    void Start()
    {
        _bossAI = GameObject.FindGameObjectWithTag("Boss").GetComponent<AI.BossAI>();
    }

    public void OnHit()
    {
        _bossAI.DecreaseHealth(2);
    }
}