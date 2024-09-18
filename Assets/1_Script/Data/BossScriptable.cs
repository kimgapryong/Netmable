using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "BossData", menuName = "NewBossData")]
public class BossScriptable : ScriptableObject
{
    public string bossName;

    public int maxHp;

    public int damage;

    public int speed;
}
