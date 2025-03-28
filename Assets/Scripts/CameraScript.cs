using DG.Tweening;
using System;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
public class CameraScript : MonoBehaviour
{
    public GameObject _2Dcamera;
    public GameObject _3Dcamera;
    public GameObject main_camera;
    private float duration = 1.5f;
    public static CameraScript instance;

    private void Awake()
    {
        instance = this;
    }

    public void OnSpin2DCameraMovement()
    {
        _2Dcamera.GetComponent<Camera>().enabled = true;
        main_camera.GetComponent<Camera>().enabled = false;
        CameraMovement(_2Dcamera, 1.3f, new Vector3(90, 0, 0), new Vector3(-0.11f, 2.5f, -0.37f));
    }
    public void OnSpin3DCameraMovement()
    {
        main_camera.GetComponent<Camera>().enabled = false;
        _3Dcamera.GetComponent<Camera>().enabled = true;
        _3Dcamera.transform.DOMove(new Vector3(0f, 3.72f, -4.77f), duration);
        _3Dcamera.transform.DOLocalRotate(new Vector3(38, 0, 0), duration);
        _3Dcamera.GetComponent<Camera>().DOFieldOfView(16f, duration);
       

    }
    public void FinalPosition2DMain_Camera()
    {
        //main_camera.GetComponent<Camera>().DOFieldOfView(21.5f, duration);
        main_camera.GetComponent<Camera>().DOFieldOfView(28f, 0.2f);
        //StartCoroutine(OrthoOn());
        //Camera.main.orthographic = true;
        CameraMovement(main_camera, 2.1f, new Vector3(90, 0, 0), new Vector3(2.83f, 10.64f, 0.23f)); 
    }
    //IEnumerator OrthoOn()
    //{
    //    yield return new WaitForSeconds(1f);
    //    Camera.main.orthographic = true;
    //}
    public void FinalPosition3DMain_Camera()
    {
        main_camera.GetComponent<Camera>().DOFieldOfView(10f, duration);
       
        main_camera.transform.DOMove(new Vector3(3.8f, 7.9f, -12.42f), duration);
        main_camera.transform.DOLocalRotate(new Vector3(32, 0, 0), duration);
        //Camera.main.orthographic = false;
        //StartCoroutine(OrthoOff());


    }
    //IEnumerator OrthoOff()
    //{
    //    yield return new WaitForSeconds(0.5f);
    //    Camera.main.orthographic = false;
    //}
    public void OnSpinEnd2DCameraMovement()
    {
        CameraMovement(_2Dcamera, 2.1f, new Vector3(90, 0, 0), new Vector3(2.83f, 10.64f, 0.23f));
        StartCoroutine(setActiveCamera(_2Dcamera));
    }
    public void OnSpinEnd3DCameraMovement()
    {
        _3Dcamera.transform.DOMove(new Vector3(3.8f, 7.9f, -12.42f), duration);
        _3Dcamera.GetComponent<Camera>().DOFieldOfView(10f, duration);
        _3Dcamera.transform.DOLocalRotate(new Vector3(32, 0, 0), duration);
        StartCoroutine(setActiveCamera(_3Dcamera));
    }
    public void CameraMovement(GameObject _camera, float orthoSize, Vector3 rotateangle, Vector3 distance)
    {
        _camera.transform.DOLocalRotate(rotateangle, duration);
        _camera.GetComponent<Camera>().DOOrthoSize(orthoSize, duration);
       
        _camera.GetComponent<Camera>().transform.DOMove(distance, duration);

    }
    IEnumerator setActiveCamera(GameObject _camera)
    {
        yield return new WaitForSeconds(duration);
        main_camera.GetComponent<Camera>().enabled = true;
        _camera.GetComponent<Camera>().enabled = false;
    }
    public void DoCamaeraMovement()
    {
        if (SettingScript.instance._3DCameraViewOn)
        {
           
            Camera.main.orthographic = false;
            FinalPosition3DMain_Camera();
        }
        else
        {
            Camera.main.orthographic = true;
            FinalPosition2DMain_Camera();
        }
    }
    public void OriginalCameraPostion()
    {
        Camera.main.orthographic = false;
        main_camera.transform.DOMove(new Vector3(2.22f, 7.75f, -11.16f), duration);
        main_camera.GetComponent<Camera>().DOFieldOfView(23f, duration);
        main_camera.transform.DOLocalRotate(new Vector3(38f, 0f, 0f), duration);

    }
}
