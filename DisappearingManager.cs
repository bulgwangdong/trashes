using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingManager : MonoBehaviour
{
    public GameObject[] characters;

    private void Awake()
    {
        FindCharacters();
    }
    public void FindCharacters()
    {
        characters = GameObject.FindGameObjectsWithTag("Character");
    }


}
