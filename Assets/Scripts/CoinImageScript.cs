using DG.Tweening;
using UnityEngine;
using System.Collections;
public class CoinImageScript : MonoBehaviour
{
    public GameObject image;
    void Start()
    {
        image.GetComponent<Transform>().localRotation=Quaternion.identity;
        SettingScript.instance.PlaySFXSound("Fx_Chips",false);
        image.GetComponent<Transform>().DOScale(1.5f, 0.2f);
        StartCoroutine(doscale());
    }
    IEnumerator doscale()
    {
        yield return new WaitForSeconds(0.25f);
        image.GetComponent<Transform>().DOScale(1f, 0.2f);
    }

}
