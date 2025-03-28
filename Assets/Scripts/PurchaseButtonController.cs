using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseButtonController : MonoBehaviour
{
    public GameObject Purchased_Amount;
    private float duration = 0.25f;
    public void PurchaseButton()
    {
       
        BettingCalculation.instance.TotalAmount = BettingCalculation.instance.TotalAmount+Convert.ToInt32(Purchased_Amount.GetComponent<Text>().text);
       
        BettingCalculation.instance.TotalAmountText.GetComponent<Text>().text = BettingCalculation.instance.TotalAmount.ToString();
       PlayerPrefs.SetString("TotalAmount", BettingCalculation.instance.TotalAmount.ToString());

        BettingCalculation.instance.DisplayBettingAmount();
        HomeScript.instance.TotalChipsUpdate();

        PurchaseChipsScript.instance.PopUPAmount.GetComponent<Text>().text=Purchased_Amount.GetComponent<Text>().GetComponent<Text>().text;

        StartCoroutine(ShowPopup());
        PurchaseChipsScript.instance.ClosePurchaseUI();
        
        
    }
    IEnumerator ShowPopup()
    {
        yield return new WaitForSeconds(duration);
        //Debug.Log("Shoe popup");
        PurchaseChipsScript.instance.ShowPurchasePopUP();
    }
    

}
