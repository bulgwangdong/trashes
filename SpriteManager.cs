using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteManager : MonoBehaviour
{
    [SerializeField] float fadeSpeed;
    [SerializeField] public Image halfRenderer;

    public static bool isFinish = true;


    public IEnumerator SpriteChangeCoroutine(Transform p_Target, string p_SpriteName)
    {
        SpriteRenderer[] t_spriteRenderer = p_Target.GetComponentsInChildren<SpriteRenderer>();
        Sprite t_sprite = Resources.Load("Character/" + p_SpriteName,typeof(Sprite)) as Sprite;
        
        if (t_sprite != null)
        {
            if (!CheckSameSprite(t_spriteRenderer[0], t_sprite))
            {
                Color t_color = t_spriteRenderer[0].color;
                Color t_ShadowColor = t_spriteRenderer[1].color;
                t_color.a = 0;
                t_spriteRenderer[0].color = t_color;
                t_spriteRenderer[1].color = t_ShadowColor;

                t_spriteRenderer[0].sprite = t_sprite;
                t_spriteRenderer[1].sprite = t_sprite;


                    while (t_color.a < 1)
                    {
                        t_color.a += fadeSpeed;
                        t_ShadowColor.a += fadeSpeed;
                        t_spriteRenderer[0].color = t_color;
                        t_spriteRenderer[1].color = t_ShadowColor;

                        yield return null;
                    }
            }
        }
    }

    public IEnumerator HalfSpriteChangeCoroutine(string p_SpriteName)
    {
        Sprite t_sprite = Resources.Load<Sprite>("Character/" + p_SpriteName);

        if (t_sprite != null)
        {
            if (halfRenderer.sprite != t_sprite)
            {
                halfRenderer.sprite = t_sprite;

                Color t_color = halfRenderer.color;

             

                while (t_color.a < 1)
                {
                    t_color.a += fadeSpeed;
                    
                    halfRenderer.color = t_color;
                    Debug.Log(halfRenderer.color);
                    yield return null;
                }
            }
        }

    }

    public IEnumerator HalfSpriteDisappearCoroutine(float p_speed = 0.1f)
    {
        Debug.Log("a");
        isFinish = false;
        Color t_color = halfRenderer.color;
        while (t_color.a > 0)
        {
            t_color.a -= p_speed;
            halfRenderer.color = t_color;

            yield return null;
        }
        halfRenderer.sprite = null;
        isFinish = true;
    }

    

    bool CheckSameSprite(SpriteRenderer p_spriteRenderer, Sprite p_Sprite)
    {
        if (p_spriteRenderer.sprite == p_Sprite)
            return true;
        else
            return false;
    }
}
