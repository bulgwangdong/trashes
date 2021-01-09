using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotating : MonoBehaviour
{
    [SerializeField] Transform trans;
    [SerializeField] float spinSpeed;
    [SerializeField] Vector3 spinDir;
    void Update()
    {
        Rotate(false);
    }

    public void Rotate(bool p_isContact)
    {
        if (!p_isContact)
        {
            trans.Rotate(spinDir * spinSpeed * Time.deltaTime);
        }

        else
        {
            trans.localEulerAngles = Vector3.zero;

        }
    }
}
