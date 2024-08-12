using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkilData", menuName = "Skil Data")]
public class SkilData : ScriptableObject
{
    
    public string SkilName;
    public GameObject SkilPrefab;

    public int damage;
    public int speed;
    public int nextSkilLevel;
    public KeyCode keyCode;
}
