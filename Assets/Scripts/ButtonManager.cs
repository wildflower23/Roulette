using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    
    public enum ButtonColor { Red , Black ,None };
    public enum ButtonType { InsideSingle , Combo , Outside };

    public static ButtonManager instance;
    private void Awake()
    {
        instance = this;
    }
    public class Betting_Button
    {
        public string Color;
        public string Name;
        public string Type;
        public float BetValue;
       
    }
   public List<Betting_Button> Betting_List = new List<Betting_Button>();

   
}
