using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseChipsScript : MonoBehaviour
{
    public GameObject PurchasePopUp;
    public GameObject PurchasePopUpContent;
    public GameObject PurchasePopUpBackground;
    public GameObject PopUPAmount;
    public GameObject PurchasePanel;
    public GameObject ScrollPanel;
    public float duration = 0.25f;
    

    public static PurchaseChipsScript instance;
    private void Awake()
    {
        instance = this;
        PurchasePopUp.SetActive(false);
        PurchasePopUp.GetComponent<Transform>().localScale = new Vector3(1,0,1);
        PurchasePopUpBackground.SetActive(false);
        PurchasePanel.SetActive(false);
        PurchasePanel.GetComponent<CanvasGroup>().alpha = 0;
        ScrollBarReset();
    }
    
    public void ScrollBarReset()
    {
        ScrollPanel.GetComponent<ScrollRect>().horizontalNormalizedPosition = 0;
    }
    public void ShowPurchasePopUP()
    {
        PurchasePopUpBackground.SetActive(true);
        PurchasePopUp.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(PurchasePopUpContent.GetComponent<RectTransform>());
        PurchasePopUp.GetComponent <Transform>().DOScaleY(1f,duration);
    }
    public void ClosePurchasePopUP()
    {
        PurchasePopUp.GetComponent<Transform>().DOScaleY(0, duration);
        StartCoroutine(SetActive(PurchasePopUp));
        StartCoroutine(SetActive(PurchasePopUpBackground)); 
    }
    public void ShowPurchaseChipUI()
    {
        ScrollBarReset();
        PurchasePanel.SetActive(true);
        DOTween.To(() => PurchasePanel.GetComponent<CanvasGroup>().alpha, x => PurchasePanel.GetComponent<CanvasGroup>().alpha = x, 1, duration);
    }
    public void ClosePurchaseUI()
    {
        ScrollBarReset();
        DOTween.To(() => PurchasePanel.GetComponent<CanvasGroup>().alpha, x => PurchasePanel.GetComponent<CanvasGroup>().alpha = x, 0, duration);
        StartCoroutine(SetActive(PurchasePanel));
        HomeScript.instance.cardAnimator.enabled = true;
    }
    IEnumerator SetActive(GameObject panel)
    {
        yield return new WaitForSeconds(duration);
        panel.SetActive(false);
    }
 
}
