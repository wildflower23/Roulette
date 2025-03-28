using UnityEngine;
using DG.Tweening;
using System.Collections;
using System;

public class BallManager : MonoBehaviour {
    
    public bool spinning = false;
    public Rigidbody ball;
    public Transform resultPoint;
    public Transform originPoint;

    public Transform pivotTransform;
    public Transform pivotWheelTransform;

    private float ballTimeSpeed = 1.3f;

    public Wheel wheel;

    private Transform Target;

    private static readonly Vector3 axis = Vector3.up;
    private float angularSpeed = 5f;
    private bool stopping = false;

    private Vector3 deltaAngularCross = Vector3.zero;

    private bool trigger_animateBall = true;

    private int res = -1;

    void Start () {
        ball.isKinematic = true;
    }

    public void StartSpin()
    {
        ball.isKinematic = true;
        ball.transform.SetParent(originPoint);
        ball.transform.localPosition = Vector3.zero;
        transform.SetParent(pivotTransform);
        transform.localRotation = Quaternion.identity;
        angularSpeed = 7;
        spinning = true;
        trigger_animateBall = true;
    }
    
    public void FindNumber(int result, bool isEuropean)
    {
        result = result == -1 && !isEuropean ? 37 : result;
        Debug.Log("Result====" + result);
        Target = wheel.resultCheckerObject[result].transform;
        res = result;
        DOTween.To(() => angularSpeed, x => angularSpeed = x, 1.5f, 5).OnComplete(() =>
        {
            stopping = true;
        });
    }

    private bool bouncing = false;
    public void PlaceToResult(float angleRatio)
    {
        ball.transform.SetParent(resultPoint);
        Vector3 direction = (Target.position - resultPoint.position);
        bouncing = true;
        SettingScript.instance.StopSFXSound();
        SettingScript.instance.PlaySFXSound("ballhitting", true);
        StartCoroutine(BounceSound());
        ball.transform.DOLocalJump(Vector3.zero, .04f, 5, ballTimeSpeed).SetEase(Ease.Linear).OnComplete(() => { bouncing = false; });
    }

    private IEnumerator BounceSound()
    {
        while (bouncing)
        {
            yield return new WaitForSeconds(.3f);
            SettingScript.instance.StopSFXSound();
        }
    }

    private float CalculateAngleRatio(Vector3 angularCross)
    {
        deltaAngularCross = angularCross - deltaAngularCross;

        Vector3 targetVector = (Target.position - transform.position);
        Vector3 ballVector = (ball.position - transform.position);
       

        targetVector.y = ballVector.y = 0;

        return (Vector3.Angle(ballVector, targetVector) / 180f);
    }

    private void FixedUpdate()
    {
        if (!spinning)
            return;

        transform.Rotate(axis, angularSpeed);

        if (stopping)
        {
            Vector3 angularCross = Vector3.Cross(transform.forward, (Target.position - transform.position).normalized);
            float angle = Vector3.SignedAngle(transform.forward, (Target.position - transform.position), transform.up);
            float angleRatio = CalculateAngleRatio(angularCross);
            if (deltaAngularCross.y > 0f)
            {
                if(angle < 35 && angle > 0)
                    angularSpeed = angleRatio * 2f;
                

                if (angleRatio <= 0.2f && trigger_animateBall && angle > 5)
                {
                    trigger_animateBall = false;
                    PlaceToResult(angleRatio);
                }
                else if (angleRatio <= 0.01f && !trigger_animateBall)
                {
                    spinning = false;
                    transform.SetParent(pivotWheelTransform);
                    ball.isKinematic = false;
                    stopping = false;
                    GameManager.SetGameActivity(GameManager.GameActivity.calculation);
                    Ball.instance.FinalOutputNumber = res;
                    Ball.instance.FinalColor = Ball.instance.Color[res];
                    Ball.instance.FinalOutputDisplay();
                    BettingCalculation.instance.BetResult();
                    BettingCalculation.instance.finalOutput();
                    if (res == 37)
                    {
                        res =00;
                    }
                    PlayScript.instance.AddOutputList(Convert.ToString(res));
                    StartCoroutine(showOutput());
                  


                }
            }
            Debug.DrawRay(transform.position, angularCross, Color.white);
            Debug.DrawRay(transform.position, (Target.position - transform.position), Color.yellow);
            Debug.DrawRay(ball.transform.position, (Target.position - resultPoint.position), Color.green);
        }
    }
    IEnumerator showOutput()
    {
        yield return new WaitForSeconds(2f);
        if (SettingScript.instance._3DCameraViewOn)
        {
            CameraScript.instance.OnSpinEnd3DCameraMovement();
        }
        else
        {
            CameraScript.instance.OnSpinEnd2DCameraMovement();
        }

        yield return new WaitForSeconds(1.25f);
        PlayScript.instance.ResultDisplayPanel();
        PlayScript.instance.ShowResultPanelUI();
        yield return new WaitForSeconds(0.5f);
        WinScript.instance.DestroyChips();

        yield return new WaitForSeconds(4f);
        GameManager.SetGameActivity(GameManager.GameActivity.calculation);
       

        yield return new WaitForSeconds(1.5F);
        PlayScript.instance.CloseResultPanel();
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(ShowBettingStartPopUp());

    }
    IEnumerator ShowBettingStartPopUp()
    {
        yield return new WaitForSeconds(0.25f);
       PlayScript.instance.ShowStartBettingPopUP();
        yield return new WaitForSeconds(1.5f);
        PlayScript.instance.CloseStartBettingPopUP();
        
        yield return new WaitForSeconds(0.1f);
        PlayScript.instance.ShowCoinPanelAnimation();
        GameManager.SetGameActivity(GameManager.GameActivity.Playing);
    }
  
}
