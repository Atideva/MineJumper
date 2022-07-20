using System;
using UnityEngine;

public class PlayerSpeedBoost : MonoBehaviour
{
    [SerializeField] [Range(0, 5)] float minMult = 1;
    [SerializeField] [Range(0, 5)] float maxMult = 1;
    [SerializeField] int jumpCountToMaxSpeed;

    float _speedMult;
    int _currentRow;

    private void Awake()
    {
        _speedMult = minMult;
    }

    public float SpeedMult => _speedMult;
    public float SpeedFactor => jumpCountToMaxSpeed > 0 ? (float) _currentRow / jumpCountToMaxSpeed : 0;
    public void IncreaseBoost()
    {
        _currentRow++;

 
        var bonus = (maxMult - minMult) * SpeedFactor;
        _speedMult = minMult + bonus;
    }

    public void ResetBoost()
    {
        _currentRow = 0;
        _speedMult = minMult;
    }
}