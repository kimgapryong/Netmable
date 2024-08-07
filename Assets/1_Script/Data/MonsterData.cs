using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMonsterData", menuName = "Monster Data")]
public class MonsterData : ScriptableObject
{
    public string monsterName;
    public int health;
    public float damage;
    public float speed;
    public bool isAttack = false;
}
