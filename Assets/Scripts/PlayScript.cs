using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using System.Linq;
using System;
using static GameManager;
public class PlayScript : MonoBehaviour
{
    public GameObject PlayPanel;
    //POP UP VARIABLE
    public GameObject StartBettingPopUp;
    public GameObject NotEnoughChipsPopUP;
    public GameObject NotEnoughChipsPopUPBackground; 
    public GameObject BetFirstPopUP;
    public GameObject BetFirstBackground;
    //Chips Variable
   
    public Sprite[] chipImages;
    public Animator Chip_animator;
    public int ChipId;
    public GameObject CoinPanel;
    public float random;
    public int counter = 0;
    public int[] value = { 1, 5, 10, 50, 100, 500, 1000 };
    //ResultPanelVariable
    public GameObject ResultPanel;
    public GameObject TotalBid;
    public GameObject TotalWin;
    public GameObject TotalLoss;
    public GameObject FinalAmount;
    public GameObject DisplayOutput;

    public Text[] OutputTexts;
    public float duration = 0.25f;

    public EuropeanWheel EuropeanWheelspin;
    public AmericanWheel AmericanWheelspin;

    public static PlayScript instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        
        NotEnoughChipsPopUP.GetComponent<Transform>().localScale = Vector3.zero;
        NotEnoughChipsPopUP.SetActive(false);
        NotEnoughChipsPopUPBackground.SetActive(false);
       
        BetFirstPopUP.GetComponent<Transform>().localScale = Vector3.zero;
        BetFirstPopUP.SetActive(false);
        BetFirstBackground.SetActive(false);
        
        StartBettingPopUp.GetComponent<Transform>().localScale = new Vector3(1, 0, 1);
        StartBettingPopUp.SetActive(false);
        
        ResultPanel.GetComponent<CanvasGroup>().alpha = 0;
        ResultPanel.SetActive(false);

        CloseCoinPanelAnimation();
        //Debug.Log(" CoinPanel:" + CoinPanel.transform.position);

        PlayPanel.GetComponent<CanvasGroup>().alpha = 0;
        PlayPanel.SetActive(false);
    }

    //Display Previous Output
    public void AddOutputList(string name)
    {
        
        if(counter >= 6)
        {
            counter = 0;
        }
        var i = Convert.ToInt32(name);
        if (Ball.instance.Color[i] == "Red")
        {
            OutputTexts[counter].color = Color.red;
            OutputTexts[counter].alignment=TextAnchor.UpperLeft;
            OutputTexts[counter].text = name;
        }
        else if (Ball.instance.Color[i] == "Black")
        {
            OutputTexts[counter].color = Color.yellow;
            OutputTexts[counter].alignment = TextAnchor.UpperRight;
            OutputTexts[counter].text = name;
        }
        else
        {

            OutputTexts[counter].color = Color.green;
            OutputTexts[counter].alignment = TextAnchor.MiddleCenter;
            OutputTexts[counter].text = name;
        }

        counter++;
    }
    //ResultPanel
    public void ResultDisplayPanel()
    {
        TotalBid.GetComponent<Text>().text = BettingCalculation.instance.BettingAmount.ToString();
        TotalWin.GetComponent<Text>().text = BettingCalculation.instance.TotalWin.ToString();
        TotalLoss.GetComponent<Text>().text = BettingCalculation.instance.TotalLoss.ToString();
        FinalAmount.GetComponent<Text>().text = BettingCalculation.instance.FinalAmount.ToString();
    }

    // Button Events
  
  
    public void SpinButton()
    {
        if (GameManager.gameActivity == GameManager.GameActivity.Playing)
        {
            if (BettingCalculation.instance.BettingAmount > 0)
            {
                CloseCoinPanelAnimation();
                GameManager.SetGameActivity(GameActivity.Spinnning);
                //print("Spin American");
               
                if (SettingScript.instance._3DCameraViewOn)
                {
                    CameraScript.instance.OnSpin3DCameraMovement();
                }
                else
                {
                    CameraScript.instance.OnSpin2DCameraMovement();
                }

                if (HomeScript.instance.isEuropean)
                {
                    EuropeanWheelspin.Spin();
                }
                else
                {
                    AmericanWheelspin.Spin();
                }
                BettingCalculation.instance.BettingText.GetComponent<Text>().text ="0";
            }
            else
            {
                ShowBetFirstPopUP();
            }
        }
        else
        {
            return;
        }
       
       
    }
    public void HomeButton()
    {
        if (GameManager.gameActivity == GameManager.GameActivity.Playing)
        {
            BetResetScript.instance.CheckBetValue();

            if (BetResetScript.instance.isbetted)
            {
                ExitScript.instance.ShowExitUI(ExitScript.instance.ExitPanel_Bet);
            }
            else
            {
                ExitScript.instance.ShowExitUI(ExitScript.instance.ExitPanel_NoBet);
            }
        }
        else
        {
            return;
        }

        
    }
    public void SettingButton()
    {
        if (GameManager.gameActivity == GameManager.GameActivity.Playing)
        {
            SettingScript.instance.ShowSettingUI();
        }else
        {
            return;
        }
       
    }
    public void PurchaseButton()
    {
        StartCoroutine(purchasePanel());
    }
    IEnumerator purchasePanel()
    {
        yield return new WaitForSeconds(duration * 2);
        PurchaseChipsScript.instance.ShowPurchaseChipUI();
    }
    //ResetButton
    public void ResetButton()
    {
        BettingCalculation.instance.BettingAmount = 0;
        BettingCalculation.instance.Result = 0;
        BettingCalculation.instance.TotalLoss = 0;
        BettingCalculation.instance.BettingAmount = 0;
        BettingCalculation.instance.DisplayBettingAmount();
        BettingCalculation.instance.TotalWin = 0;
        BettingCalculation.instance.FinalAmount = 0;
        BettingCalculation.instance.betwin = 0;
        ControlerButton.instance.coincount = 0;
        BetResetScript.instance.ResetBetValues();    
        WinScript.instance.gameobj.Clear();
        WinScript.instance.Winchipobj.Clear();
        WinScript.instance.WinButton_List.Clear();
        ControlerButton.instance.coincount = 0;
      
        for(int i=0;i<7;i++)
        {
            WinScript.instance.chip[i] = 0;
        }
       //StartCoroutine(set_state());
        HomeScript.instance.TotalChipsUpdate();
    }
  
    

