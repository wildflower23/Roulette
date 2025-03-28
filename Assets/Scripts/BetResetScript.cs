using UnityEngine;

public class BetResetScript : MonoBehaviour
{
    public GameObject[] Betting_Buttons;
    public bool isbetted=false;
    public static BetResetScript instance;
    private void Awake()
    {
        instance = this;
    }
    public void ResetBetValues()
    {
        foreach(var button in Betting_Buttons)
        {
            button.GetComponent<ControlerButton>().BetAmount = 0;
            button.GetComponent<ControlerButton>().coincount = 0;
        }
        foreach (var item in ButtonManager.instance.Betting_List)
        {
            item.BetValue = 0;
        }
    }
    public void CheckBetValue()
    {
        foreach (var item in ButtonManager.instance.Betting_List)
        {
            if(item.BetValue > 0)
            {
                isbetted = true;
                break;
            }
            else
            {
                isbetted= false;
            }
        }
    }
}
