using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public GameObject[] enemyObjects;

    private GameObject parentObject;
    private float speed;
    
    void Awake()
    {
        
    }

	// Use this for initialization
	void Start () 
    {
        parentObject = GameObject.Find("BG1");
        LoopMoveBG comp = parentObject.GetComponent<LoopMoveBG>();
        speed = comp.Speed;
	}
	
	// Update is called once per frame
	void Update () 
    {
        transform.Translate(speed * Time.deltaTime, 0, 0);
	}

    public void RandomEnemyPosition()
    {
        Vector3 position;

        for(int index = 0; index < enemyObjects.Length; ++index)
        {
            /*enemyObjects[index].transform.localPosition += new Vector3(0, Random.Range(-2, 3) * 130.0f, 0);*/
            position = enemyObjects[index].transform.localPosition;
            position.Set(position.x, Random.Range(-2, 3) * 130.0f, position.z);
            enemyObjects[index].transform.localPosition = position;
        }
    }
}