//Chip Selection
public void ChipSelectionButton(int id)
    {
         ChipId = id;
    }

    IEnumerator SetActive(GameObject panel,float duration)
    {
        yield return new WaitForSeconds(duration);
        panel.SetActive(false);
    }
    public void ShowPlayUI()
    {
     
        

        PlayPanel.SetActive(true);
       StartCoroutine(startbetting());  
        DOTween.To(() => PlayPanel.GetComponent<CanvasGroup>().alpha, x => PlayPanel.GetComponent<CanvasGroup>().alpha = x, 1, duration);
        
   }
    IEnumerator startbetting()
    {
        yield return new WaitForSeconds(duration);
        ShowStartBettingPopUP();
        yield return new WaitForSeconds(1f);
        CloseStartBettingPopUP();
        yield return new WaitForSeconds(0.1f);
        ShowCoinPanelAnimation();
        GameManager.SetGameActivity(GameManager.GameActivity.Playing);
    }
   
    public void ClosePlayUI()
    {
        CloseCoinPanelAnimation();
        DOTween.To(() => PlayPanel.GetComponent<CanvasGroup>().alpha, x => PlayPanel.GetComponent<CanvasGroup>().alpha = x, 0, duration);
        counter = 0;
        foreach (Text i in OutputTexts)
        {
            i.text = "";
        }
        StartCoroutine(SetActive(PlayPanel,duration));
    }
   
    public void ShowStartBettingPopUP()
    {
        StartBettingPopUp.SetActive(true);  
        StartBettingPopUp.GetComponent<Transform>().DOScaleY(1f,duration);
    }
    public void CloseStartBettingPopUP()
    {
        StartCoroutine(SetActive(StartBettingPopUp,duration));
        StartBettingPopUp.GetComponent<Transform>().DOScaleY(0f,duration);
    }

    public void ShowResultPanelUI()
    {
       ResultPanel.SetActive(true);
        DOTween.To(() => ResultPanel.GetComponent<CanvasGroup>().alpha, x => ResultPanel.GetComponent<CanvasGroup>().alpha = 1, 0, 0.2f);
    }
    public void CloseResultPanel()
    {
        StartCoroutine(SetActive(ResultPanel,duration));
        DOTween.To(() => ResultPanel.GetComponent<CanvasGroup>().alpha, x => ResultPanel.GetComponent<CanvasGroup>().alpha = 0, 0, 0.2f);
    }
    public void ShowNotEnoughChipsPopUP()
    {
        NotEnoughChipsPopUPBackground.SetActive(true);
        NotEnoughChipsPopUP.SetActive(true);
        StartCoroutine(UiAnimation(1, NotEnoughChipsPopUP, duration));
        NotEnoughChipsPopUP.GetComponent<Transform>().DOScale(1.05f, duration);
    }
    public void CloseNotEnoughChipsPopUP()
    {
        StartCoroutine(SetActive(NotEnoughChipsPopUP, duration * 2));
        StartCoroutine(SetActive(NotEnoughChipsPopUPBackground, duration * 2));
        StartCoroutine(UiAnimation(0, NotEnoughChipsPopUP, 0.24f));
        NotEnoughChipsPopUP.GetComponent<Transform>().DOScale(1.05f, duration); 
    } 
    public void ShowBetFirstPopUP()
    {
        BetFirstBackground.SetActive(true);
        BetFirstPopUP.SetActive(true);
        StartCoroutine(UiAnimation(1, BetFirstPopUP,duration));
        BetFirstPopUP.GetComponent<Transform>().DOScale(1.05f, duration);
    }
    public void CloseBetFirstPopUP()
    {
        
        StartCoroutine(SetActive(BetFirstPopUP, duration * 2));
        StartCoroutine(SetActive(BetFirstBackground, duration * 2));
        StartCoroutine(UiAnimation(0, BetFirstPopUP,0.24f));
        BetFirstPopUP.GetComponent<Transform>().DOScale(1.05f, duration);
       
    }
    IEnumerator UiAnimation(float scale, GameObject name,float duration)
    {
        yield return new WaitForSeconds(duration);
        name.GetComponent<Transform>().DOScale(scale, duration);
    }

    public void ShowCoinPanelAnimation()
    {
        StartCoroutine(coinpanelanimation(-460f, 1f));

    }public void CloseCoinPanelAnimation()
    {
        StartCoroutine(coinpanelanimation(-750f, 0f));

    }
    IEnumerator coinpanelanimation(float distance,float alpha)
    {
        yield return null;
        CoinPanel.GetComponent<Transform>().DOLocalMove(new Vector3(553, distance, 0), 0.5f);
       yield return new WaitForSeconds(0.25f);
        DOTween.To(() => CoinPanel.GetComponent<CanvasGroup>().alpha, x => CoinPanel.GetComponent<CanvasGroup>().alpha = alpha, 0, duration);
    }

}
