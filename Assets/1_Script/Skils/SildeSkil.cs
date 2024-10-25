using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SildeSkil : Skil
{
    public SkilData skilData;

    private void Start()
    {
        ResetSkil(skilData);
        StartCoroutine(waitSkil());
    }
    public IEnumerator waitSkil()
    {
        while (status == null)
        {
            yield return null;
        }
        status.OnLevelUp += OkShield;
    }

    public void OkShield()
    {
        if(status.currentLevel >= SkilLevel)
        {
            playerSkils.GetShield(key);
        }
    }


}
