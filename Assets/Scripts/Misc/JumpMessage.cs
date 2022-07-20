using System;
using System.Collections;
using System.Collections.Generic;
using DamageNumbersPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class JumpMessage : MonoBehaviour
{
    public KeyCode testKey;
    public DamageNumber textPrefab;
    public RectTransform spawnPos;

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(testKey))
        {
            Spawn("Test Jump Message", Color.HSVToRGB(Random.value, 0.5f, 1f));
        }
    }
#endif

    public void Spawn(string text, Color clr)
    {
        var newDamageNumber = textPrefab.Spawn(Vector3.zero);
        newDamageNumber.SetAnchoredPosition(spawnPos, Vector2.zero);
        newDamageNumber.enableFollowing = true;
        newDamageNumber.followedTarget = spawnPos;
        newDamageNumber.leftText = text;
        newDamageNumber.SetColor(clr);
    }
}