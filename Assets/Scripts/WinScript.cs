using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ButtonManager;

public class WinScript : MonoBehaviour
{
    public GameObject ChipWinPoint2d;
    public GameObject ChipWinPoint3d;
    public GameObject ChipDestroyPoint;
    public GameObject ChiptCollectionPoint;
    public GameObject ChipPrefab;
    public GameObject Americanparentobj;
    public GameObject Europeanparentobj;
    public GameObject parentobj;
    public static WinScript instance;
    private Vector3 pos;
    
    public float[] chip;
    public List<GameObject> gameobj = new List<GameObject>();
    public List<GameObject> Winchipobj = new List<GameObject>();
    public List<string> WinButton_List = new List<string>();
    public float total;
    private void Awake()
    {
        instance=this;
    }
    public void DestroyChips()
    {
        if (HomeScript.instance.isEuropean)
        {
            parentobj = Europeanparentobj;
        }
        else
        {
            parentobj = Americanparentobj;
        }
        //foreach (GameObject go in gameobj)
        for(int i=0;i<gameobj.Count;i++) 
        {
            if (WinButton_List.Count > 0)
            {
                if (WinButton_List.Contains(gameobj[i].name))
                {
                    Winchipobj.Add(gameobj[i]);
                        //Debug.Log(gameobj[i].name);
                }
                else
                {
                    gameobj[i].transform.DOMove(ChipDestroyPoint.transform.position, 0.5f);
                    StartCoroutine(destroyChip(gameobj[i]));

                }
            }
            else
            {
                gameobj[i].transform.DOMove(ChipDestroyPoint.transform.position, 0.5f);
                StartCoroutine(destroyChip(gameobj[i]));
            }
        }
        total = BettingCalculation.instance.TotalWin;
       StartCoroutine(StartCalculation());   
    }
    IEnumerator destroyChip(GameObject obj)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(obj);
    }
    IEnumerator StartCalculation()
    {
        yield return new WaitForSeconds(1f);
        chipCalculation();
    }
    public void chipMovementToCollection()
    {
        foreach (GameObject go in Winchipobj)
        {
            Vector3 chipos = ChiptCollectionPoint.transform.position + Random.insideUnitSphere * 0.5f;
            go.transform.DOMove(chipos, 1f);
        }
       
    }
   public void chipCalculation()
    {
        
        for (int i = 6; i >= 0; i--)
        {
           
            while (total >= BettingCalculation.instance.Amount[i])
            {
                total -= BettingCalculation.instance.Amount[i];
                chip[i]=chip[i]+1;
            }
        }
        for(int i = 0; i < chip.Length; i++)
        {
            if(chip[i] > 0)
            {
                for(int j = 0; j < chip[i]; j++)
                {
                    Vector3 chipos = ChipDestroyPoint.transform.position + Random.insideUnitSphere * 0.5f;
                    ChipPrefab.GetComponent<Image>().sprite = PlayScript.instance.chipImages[i];
                   GameObject chip= Instantiate(ChipPrefab, chipos, Quaternion.identity,parentobj.transform.parent);
                    Winchipobj.Add(chip);
                }
            }
        }
        StartCoroutine(Movechip());
        StartCoroutine(winpos());
    }
    public void movetochipWinposition()
    {
        if (SettingScript.instance._3DCameraViewOn)
        {
            pos=ChipWinPoint3d.transform.position;
        }
        else
        {
            pos = ChipWinPoint2d.transform.position;
        }
        foreach(GameObject go in Winchipobj)
        {
            go.transform.DOMove(pos,1f);
        }
    }
    IEnumerator Movechip()
    {
        yield return new WaitForSeconds(1f);
        chipMovementToCollection();
    }
    IEnumerator winpos()
    {
        yield return new WaitForSeconds(2f);
        movetochipWinposition();
        yield return new WaitForSeconds(0.95f);
        BettingCalculation.instance.TotalAmountText.GetComponent<Text>().text = BettingCalculation.instance.FinalAmount.ToString();
        yield return new WaitForSeconds(0.2f);
        foreach (GameObject go in Winchipobj)
        {
            Destroy(go);
        }
        PlayScript.instance.ResetButton();
    
    }

}
