using DG.Tweening;
using UnityEngine;
using System.Collections;
using System;
public class ExitScript : MonoBehaviour
{
    public GameObject ExitPanel_NoBet;
    public GameObject ExitPanel_Bet;
    public GameObject ExitPanelBackground;

    public float duration = 0.25f;

    public static ExitScript instance;
    private void Awake()
    {
        instance = this;
        ExitPanel_NoBet.transform.localScale = Vector3.zero;
        ExitPanel_Bet.transform.localScale = Vector3.zero;
        ExitPanel_NoBet.SetActive(false);
        ExitPanel_Bet.SetActive(false);
        ExitPanelBackground.SetActive(false);
       
    }
    public void ShowExitUI(GameObject name)
    {
        name.SetActive(true);
        ExitPanelBackground.SetActive(true);
        name.transform.DOScale(1.05f, duration);
        StartCoroutine(UiAnimation(1, name,duration));
       
    }  public void CloseExitUI(GameObject name)
    {
        StartCoroutine(UiAnimation(0, name,0.24f));
        name.transform.DOScale(1.05f,duration);
        StartCoroutine(setactive(name));
        StartCoroutine(setactive(ExitPanelBackground));
       
    }
    IEnumerator UiAnimation(float scale,GameObject name,float duration)
    {
        yield return new WaitForSeconds(duration);
        name.GetComponent<Transform>().DOScale(scale, duration);
    }
    IEnumerator setactive(GameObject panel)
    {
        yield return new WaitForSeconds(duration*2);
        panel.SetActive(false);
    }

    public void Yes_NotBetButton()
    {
        StartCoroutine(OpenHomePanel());
        CloseExitUI(ExitPanel_NoBet);
        PlayScript.instance.ClosePlayUI();
        GameManager.SetGameActivity(GameManager.GameActivity.Home);
        
        // HomeScript.instance.Table.SetActive(false);
    }
    public void Yes_BetButton()
    {
        StartCoroutine(OpenHomePanel());
        CloseExitUI(ExitPanel_Bet);
        BettingCalculation.instance.lossCalculation(BettingCalculation.instance.BettingAmount);
        BettingCalculation.instance.finalOutput();
        WinScript.instance.DestroyChips();
        PlayScript.instance.ResetButton();
         PlayScript.instance.ClosePlayUI(); 
       
        GameManager.SetGameActivity(GameManager.GameActivity.Home);
        // HomeScript.instance.Table.SetActive(false);
    }
    IEnumerator OpenHomePanel()
    {
        yield return new WaitForSeconds(duration*2);
        HomeScript.instance.ShowHomeUI();
    }
}
