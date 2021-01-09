using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneManager : MonoBehaviour
{
    public static bool isFinished = false;

    SplashManager theSplash;
    CameraController theCam;
    SlideManager theSlide;

    [SerializeField] Image img_CutScene;

    void Start()
    {
        theSplash = FindObjectOfType<SplashManager>();
        theCam = FindObjectOfType<CameraController>();
        theSlide = FindObjectOfType<SlideManager>();
    }

    public bool CheckCutScene()
    {
        return img_CutScene.gameObject.activeSelf;
    }

    public IEnumerator CutSceneCoroutine(string p_CutSceneName, bool p_isShow)
    {
        SplashManager.isfinish = false;
        StartCoroutine(theSplash.FadeOut(true, false,true));
        yield return new WaitUntil(() => SplashManager.isfinish);

        if (theSlide.CheckSlide())
        {
            StartCoroutine(theSlide.DisAppearSlide());
        }

        if (p_isShow)
        {
            Sprite t_Sprite = Resources.Load<Sprite>("CutScenes/" + p_CutSceneName);
            if (t_Sprite != null)
            {
                img_CutScene.gameObject.SetActive(true);
                img_CutScene.sprite = t_Sprite;
                theCam.CameraTargetting(null, 0.05f, true, false);
            }
            else
            {
                Debug.LogError("잘못된 컷신 CG파일 이름입니다");
            }
        }
        else
        {
            img_CutScene.gameObject.SetActive(false);
        }


        SplashManager.isfinish = false;
        StartCoroutine(theSplash.FadeIn(true, false));
        yield return new WaitUntil(() => SplashManager.isfinish);

        yield return new WaitForSeconds(0.5f);
        isFinished = true;
    }
} 
