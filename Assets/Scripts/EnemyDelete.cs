using UnityEngine;
using System.Collections;

public class EnemyDelete : MonoBehaviour {

    public GameManager gm;

	// Use this for initialization
	void Start () 
    {
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void OnTriggerEnter(Collider other)
    {
        gm.SendMessage("DestroyEnemy", other.gameObject);
    }
}
