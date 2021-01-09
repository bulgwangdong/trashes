using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    Transform tf_Target;

    bool spin = false;
    public static bool isFinished = true;

    private void Start()
    {
        tf_Target = PlayerController.instance.transform;
    }
    void Update()
    {
        if (tf_Target != null)
        {
            if (!spin)
            {
                Quaternion t_rotation = Quaternion.LookRotation(tf_Target.position - transform.position);
                Vector3 t_Euler = new Vector3(0, t_rotation.eulerAngles.y, 0);
                transform.eulerAngles = t_Euler;
            }
            else
            {
                transform.Rotate(0, 90 * Time.deltaTime * 8, 0);
            }

        }
    }

        public IEnumerator SetAppearOrDisappear(bool p_flag)
        {
            spin = true;

            SpriteRenderer[] t_SpriteRenderers = GetComponentsInChildren<SpriteRenderer>();

            Color t_FrontColor = t_SpriteRenderers[0].color;
            Color t_RearColor = t_SpriteRenderers[1].color;

            if (p_flag)
            {
                t_FrontColor.a = 0; t_RearColor.a = 0;
                t_SpriteRenderers[0].color = t_FrontColor; t_SpriteRenderers[1].color = t_RearColor;
            }

            float t_FadeSpeed = (p_flag == true) ? 0.005f : -0.005f;

            yield return new WaitForSeconds(0.5f);

            while (true)
            {   
                if (p_flag && t_FrontColor.a >= 1) break;
                else if (!p_flag && t_FrontColor.a <= 0) break;
                t_FrontColor.a += t_FadeSpeed; t_RearColor.a += t_FadeSpeed;
                t_SpriteRenderers[0].color = t_FrontColor; t_SpriteRenderers[1].color = t_RearColor;
                yield return null;
            }

            spin = false;
            isFinished = true;
            gameObject.SetActive(p_flag);
        }
    
}
