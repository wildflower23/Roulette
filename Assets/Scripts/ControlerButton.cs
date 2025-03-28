using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static ButtonManager;
using System.Collections;
using DG.Tweening;

public class ControlerButton : MonoBehaviour
{
    public GameObject prefab;
    public int coincount = 0;
    public ButtonColor Color;
    public ButtonType Type;
    public float BetAmount=0;
    public Vector3 pos;

  public static ControlerButton instance;
    private void Awake()
    {
        instance = this;
        coincount = 0;
    }
    public void Click()
    {
       
        BettingCalculation.instance.CheckBettingEnabled(PlayScript.instance.ChipId);
        if ( GameManager.gameActivity == GameManager.GameActivity.Playing)
        {
            if (BettingCalculation.instance.BettingEnabled)
            {
                BettingCalculation.instance.AddBettingAmount(PlayScript.instance.ChipId);
                if (Type.ToString() == "InsideSingle" || Type.ToString() == "Outside")
                {
                    StartCoroutine(ButtonAnimation(gameObject.name + "/Image", 0.6f));
                }
                if (Type.ToString() == "Outside" || Type.ToString() == "Combo")
                {
                    StartCoroutine(ComboButtonAnimation("NmbersSplit/" + gameObject.name, 0.4f));
                }
                prefab.GetComponent<Image>().sprite = PlayScript.instance.chipImages[PlayScript.instance.ChipId];
                if (coincount == 1)
                {
                    pos = gameObject.transform.position + new Vector3(0.03f, 0, 0);

                }
                else
                {
                    pos = gameObject.transform.position;
                }
                GameObject chipob = Instantiate(prefab, pos, Quaternion.identity, gameObject.GetComponent<RectTransform>());
                coincount++;
                chipob.name = gameObject.name;
                WinScript.instance.gameobj.Add(chipob);

                BetAmount = BetAmount + PlayScript.instance.value[PlayScript.instance.ChipId];
                //adding button to list
                var items = (from Betting_List in ButtonManager.instance.Betting_List
                             where Betting_List.Name == gameObject.name
                             select Betting_List).ToList();
                if (items.Count > 0)
                {
                    foreach (var item in items)
                    {
                        item.BetValue = BetAmount;
                    }
                }
                else
                {
                    ButtonManager.instance.Betting_List.Add(new Betting_Button { Color = Color.ToString(), Type = Type.ToString(), Name = gameObject.name, BetValue = BetAmount });

                }
            }
            else
            {
                PlayScript.instance.ShowNotEnoughChipsPopUP();
            }
        }
       
    }
    IEnumerator ButtonAnimation(string name, float range)
    {
        yield return new WaitForSeconds(0);
        GameObject.Find(name).GetComponent<Image>().DOFade(range, 0.2f);
        yield return new WaitForSeconds(0.2f);
        GameObject.Find(name).GetComponent<Image>().DOFade(0f, 0.2f);

    }
    IEnumerator ComboButtonAnimation(string name, float range)
    {
        yield return new WaitForSeconds(0);
        DOTween.To(() => GameObject.Find(name).GetComponent<CanvasGroup>().alpha, x => GameObject.Find(name).GetComponent<CanvasGroup>().alpha = range, 0, 0.2f);
        yield return new WaitForSeconds(0.2f);
        DOTween.To(() => GameObject.Find(name).GetComponent<CanvasGroup>().alpha, x => GameObject.Find(name).GetComponent<CanvasGroup>().alpha = 0, 0, 0.2f);

    }
    private void Update()
    {
        if (!BettingCalculation.instance.BettingEnabled)
        {
            gameObject.GetComponent<Button>().transition = Selectable.Transition.None;
        }
    }
  
}
