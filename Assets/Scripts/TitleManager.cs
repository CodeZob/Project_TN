using UnityEngine;
using System.Collections;

public class TitleManager : MonoBehaviour 
{
    public AudioClip playBgm;
    public AudioClip damageEffect;

    public GameObject soundOff;
    public GameObject soundOn;

    void Awake()
    {
        if (PlayerPrefs.GetInt("Sound") == 0)
            SoundOff();
        else
            SoundOn();

        DontDestroyOnLoad(gameObject);
    }

	// Use this for initialization
	void Start () 
    {
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void GameStart()
    {
        if (audio.enabled)
        {
            audio.clip = playBgm;
            audio.Play();
        }
        Application.LoadLevel("Play");
    }

    public void SoundOn()
    {
        PlayerPrefs.SetInt("Sound", 1);
        soundOff.SetActive(true);
        soundOn.SetActive(false);
        audio.enabled = true;
    }

    public void SoundOff()
    {
        PlayerPrefs.SetInt("Sound", 0);
        soundOff.SetActive(false);
        soundOn.SetActive(true);
        audio.enabled = false;
    }
}
