using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    float applySpeed;
    [SerializeField] float fieldLookLimitX;

    [SerializeField] float fieldSensetivity;

    [SerializeField] RectTransform tf_Crosshair;
    [SerializeField] RectTransform uiCanvas;
    [SerializeField] Transform tf_Cam;
    [SerializeField] float sightSensitivity;
    [SerializeField] float lookLimitX;
    [SerializeField] float lookLimitY;
    Vector2 anchoredPos;
    float currentAngleX;
    float currentAngleY;
    [SerializeField] float sightMoveSpeed;
    [SerializeField] Vector2 camBoundary;
    [SerializeField] Camera cam;



    // Update is called once per frame
    void Update()
    {
        if (!InteractionControler.isInteract && !EHandBookManager.isPaused)
        {
            if (CameraController.onlyView)
            {

                CrosshairMoving();
                ViewMoving();
                KeyViewMoving();
                CameraLimit();

            }
            else
            {
                FieldMoving();
                FieldLooking();
            }
        }


    }

    void FieldMoving()
    {
        if(Input.GetAxisRaw("Horizontal")!= 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            Vector3 t_Dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                applySpeed = runSpeed;
            }
            else
            {
                applySpeed = walkSpeed;
            }

            transform.Translate(t_Dir * applySpeed * Time.deltaTime, Space.Self);
        }

    }

    void FieldLooking()
    {
        if(Input.GetAxisRaw("Mouse X") != 0)
        {
            float t_AngleY = Input.GetAxisRaw("Mouse X");
            Vector3 t_Rot = new Vector3(0, t_AngleY * fieldSensetivity, 0);
            transform.rotation = Quaternion.Euler(transform.localEulerAngles + t_Rot);
        }
        if(Input.GetAxisRaw("Mouse Y") != 0)
        {
            float t_AngleX = Input.GetAxisRaw("Mouse Y");
            currentAngleX -= t_AngleX;

            currentAngleX = Mathf.Clamp(currentAngleX, -fieldLookLimitX, fieldLookLimitX);
            tf_Cam.localEulerAngles = new Vector3(currentAngleX, 0, 0);
        }
    }
    void CameraLimit()
    {
        if(tf_Cam.localPosition.x >= camBoundary.x){
            tf_Cam.localPosition = new Vector3(camBoundary.x, tf_Cam.localPosition.y, tf_Cam.localPosition.z);
        }
        else if (tf_Cam.localPosition.x <= -camBoundary.x)
        {
            tf_Cam.localPosition = new Vector3(-camBoundary.x, tf_Cam.localPosition.y, tf_Cam.localPosition.z);
        }
        if (tf_Cam.localPosition.y >= 1+camBoundary.y)
        {
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x, 1+camBoundary.y, tf_Cam.localPosition.z);
        }
        else if (tf_Cam.localPosition.y <= 1 - camBoundary.y)
        {
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x, 1-camBoundary.y, tf_Cam.localPosition.z);
        }
    }

    void CrosshairMoving()
    {

            RectTransformUtility.ScreenPointToLocalPointInRectangle(uiCanvas, Input.mousePosition, null, out anchoredPos);


            tf_Crosshair.anchoredPosition = anchoredPos;

            float t_CoursorPosX = tf_Crosshair.anchoredPosition.x;
            float t_CoursorPosY = tf_Crosshair.anchoredPosition.y;

            t_CoursorPosX = Mathf.Clamp(t_CoursorPosX, -Screen.width / 2 + 50, Screen.width / 2 - 50);
            t_CoursorPosY = Mathf.Clamp(t_CoursorPosY, -Screen.height / 2 + 50, Screen.height / 2 - 50);

            tf_Crosshair.anchoredPosition = new Vector2(t_CoursorPosX, t_CoursorPosY);



    }


    void ViewMoving()
    {
        if(tf_Crosshair.anchoredPosition.x > (Screen.width/2 - 100) || tf_Crosshair.anchoredPosition.x < (-Screen.width / 2 + 100))
        {
            currentAngleY += tf_Crosshair.anchoredPosition.x > 0 ? sightSensitivity : -sightSensitivity;
            currentAngleY = Mathf.Clamp(currentAngleY, -lookLimitX, lookLimitX);

            float t_applySpeed = (tf_Crosshair.anchoredPosition.x > 0) ? sightMoveSpeed : -sightMoveSpeed;
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x + t_applySpeed, tf_Cam.localPosition.y, tf_Cam.localPosition.z);
        
        }
        else if (tf_Crosshair.anchoredPosition.y > (Screen.height / 2 - 100) || tf_Crosshair.anchoredPosition.y < (-Screen.height / 2 + 100))
        {
            currentAngleX += tf_Crosshair.anchoredPosition.y > 0 ? -sightSensitivity : sightSensitivity;
            currentAngleX = Mathf.Clamp(currentAngleX, -lookLimitY, lookLimitY);
            float t_applySpeed = (tf_Crosshair.anchoredPosition.y > 0) ? sightMoveSpeed : -sightMoveSpeed;
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x, tf_Cam.localPosition.y + t_applySpeed, tf_Cam.localPosition.z);
        }
        tf_Cam.localEulerAngles = new Vector3(currentAngleX, currentAngleY, tf_Cam.localEulerAngles.z);
        
    }
    void KeyViewMoving()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            currentAngleY += (sightSensitivity * Input.GetAxisRaw("Horizontal"));
            currentAngleY = Mathf.Clamp(currentAngleY, -lookLimitX, lookLimitX);
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x + (sightMoveSpeed * Input.GetAxisRaw("Horizontal")), tf_Cam.localPosition.y, tf_Cam.localPosition.z);
        }
        if (Input.GetAxisRaw("Vertical") != 0)
        {
            currentAngleX += -(sightSensitivity * Input.GetAxisRaw("Vertical"));
            currentAngleX = Mathf.Clamp(currentAngleX, -lookLimitY, lookLimitY);
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x , tf_Cam.localPosition.y + (sightMoveSpeed * Input.GetAxisRaw("Horizontal")), tf_Cam.localPosition.z);
        }
        tf_Cam.localEulerAngles = new Vector3(currentAngleX, currentAngleY, tf_Cam.localEulerAngles.z);
    }

    public void Reset()
    {
        tf_Crosshair.localPosition = Vector3.zero;
        currentAngleX = 0;
        currentAngleY = 0;
    }
}
