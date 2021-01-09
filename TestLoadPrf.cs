using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestLoadPrf : MonoBehaviour
{
    [SerializeField] GameObject[] maps;
    [SerializeField] GameObject whereMap;
    [SerializeField] EventSystem evs;
    [SerializeField] Text showArea;
    [SerializeField] Text showSpawns;

    int nowMapIndex;



    public void SetEHandbookMap()
    {
        for (int i = 0; i < maps.Length;)
        {
            MapBehaviour mapB = maps[i].GetComponent<MapBehaviour>();

            if (mapB.IsWhereScene() != null)
            {
                maps[i].gameObject.SetActive(true);
                nowMapIndex = i;
                evs.SetSelectedGameObject(mapB.IsWhereScene());
                break;
            }
            else i++;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ChangeMap(false);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            ChangeMap(true);
        }

        ChangeText();
    }

    void ChangeMap(bool p_bool)
    {
        maps[nowMapIndex].gameObject.SetActive(false);

        nowMapIndex += p_bool ? 1 : -1;

        if(nowMapIndex < 0)
        {
            nowMapIndex = maps.Length - 1;
            maps[nowMapIndex].SetActive(true);
            evs.SetSelectedGameObject(maps[nowMapIndex].GetComponent<MapBehaviour>().LastButton.gameObject);

        }
        else if(nowMapIndex >= maps.Length)
        {
            nowMapIndex = 0;
            maps[nowMapIndex].SetActive(true);
            evs.SetSelectedGameObject(maps[nowMapIndex].GetComponent<MapBehaviour>().FirstButton.gameObject);
        }
        else
        {
            maps[nowMapIndex].SetActive(true);
            if (!p_bool)
                evs.SetSelectedGameObject(maps[nowMapIndex].GetComponent<MapBehaviour>().LastButton.gameObject);
            else
                evs.SetSelectedGameObject(maps[nowMapIndex].GetComponent<MapBehaviour>().FirstButton.gameObject);
        }
    }
    void ChangeText()
    {
        showArea.text = maps[nowMapIndex].GetComponent<MapBehaviour>().mapName;
        showSpawns.text = evs.currentSelectedGameObject.name;
    }
}
