using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class HomeScript : MonoBehaviour
{
   
    public GameObject HomePanel;
    public GameObject HowTOPlayPanel;
    public GameObject HowTOPlayPanelContent;
    public GameObject American_Table;
    public GameObject European_Table;
    public GameObject TotalChips;
    public Animator cardAnimator;
    private float duration=0.25f;
   
    public bool isEuropean ;
    public static HomeScript instance;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        ShowHomeUI();
        PlayScript.instance.PlayPanel.SetActive(false);
        HowTOPlayPanelContent.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
        HowTOPlayPanel.GetComponent<CanvasGroup>().alpha = 0f;
        HowTOPlayPanel.SetActive(false );
    }

    //Button Events
    public void StoreButton()
    {
        cardAnimator.enabled = false;
        PurchaseChipsScript.instance.ShowPurchaseChipUI();
    }
    public void TablePickButton(bool value)
    {
        
        isEuropean = value;
        if(isEuropean)
        {
            European_Table.SetActive(true);
            American_Table.SetActive(false);
            PlayButton();
        }
        else
        {
            European_Table.SetActive(false);
            American_Table.SetActive(true);
            PlayButton();
        }
    }
    public void PlayButton()
    {
        CameraScript.instance.DoCamaeraMovement();
        StartCoroutine(ShowUI());
        CloseHomeUI();
        
    }
    IEnumerator ShowUI()
    {
        yield return new WaitForSeconds(1f);
        PlayScript.instance.ShowPlayUI();
       //Table.SetActive(true);
    }
    public void SettingButton()
    {
        SettingScript.instance.ShowSettingUI();
    }

    
    //HomePanel UI Control
    public void CloseHomeUI()
    {
        DOTween.To(() => HomePanel.GetComponent<CanvasGroup>().alpha, x => HomePanel.GetComponent<CanvasGroup>().alpha = x, 0, duration);
        StartCoroutine(SetActive(HomePanel));
        
    }
    public void ShowHomeUI()
    {
        cardAnimator.enabled = true;
        TotalChipsUpdate();
        GameManager.SetGameActivity(GameActivity.Home);
        CameraScript.instance.OriginalCameraPostion();
        //TotalChips.GetComponent<Text>().text = BettingCalculation.instance.TotalAmount.ToString();
        HomePanel.SetActive(true);
        DOTween.To(() => HomePanel.GetComponent<CanvasGroup>().alpha, x => HomePanel.GetComponent<CanvasGroup>().alpha = x, 1, duration);
    }
    IEnumerator SetActive(GameObject panel)
    {
        yield return new WaitForSeconds(duration);
        panel.SetActive(false);
    }
    public void CloseHowtoplayUI()
    {
        HowTOPlayPanelContent.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
        DOTween.To(() => HowTOPlayPanel.GetComponent<CanvasGroup>().alpha, x => HowTOPlayPanel.GetComponent<CanvasGroup>().alpha = x, 0, duration);
        StartCoroutine(SetActive(HowTOPlayPanel));
        
    }
    public void SHowHowtoplayUI()
    {
        
        HowTOPlayPanel.SetActive(true);
        DOTween.To(() => HowTOPlayPanel.GetComponent<CanvasGroup>().alpha, x => HowTOPlayPanel.GetComponent<CanvasGroup>().alpha = x, 1, duration);
        

    }
    public void TotalChipsUpdate()
    {
        TotalChips.GetComponent<Text>().text = BettingCalculation.instance.TotalAmount.ToString();
    }
    
}
