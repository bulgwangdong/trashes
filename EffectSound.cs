using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSound : MonoBehaviour
{
    public static bool isFirst = true;

    public void SelectSound()
    {
        if (!isFirst)
        {
            SoundManager.instance.StopAllEffectSound();
            SoundManager.instance.PlaySound("Select", 1);
        }
 
        else
            isFirst = false;
    }
    public void SetIsFirst(bool p_bool)
    {
        isFirst = p_bool;
    }
}
