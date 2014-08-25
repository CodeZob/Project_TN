using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour 
{
    float _halfHeight;

    public float speed = 5f;
    public int hp = 100;
    public int hpDam = 5;
    public Animator anim;
    public GameObject originalEffect;

    private ObjectPool effectPool;
    private GameObject damageEffect;

    private float effectDestroyTime = 5.0f;
    private float effectTime = 0.0f;

    public UISprite uiGuage;

    public TitleManager audioManager;

    void Awake()
    {
        audioManager = GameObject.Find("TitleManager").GetComponent<TitleManager>();
        effectPool = new ObjectPool(originalEffect, 10);
    }

	// Use this for initialization
	void Start () 
    {
        _halfHeight = Screen.height >> 1;
	}

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {
            float deltaPosY = Input.GetTouch(0).position.y - _halfHeight;
            float posY = deltaPosY - transform.localPosition.y;
            transform.Translate(0, speed * Time.deltaTime * posY * 0.01f, 0);
        }

        if(Input.GetMouseButton(0))
        {
            float deltaPosY = Input.mousePosition.y - _halfHeight;
            float posY = deltaPosY - transform.localPosition.y;
            transform.Translate(0, speed * Time.deltaTime * posY * 0.01f, 0);
        }

        transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Clamp(transform.localPosition.y, -270.0f, 250.0f), transform.localPosition.z);

        effectTime += Time.deltaTime;

        if(effectTime > effectDestroyTime)
        {
            effectPool.Reset();
            effectTime = 0.0f;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        hp -= hpDam;
        uiGuage.fillAmount = hp * 0.01f;

        GameObject.Find("GameManager").SendMessage("LevelReset", SendMessageOptions.DontRequireReceiver);

        if(hp > 0)
        {
            anim.SetBool("IsDamage", true);

            damageEffect = effectPool.GetGameObject(this.gameObject.transform);
            damageEffect.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            GameObject.Find("GameManager").SendMessage("GameOver", SendMessageOptions.DontRequireReceiver);
        }
    }

    void DamageEnd()
    {
        anim.SetBool("IsDamage", false);
    }

    void DamageSound()
    {
        if (audioManager.audio.enabled)
        {
            audioManager.audio.PlayOneShot(audioManager.damageEffect);
        }
    }

    void OnApplicationQuit()
    {
        effectPool.Dispose();
    }
}
