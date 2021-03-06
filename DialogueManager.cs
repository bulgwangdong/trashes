﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static bool isWaiting;

    [SerializeField] GameObject go_DialogueBar;
    [SerializeField] GameObject go_DialogueNameBar;

    [SerializeField] Text txt_Dialogue;
    [SerializeField] Text txt_Name;

    // 이벤트 끝나면 등장시킬(혹은 퇴장시킬) 오브젝트들
    GameObject[] go_Objects;
    byte appearTypeNumber;
    const byte NONE = 0, APPEAR = 1, DISAPPEAR = 2;

    public void SetAppearObjects(GameObject[] p_Targets)
    {
        go_Objects = p_Targets;
        appearTypeNumber = APPEAR;
    }
    public void SetDisAppearObjects(GameObject[] p_Targets)
    {
        go_Objects = p_Targets;
        appearTypeNumber = DISAPPEAR;
    }

    // 다음 이벤트를 위한 세팅

    GameObject go_NextEvent;

    public void SetNextEvent(GameObject p_NextEvent)
    {
        go_NextEvent = p_NextEvent;
    }
    


    InteractionControler theIC;
    CameraController theCam;
    SpriteManager theSprite;
    SplashManager theSplash;
    CutSceneManager theCutScene;
    SlideManager theSlide;
    DisappearingManager theDM;

    Dialogue[] dialogues;

    [Header("텍스트 출력 딜레이.")]
    [SerializeField] float txtDelay;



    bool isDialogue = false;
    bool isNext = false;

    string nowText;




    [SerializeField]
    int lineCount = 0;
    [SerializeField]
    int contextCount = 0;


    private void Start()
    {
        theIC = FindObjectOfType<InteractionControler>();
        theCam = FindObjectOfType<CameraController>();
        theSprite = FindObjectOfType<SpriteManager>();
        theSplash = FindObjectOfType<SplashManager>();
        theCutScene = FindObjectOfType<CutSceneManager>();
        theSlide = FindObjectOfType<SlideManager>();
        theDM = FindObjectOfType<DisappearingManager>();
    }


    private void Update()
    {
        if (isDialogue)
        {
            if (isNext)
            {
                if (!EHandBookManager.isPaused)
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        isNext = false;
                        txt_Dialogue.text = "";
                        ++contextCount;



                        if (contextCount < dialogues[lineCount].contexts.Length)
                        {
                            StartCoroutine(TypeWriter());


                        }
                        else
                        {
                            contextCount = 0;
                            if (++lineCount < dialogues.Length)
                            {
                                StartCoroutine(CameraTargetType());

                            }
                            else
                            {
                                StartCoroutine(EndDialogue());
                            }
                        }
                    }
                }

                
            }
        }
    }
    public void ShowDialogue(Dialogue[] p_dialogues)
    {
        isDialogue = true;
        txt_Dialogue.text = "";
        txt_Name.text = "";
        theIC.SettingUI(false);
        dialogues = p_dialogues;

        StartCoroutine(StartDialogue());
    }

    IEnumerator StartDialogue()
    {
        if (isWaiting)
            yield return new WaitForSeconds(0.5f);
        isWaiting = false;
        theCam.CameraOriginSetting();
        StartCoroutine(CameraTargetType());

    }

    IEnumerator CameraTargetType()
    {   if(dialogues[lineCount].tf_target != null)
        {
            DisappearObjects(false);
        }
        else if(dialogues[lineCount].cameraType == CameraType.Reset)
        {
            DisappearObjects(true);
            StartCoroutine(theSprite.HalfSpriteDisappearCoroutine());
        }

        
        switch (dialogues[lineCount].cameraType)
        {
            case CameraType.ObjectFront: theCam.CameraTargetting(dialogues[lineCount].tf_target); new WaitUntil(() => CameraController.isFinish); break;
            case CameraType.Reset: theCam.CameraTargetting(null, 0.05f, true, false); break;
            case CameraType.FadeIn: SettingUI(false); SplashManager.isfinish = false; StartCoroutine(theSplash.FadeIn(false, true)); yield return new WaitUntil(() => SplashManager.isfinish); break;
            case CameraType.FadeOut: SettingUI(false); SplashManager.isfinish = false; StartCoroutine(theSplash.FadeOut(false, true,true)); yield return new WaitUntil(() => SplashManager.isfinish); break;
            case CameraType.FlashIn: SettingUI(false); SplashManager.isfinish = false; StartCoroutine(theSplash.FadeIn(true, true)); yield return new WaitUntil(() => SplashManager.isfinish); break;
            case CameraType.FlashOut: SettingUI(false); SplashManager.isfinish = false; StartCoroutine(theSplash.FadeOut(true, true,false)); yield return new WaitUntil(() => SplashManager.isfinish); break;
            case CameraType.ShowCutScene: SettingUI(false); CutSceneManager.isFinished = false; StartCoroutine(theCutScene.CutSceneCoroutine(dialogues[lineCount].spriteName[contextCount], true)); yield return new WaitUntil(() => CutSceneManager.isFinished); break;
            case CameraType.HideCutScene: SettingUI(false); CutSceneManager.isFinished = false; StartCoroutine(theCutScene.CutSceneCoroutine(null, false)); yield return new WaitUntil(() => CutSceneManager.isFinished); theCam.CameraTargetting(dialogues[lineCount].tf_target); break;
            case CameraType.AppearSlideCG: SlideManager.isFinished = false; StartCoroutine(theSlide.AppearSlide(SplitSlideCGName())); yield return new WaitUntil(() => SlideManager.isFinished); theCam.CameraTargetting(dialogues[lineCount].tf_target); break;
            case CameraType.DisappearSlideCG: SlideManager.isFinished = false; StartCoroutine(theSlide.DisAppearSlide()); yield return new WaitUntil(() => SlideManager.isFinished); theCam.CameraTargetting(dialogues[lineCount].tf_target); break;
            case CameraType.ChangeSlideCG: SlideManager.isChanged = false; StartCoroutine(theSlide.ChangeSlide(SplitSlideCGName())); yield return new WaitUntil(() => SlideManager.isChanged); theCam.CameraTargetting(dialogues[lineCount].tf_target); break;

        }

        StartCoroutine(TypeWriter());



    }

    string SplitSlideCGName()
    {
        string t_Text = dialogues[lineCount].spriteName[contextCount];
        string[] t_Array = t_Text.Split(new char[] { '/' });
        if (t_Array.Length <= 1)
        {
            return t_Array[0];
        }
        else
        {
            return t_Array[1];
        }
    }

    IEnumerator TypeWriter()
    {
        
        SettingUI(true);
        ChangeSprite();
        PlaySound();

        string t_ReplaceText = dialogues[lineCount].contexts[contextCount];
        t_ReplaceText = t_ReplaceText.Replace("'", ",");
        t_ReplaceText = t_ReplaceText.Replace("\\n", "\n");

        bool t_white = false, t_yellow = false, t_cyan = false;
        bool t_ignore = false;
        for (int i = 0; i < t_ReplaceText.Length; i++)
        {

            switch (t_ReplaceText[i])
            {
                case 'ⓦ': t_white = true; t_yellow = false; t_cyan = false; t_ignore = true; break;
                case 'ⓨ': t_white = false; t_yellow = true; t_cyan = false; t_ignore = true; break;
                case 'ⓒ': t_white = false; t_yellow = false; t_cyan = true; t_ignore = true; break;
                case '①': StartCoroutine(theSplash.Splash()); SoundManager.instance.PlaySound("Emotion1", 1); t_ignore = true; break;
                case '②': StartCoroutine(theSplash.Splash()); SoundManager.instance.PlaySound("Emotion2", 1); t_ignore = true; break;
                case '⒜': StartCoroutine(theSprite.HalfSpriteDisappearCoroutine(0.005f)); t_ignore = true; break;


            }

            string t_letter = t_ReplaceText[i].ToString();
            if (!t_ignore)
            {
                if (t_white) { t_letter = "<color=#ffffff>" + t_letter + "</color>"; }
                else if (t_yellow) { t_letter = "<color=#FFFF00>" + t_letter + "</color>"; }
                else if (t_cyan) { t_letter = "<color=#42DEE3>" + t_letter + "</color>"; }

                txt_Dialogue.text += t_letter;
            }
            t_ignore = false;

            if (Input.GetKey(KeyCode.S))
            {
                yield return null;
            }
            else
            {
                yield return new WaitForSeconds(txtDelay);

            }
        }
            isNext = true;
    }





     void SettingUI(bool p_flag)
        {
            go_DialogueBar.SetActive(p_flag);

            if (p_flag)
            {
                if (dialogues[lineCount].name == "")
                {
                    go_DialogueNameBar.SetActive(false);

                }
                else
                {
                    go_DialogueNameBar.SetActive(true);
                    txt_Name.text = dialogues[lineCount].name;

                }
            }
            else
            {
                go_DialogueNameBar.SetActive(false);
            }
        }
        IEnumerator EndDialogue()
        {
            if (theCutScene.CheckCutScene())
            {
                SettingUI(false);
                CutSceneManager.isFinished = false;
                StartCoroutine(theCutScene.CutSceneCoroutine(null, false));
                yield return new WaitUntil(() => CutSceneManager.isFinished);
            }
            if (theSlide.CheckSlide())
            {
                SettingUI(false);
                SlideManager.isFinished = false;
                StartCoroutine(theSlide.DisAppearSlide());
                yield return new WaitUntil(() => SlideManager.isFinished);
            }
        DisappearObjects(true);

        AppearOrDisappearObjects();
            yield return new WaitUntil(() => Spin.isFinished);
        StartCoroutine(theSprite.HalfSpriteDisappearCoroutine());
            yield return new WaitUntil(() => SpriteManager.isFinish);



        isDialogue = false;
            contextCount = 0;
            lineCount = 0;
            dialogues = null;
        theCam.CameraTargetting(null, 0.05f, true, true);





        isNext = false;

        SettingUI(false);
            yield return new WaitUntil(() => !InteractionControler.isInteract);

            if (go_NextEvent != null)
            {
            InteractionEvent tIE = go_NextEvent.GetComponent<InteractionEvent>();
            if (tIE.CheckEvent())
            {
                go_NextEvent.SetActive(true);
                go_NextEvent = null;
            }
            else
            {
                theIC.SettingUI(true);
            }
        }
            else
            {
                theIC.SettingUI(true);
            }
        }

        void AppearOrDisappearObjects()
        {
            if (go_Objects != null)
            {
                Spin.isFinished = false;
                for (int i = 0; i < go_Objects.Length; i++)
                {
                    if (appearTypeNumber == APPEAR)
                    {
                        go_Objects[i].SetActive(true);
                        StartCoroutine(go_Objects[i].GetComponent<Spin>().SetAppearOrDisappear(true));
                    }
                    else if (appearTypeNumber == DISAPPEAR)
                        StartCoroutine(go_Objects[i].GetComponent<Spin>().SetAppearOrDisappear(false));

                }
                go_Objects = null;
                appearTypeNumber = NONE;
            }


    }

    void ChangeSprite()
        {
            if (dialogues[lineCount].tf_target != null)
            {
                if (dialogues[lineCount].spriteName[contextCount] != "")
                {
                    {
                    StartCoroutine(theSprite.SpriteChangeCoroutine(dialogues[lineCount].tf_target, dialogues[lineCount].spriteName[contextCount].Split(new char[] { '/' })[0]));
                    StartCoroutine(theSprite.HalfSpriteChangeCoroutine(dialogues[lineCount].spriteName[contextCount].Split(new char[] { '/' })[0]));
                    }
                }
            }

        }
        void PlaySound()
        {
            if (dialogues[lineCount].VoiceName[contextCount] != "")
            {
                SoundManager.instance.PlaySound(dialogues[lineCount].VoiceName[contextCount], 2);
            }
        }
    void DisappearObjects(bool p_value)
    {
        for (int i = 0; i < theDM.characters.Length; i++)
        {
            theDM.characters[i].SetActive(p_value);
        }
    }
}

