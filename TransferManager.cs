using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferManager : MonoBehaviour
{
    string locationName;

    [SerializeField] GameObject mapMenu;
    [SerializeField] GameObject backGround;

    SplashManager theSplash;
    InteractionControler theIC;
    DisappearingManager theDS;

    public static bool isFinished = true;

    private void Start()
    {
        theIC = FindObjectOfType<InteractionControler>();
        theSplash = FindObjectOfType<SplashManager>();
        theDS = FindObjectOfType<DisappearingManager>();

    }
    public IEnumerator Transfer(string p_SceneName, string p_LocationName)
    {

        Debug.Log(p_LocationName);
        Debug.Log(p_SceneName);
        theIC.SettingUI(false);
        if (EHandBookManager.isPaused)
        {
            Time.timeScale = 1f;
            backGround.SetActive(false);
            mapMenu.SetActive(false);
            EHandBookManager.isPaused = false;
            EHandBookManager.returned = false;
        }
        Debug.Log(EHandBookManager.isPaused);

        isFinished = false;
        SplashManager.isfinish = false;
        StartCoroutine(theSplash.FadeOut(false, true,false));

        Debug.Log(SplashManager.isfinish);

        yield return new WaitUntil(() => SplashManager.isfinish);

        Debug.Log(SplashManager.isfinish);

        Debug.Log(p_LocationName);
        Debug.Log(p_SceneName);
        locationName = p_LocationName;
        TransferSpawnManager.isSpawnTiming = true;
        Debug.Log(TransferSpawnManager.isSpawnTiming);
        SceneManager.LoadScene(p_SceneName);
    }

    public IEnumerator Done()
    {
        SplashManager.isfinish = false;
        StartCoroutine(theSplash.FadeIn(false, true));
        yield return new WaitUntil(() => SplashManager.isfinish);
        theDS.FindCharacters();

        isFinished = true;


        yield return new WaitForSeconds(0.5f);
        if (!DialogueManager.isWaiting)
        theIC.SettingUI(true);

    }
    public string GetLocationName()
    {
        return locationName;
    }
}
