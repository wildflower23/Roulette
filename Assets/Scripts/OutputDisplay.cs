using UnityEngine;
using DG.Tweening;
using System.Collections;
using System;
using Random = UnityEngine.Random;


public class Ball : MonoBehaviour
{

    public GameObject OutputSprite2D;
    public GameObject OutputSprite3D;
    
   
    public int FinalOutputNumber;
    public Sprite[] Output_Sprites;
    public static Ball instance;
   
    public float duration = 1f;
    public string[] Color;
    public string FinalColor;
    public float Outputscale;


    private void Awake()
    {
        instance = this;
        OutputSprite2D.transform.localScale = Vector3.zero;
        OutputSprite3D.transform.localScale = Vector3.zero;
    }


    public void FinalOutputDisplay()
    {

        if (SettingScript.instance._3DCameraViewOn)
        {
            Outputscale = 0.5f;
            OutputSprite3D.GetComponent<SpriteRenderer>().sprite = Output_Sprites[FinalOutputNumber];
            OutputSprite3D.transform.DOScale(Outputscale, 0.25f);
            StartCoroutine(OutputDisplay(OutputSprite3D));
        }
        else
        {
            Outputscale = 0.6f;
            OutputSprite2D.GetComponent<SpriteRenderer>().sprite = Output_Sprites[FinalOutputNumber];
            OutputSprite2D.transform.DOScale(Outputscale, 0.25f);
            StartCoroutine(OutputDisplay(OutputSprite2D));
        }
       
        
    }
    IEnumerator OutputDisplay(GameObject outputSprite)
    {
        yield return new WaitForSeconds(2f);
        outputSprite.transform.DOScale(0f, 0.25f);

    }
   

}
