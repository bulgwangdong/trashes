using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MapBehaviour : MonoBehaviour
{
    
    [SerializeField] Wheres[] tfButs;
    public string mapName;
    public Button FirstButton;
    public Button LastButton;

    public GameObject IsWhereScene()
    {
        for (int i = 0; i < tfButs.Length; i++)
        {
            if (tfButs[i].ButSceneName == SceneManager.GetActiveScene().name)
            {
                return tfButs[i].whereBut.gameObject;
            }
        }

        return null;
    }
    public void LetsTP(int p_Index)
    {
        if (tfButs[p_Index].isCanTP)
        {
            TransferManager tfM = FindObjectOfType<TransferManager>();
            StartCoroutine(tfM.Transfer(tfButs[p_Index].ButSceneName, tfButs[p_Index].SpawnTPName));
        }
    }


}

[System.Serializable]
public class Wheres
{
    public Button whereBut;
    public bool isCanTP;
    public string ButSceneName;
    public string SpawnTPName;
}