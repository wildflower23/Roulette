using DG.Tweening;
using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class SettingScript : MonoBehaviour
{
    public GameObject SettingPanel;
    public GameObject SettingBackground;

    //Sound Variable
    [Header("Toggle----")]
    public GameObject backgroungtoggle;
    public GameObject sfxtoggle;
    public GameObject backgroungtogglebackground;
    public GameObject sfxtogglebackground;
    public bool isBackgroundSoundOn;
    public bool isSFXSoundOn;
    public float ToggleDistance = 50;
    public AudioSource BackgroundAudio;
    public AudioSource SFXAudio;

    //Camera View Bool
    public GameObject cameratoggle;
    public GameObject cameratogglebackground;
    public bool _3DCameraViewOn;
    public float duration=0.25f;

    private string previous_State;
    public static SettingScript instance;

  
    private void Awake()
    {
        instance= this;
    }
    private void Start()
    {
        if(PlayerPrefs.GetString("BackgroundSound") == "")
        {
            PlayerPrefs.SetString("BackgroundSound", "false");
        }
        if (PlayerPrefs.GetString("SFXSound") == "")
        {
            PlayerPrefs.SetString("SFXSound", "false");
        }
        SettingPanel.SetActive(false);
        SettingBackground.SetActive(false);
        BackgroundAudio = gameObject.AddComponent<AudioSource>();
        SFXAudio = gameObject.AddComponent<AudioSource>();
        BackgroundAudio.clip = (AudioClip)Resources.Load("Audio/Casino Music_Brass Funk Fashion Music by Rovador (No Copyright_FREE download)");
        BackgroundAudio.loop = true;
        setboolvalue();
        //isBackgroundSoundOn=Convert.ToBoolean(PlayerPrefs.GetString("BackgroundSound"));
        //isSFXSoundOn = Convert.ToBoolean(PlayerPrefs.GetString("SFXSound"));
        CheckbackgroundSound(isBackgroundSoundOn, backgroungtoggle,backgroungtogglebackground, BackgroundAudio, "BackgroundSound");
        CheckbackgroundSound(isSFXSoundOn, sfxtoggle,sfxtogglebackground ,SFXAudio, "SFXSound");
        SettingPanel.GetComponent<Transform>().localScale = Vector3.zero;
    }
    
    public void OnBackgroundSoundButton()
    {
        isBackgroundSoundOn = !isBackgroundSoundOn;
        if (isBackgroundSoundOn)
        {
            movetoggleright(backgroungtoggle, ToggleDistance);
            backgroungtogglebackground.GetComponent<RawImage>().color = Color.green;
            PlayerPrefs.SetString("BackgroundSound", "true");
            BackgroundAudio.Play();
        }
        else
        {
            movetoggleright(backgroungtoggle, -ToggleDistance);
            backgroungtogglebackground.GetComponent<RawImage>().color = Color.white;
            PlayerPrefs.SetString("BackgroundSound", "false");
            BackgroundAudio.Stop();
        }
    }
    public void OnSFXSoundButton()
    {
        isSFXSoundOn = !isSFXSoundOn;
        if (isSFXSoundOn)
        {
            movetoggleright(sfxtoggle, ToggleDistance);
            sfxtogglebackground.GetComponent<RawImage>().color = Color.green;
            PlayerPrefs.SetString("SFXSound", "true");
            
        }
        else
        {
            movetoggleright(sfxtoggle, -ToggleDistance);
            sfxtogglebackground.GetComponent<RawImage>().color= Color.white;
            PlayerPrefs.SetString("SFXSound", "false");
            
        }
    }

    public void CheckbackgroundSound(Boolean tooglebool, GameObject togglename, GameObject bgtogglename, AudioSource audioname, string name)
    {
        tooglebool = Convert.ToBoolean(PlayerPrefs.GetString(name));

        if (tooglebool)
        {
            movetoggleright(togglename, ToggleDistance);
            bgtogglename.GetComponent<RawImage>().color = Color.green;
            PlayerPrefs.SetString(name, "true");
            if(name == "BackgroundSound")
            {
                audioname.Play();
            } 
        }
        else
        {
            movetoggleright(togglename, -ToggleDistance);
            bgtogglename.GetComponent<RawImage>().color = Color.white;
            PlayerPrefs.SetString(name, "false");
            if (name == "BackgroundSound")
            {
                audioname.Stop();
            }
        }
    }
    public void PlaySFXSound(string AudioName, bool loopOn)
    {
        if (isSFXSoundOn)
        {
            SFXAudio.clip = (AudioClip)Resources.Load("Audio/" + AudioName);
            SFXAudio.Play();
            if (loopOn)
            {
                SFXAudio.loop = true;
            }
            else
            {
                SFXAudio.loop = false;
            }
        }
    }
    public void StopSFXSound()
    {
        SFXAudio.Stop();
    }
    //CameraView
    public void _3DCameraButton()
    {
        _3DCameraViewOn = !_3DCameraViewOn;
        if (_3DCameraViewOn)
        {
            movetoggleright(cameratoggle, ToggleDistance);
            cameratogglebackground.GetComponent<RawImage>().color=Color.green; 
        }
        else
        {
            movetoggleright(cameratoggle, -ToggleDistance);
            cameratogglebackground.GetComponent<RawImage>().color = Color.white;
        }
    }
    public void movetoggleright(GameObject toggle, float distance)
    {
        toggle.transform.DOLocalMoveX(distance, 0.2f);
    }
   

    public void ShowSettingUI()
    {
        SettingBackground.SetActive(true);
        SettingPanel.SetActive(true);
        StartCoroutine(SettingUiAnimation(1,duration));
        SettingPanel.GetComponent<Transform>().DOScale(1.05f, duration);
       
        previous_State = GameManager.gameActivity.ToString();
        //GameManager.SetGameActivity(GameManager.GameActivity.Setting);
        setboolvalue();
        //isBackgroundSoundOn = Convert.ToBoolean(PlayerPrefs.GetString("BackgroundSound"));
        //isSFXSoundOn = Convert.ToBoolean(PlayerPrefs.GetString("SFXSound"));
        CheckbackgroundSound(isBackgroundSoundOn, backgroungtoggle,backgroungtogglebackground, BackgroundAudio, "BackgroundSound");
        CheckbackgroundSound(isSFXSoundOn, sfxtoggle,sfxtogglebackground ,SFXAudio, "SFXSound");
    }
    IEnumerator SettingUiAnimation(float scale,float duration)
    {
         yield return new WaitForSeconds(duration);
        SettingPanel.GetComponent<Transform>().DOScale(scale, duration);
    }
   
    public void CloseSettingUI()
    {
        Debug.Log("!!!!!!!!!!!!!!!!!!!!!!Setting Close");
        StartCoroutine(SettingUiAnimation(0, 0.24f));
        SettingPanel.GetComponent<Transform>().DOScale(1.05f, duration);
       
        StartCoroutine(setactive(SettingPanel));
        StartCoroutine(setactive(SettingBackground));
        if (previous_State == "Home")
        {
            GameManager.SetGameActivity(GameManager.GameActivity.Home);
        }
        else
        {
            GameManager.SetGameActivity(GameManager.GameActivity.Playing);
        }
        if (GameManager.gameActivity == GameManager.GameActivity.Playing)
        {
            Debug.Log("!!!!!!!!!!!!!!!!!!!Camera movement started");
            CameraScript.instance.DoCamaeraMovement();
        }
        CheckbackgroundSound(isBackgroundSoundOn, backgroungtoggle,backgroungtogglebackground, BackgroundAudio, "BackgroundSound");
        CheckbackgroundSound(isSFXSoundOn, sfxtoggle, sfxtogglebackground, SFXAudio, "SFXSound");
        setboolvalue();
        //isBackgroundSoundOn = Convert.ToBoolean(PlayerPrefs.GetString("BackgroundSound"));
        //isSFXSoundOn = Convert.ToBoolean(PlayerPrefs.GetString("SFXSound"));


    }
    public void setboolvalue()
    {
        string boolvalue = PlayerPrefs.GetString("BackgroundSound");
        if (boolvalue == "true")
        {
            isBackgroundSoundOn = true;
        }
        else
        {
            isBackgroundSoundOn = false;
        }
        string boolvalue1 = PlayerPrefs.GetString("SFXSound");
        if (boolvalue == "true")
        {
            isSFXSoundOn = true;
        }
        else
        {
            isSFXSoundOn = false;
        }
    }
    IEnumerator setactive(GameObject panel)
    {
        yield return new WaitForSeconds(duration*2);
        panel.SetActive(false);
    }   
}
