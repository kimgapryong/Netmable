using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMonsterData", menuName = "Monster Data")]
public class MonsterData : ScriptableObject
{
    public Sprite monsterSprite;
    public string monsterName;

    public int Exp;
    public int health;
    public int damage;
    public float speed;
    public bool isAttack = false;
}
