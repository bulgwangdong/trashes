using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EHandBookManager : MonoBehaviour
{
    public static EHandBookManager instance;

    public GameObject HandbookFirstMenu;
    public GameObject BackGround;
    public GameObject MapMenu;
    public EventSystem Evs;
    public GameObject FirstButton;


    public static bool isPaused = false;
    public  static bool returned = false;




    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            SetPause(true);
        }
        else if(isPaused && returned)
        {
            SetPause(false);
            returned = false;
        }
    }


    public void SetPause(bool p_bool)
    {
        HandbookFirstMenu.SetActive(p_bool);
        BackGround.SetActive(p_bool);
        if (!p_bool)
        {
            MapMenu.SetActive(p_bool);
        }

        Time.timeScale = p_bool ? 0f : 1f;

        if (p_bool)
        {
            EffectSound.isFirst = true;
            Evs.SetSelectedGameObject(FirstButton);
        }



        isPaused = p_bool;


    }

    public void ReturnButtons(bool p_bool)
    {
        returned = p_bool;
    }
    public void SetIsPaused(bool p_bool)
    {
        isPaused = p_bool;
    }

}
