using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Unity.VisualScripting;

public class BettingCalculation : MonoBehaviour
{
    public GameObject BettingText;
    public GameObject TotalAmountText;
   
    public float betwin;
    public float TotalAmount;
  
    public float BettingAmount;
    public bool BettingEnabled;
    public float[] Amount;
    public float Result=0;
    public float[] First_Row;
    public float[] Second_Row;
    public float[] Third_Row;
    public string[] nameArray;
    //Result Variabless
    public float TotalWin;
    public float TotalLoss;
    public float FinalAmount;
    //public GameObject chipdisplay;

    public static BettingCalculation instance;

    private void Awake()
    {
        instance = this;
        BettingEnabled = true;
        
        if (PlayerPrefs.GetString("TotalAmount")=="")
        {
            PlayerPrefs.SetString("TotalAmount", "1000");
        }
        TotalAmount = Convert.ToInt32(PlayerPrefs.GetString("TotalAmount"));
        TotalAmountText.GetComponent<Text>().text = TotalAmount.ToString();
    }
    public void AddBettingAmount(int id)
    {
        
        BettingAmount = BettingAmount + Amount[id];
        DisplayBettingAmount();
        CheckBettingEnabled(id);
    }
    public void CheckBettingEnabled(int id)
    {
        if (Amount[id] > TotalAmount - BettingAmount )
        {
            BettingEnabled = false;
        }
        else
        {
            BettingEnabled = true;
        }
    }
    public void DisplayBettingAmount()
    {
        if (BettingAmount <= TotalAmount)
        {
            BettingText.GetComponent<Text>().text = BettingAmount.ToString();
            TotalAmountText.GetComponent<Text>().text = (TotalAmount - BettingAmount).ToString();
            
        }
        else
        {
            BettingAmount = Convert.ToInt32(BettingText.GetComponent<Text>().text);
        }
    }

    //Result calculation
    public void ResultCalculation(int amount,float betValue)
    {   
               Result = (betValue * amount)+Result+betValue;
        betwin=betValue+betwin;
    }
    public void lossCalculation(float betValue)
    {
        TotalLoss=TotalLoss + betValue;
    }
    public void finalOutput()
    {
        //if (TotalLoss <= 0)
        //{
            TotalAmount = TotalAmount-BettingAmount;
        //}
        //else
        //{
        //    TotalAmount = TotalAmount - TotalLoss;
        //}
       
        //Debug.Log("TotalAmount=====::"+TotalAmount);
        if (Result <= 0)
        {
            TotalWin = 0;
        }
        else
        {
            TotalWin = Mathf.Abs(betwin - Result);
        }  
        FinalAmount = TotalAmount + Result;
        //TotalAmountText.GetComponent<Text>().text = (FinalAmount).ToString();
        TotalAmount = FinalAmount;
       PlayerPrefs.SetString("TotalAmount",TotalAmount.ToString()); 
    }
    
