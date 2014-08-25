using UnityEngine;
using System.Collections;

public class LoopMoveBG : MonoBehaviour 
{

    // 해당 오브젝트의 초기 위치
    Vector3 _initPosition;
    
    // 끝 위치 X 값
    float _endX;

    // 이동속도
    public float speed;

    public float Speed
    {
        get
        {
            return speed;
        }
        set
        {
            speed = value;
        }
    }

    void Awake()
    {
        _initPosition = transform.localPosition;
        _endX = -2560.0f;
        speed = -speed;
    }

	// Use this for initialization
	void Start () 
    {
        
	}
	
	// Update is called once per frame
	void Update () 
    {
        transform.Translate(speed * Time.smoothDeltaTime, 0, 0);

        if(transform.localPosition.x < _endX)
        {
            transform.localPosition = _initPosition;
        }
	}
}
