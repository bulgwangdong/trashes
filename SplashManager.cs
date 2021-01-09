using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashManager : MonoBehaviour
{
    [SerializeField] Image image;

    [SerializeField] Color colorWhite;
    [SerializeField] Color colorBlack;

    [SerializeField] float fadeSpeed;
    [SerializeField] float fadeSlowSpeed;

    public static bool isfinish = true;

    SpriteManager spriteManager;

    private void Start()
    {
        spriteManager = FindObjectOfType<SpriteManager>();
    }


    public IEnumerator Splash()
    {
        isfinish = false;
        StartCoroutine(FadeOut(true, false,false));
        yield return new WaitUntil(() => isfinish);
        isfinish = false;
        StartCoroutine(FadeIn(true, false));


    }

    public IEnumerator FadeOut(bool _isWhite, bool _isSlow , bool _isDisappearSprite)
    {
        Color t_color = (_isWhite == true) ? colorWhite : colorBlack;

            t_color.a = 0;
        if (_isDisappearSprite == true)
        {
            StartCoroutine(spriteManager.HalfSpriteDisappearCoroutine());
        }

        while (t_color.a < 1)
        {
            t_color.a += (_isSlow == true) ? fadeSlowSpeed : fadeSpeed;
            image.color = t_color;
            yield return null;
        }

        isfinish = true;


    }
    public IEnumerator FadeIn(bool _isWhite, bool _isSlow)
    {

        Color t_color = (_isWhite == true) ? colorWhite : colorBlack;
        t_color.a = 1;

        while (t_color.a > 0)
        {
            t_color.a -= (_isSlow == true) ? fadeSlowSpeed : fadeSpeed;
            image.color = t_color;
            yield return null;
        }
        isfinish = true;
    }


}