    public void BetResult()
    {

        var Outsideitems = (from aa in ButtonManager.instance.Betting_List
                            where aa.Type== "Outside"
                            select aa).ToList();
        var Insideitems = (from aa in ButtonManager.instance.Betting_List
                     where aa.Type == "InsideSingle"
                     select aa).ToList();
        var Combo = (from aa in ButtonManager.instance.Betting_List
                     where aa.Type == "Combo"
                     select aa).ToList();
        //InsideBet 35to1
        foreach (var item in Insideitems)
        {
            if (Convert.ToInt32(item.Name) == Ball.instance.FinalOutputNumber)
            {
                ResultCalculation(35, item.BetValue);
                WinScript.instance.WinButton_List.Add(item.Name);
            }
            else
            {
                lossCalculation(item.BetValue);
            }
        }
        //Outside Bet  1to1
        for (int i = 0; i < Outsideitems.Count; i++)
        {
            var item = Outsideitems[i];
            
            if (item.BetValue > 0 && Ball.instance.FinalOutputNumber!=0 && Ball.instance.FinalOutputNumber != 37)
            {
                //RedButton 1to1
                if (item.Name== "RedButton")
                {
                    if (Ball.instance.FinalColor == "Red")
                    {
                        ResultCalculation(1, item.BetValue);
                        WinScript.instance.WinButton_List.Add(item.Name);
                    }
                    else
                    {
                        lossCalculation(item.BetValue);
                    }
                }
                else if (item.Name == "BlackButton")//BlackButton 1to1
                {
                    if (Ball.instance.FinalColor == "Black")
                    {
                        ResultCalculation(1, item.BetValue);
                        WinScript.instance.WinButton_List.Add(item.Name);
                    }
                    else
                    {
                        lossCalculation(item.BetValue);
                    }
                }
                else if (item.Name== "EvenButton") //Even Odd Button 1to1
                {
                    float number = Ball.instance.FinalOutputNumber % 2;

                    if (number == 0)
                    {
                        ResultCalculation(1, item.BetValue);
                        WinScript.instance.WinButton_List.Add(item.Name);
                    }
                    else
                    {
                         lossCalculation(item.BetValue);
                    }
                }
                else if (item.Name == "OddButton")//Odd Odd Button 1to1
                {
                    float number = Ball.instance.FinalOutputNumber % 2;

                    if (number != 0)
                    {
                        ResultCalculation(1, item.BetValue);
                        WinScript.instance.WinButton_List.Add(item.Name);
                    }
                    else
                    {
                        lossCalculation(item.BetValue);
                    }
                }
                else if(item.Name == "1to18Button")//1to18Button 1to1
                {
                    if(Ball.instance.FinalOutputNumber >0 && Ball.instance.FinalOutputNumber < 19)
                    {
                        ResultCalculation(1, item.BetValue);
                        WinScript.instance.WinButton_List.Add(item.Name);
                    }
                    else
                    {
                        lossCalculation(item.BetValue);
                    }
                }
                else if (item.Name == "19to36Button")//19t36Button 1to1
                {
                    if (Ball.instance.FinalOutputNumber > 18 && Ball.instance.FinalOutputNumber < 37)
                    {
                        ResultCalculation(1, item.BetValue);
                        WinScript.instance.WinButton_List.Add(item.Name);
                    }
                    else
                    {
                        lossCalculation(item.BetValue);
                    }
                }
                else if (item.Name == "First12Button")//1St12Button 2to1
                {
                    if (Ball.instance.FinalOutputNumber > 0 && Ball.instance.FinalOutputNumber < 13)
                    {
                        ResultCalculation(2, item.BetValue);
                        WinScript.instance.WinButton_List.Add(item.Name);
                    }
                    else
                    {
                        lossCalculation(item.BetValue);
                    }
                }
                else if (item.Name == "Second12Button")//2nd12Button 2to1
                {
                    if (Ball.instance.FinalOutputNumber > 12 && Ball.instance.FinalOutputNumber < 25)
                    {
                        ResultCalculation(2, item.BetValue);
                        WinScript.instance.WinButton_List.Add(item.Name);
                    }
                    else
                    {
                        lossCalculation(item.BetValue);
                    }
                }
                else if (item.Name == "Third12Button")//3rd12Button 2to1
                {
                    if (Ball.instance.FinalOutputNumber > 24 && Ball.instance.FinalOutputNumber < 37)
                    {
                        ResultCalculation(2, item.BetValue);
                        WinScript.instance.WinButton_List.Add(item.Name);
                    }
                    else
                    {
                        lossCalculation(item.BetValue);
                    }
                }
                else if(item.Name == "FirstRowButton")//1strow 2to1
                {
                    if (First_Row.Contains(Ball.instance.FinalOutputNumber))
                    {
                        ResultCalculation(2, item.BetValue);
                        WinScript.instance.WinButton_List.Add(item.Name);
                    }
                    else
                    {
                        lossCalculation(item.BetValue);
                    }
                }
                else if (item.Name == "SecondRowButton")//2ndrow 2to1
                {
                    if (Second_Row.Contains(Ball.instance.FinalOutputNumber))
                    {
                        ResultCalculation(2, item.BetValue);
                        WinScript.instance.WinButton_List.Add(item.Name);
                    }
                    else
                    {
                        lossCalculation(item.BetValue);
                    }
                }
                else if (item.Name == "ThirdRowButton")//3rdrow 2to1
                {
                    if (Third_Row.Contains(Ball.instance.FinalOutputNumber))
                    {
                        ResultCalculation(2, item.BetValue);
                        WinScript.instance.WinButton_List.Add(item.Name);
                    }
                    else
                    {
                        lossCalculation(item.BetValue);
                    }
                }
                
            }
        }

        foreach(var item in Combo)
        {
            if(item.BetValue > 0)
            {
                
                for (int i = 0; i < item.Name.Length; i = i + 2)
                {
                    var str = item.Name.Substring(i,2);
                    var str1 = item.Name.Substring(0, 2);
                    if (i== 0)
                    {
                        continue;
                    }
                    if (Convert.ToInt32(str) == Ball.instance.FinalOutputNumber)
                    {
                        WinScript.instance.WinButton_List.Add(item.Name);
                        if (str1 == "2C")//17to1
                        {
                            ResultCalculation(17, item.BetValue);
                            break;
                        }
                        else if (str1 == "4C")//8to1
                        {
                            ResultCalculation(8, item.BetValue);
                            break;

                        }
                        else if (str1 == "3C")//11to1
                        {
                            ResultCalculation(11, item.BetValue);
                            break;

                        }
                        else if (str1 == "5C")//6to1
                        {
                            ResultCalculation(6, item.BetValue);
                            break;

                        }
                        else if (str1 == "6C")//5to1
                        {
                            ResultCalculation(5, item.BetValue);
                            break;
                        }
                    }
                   if(i == item.Name.Length - 2)
                    {
                        lossCalculation(item.BetValue);
                    }
                }
                
            }
        }
    }
}
  