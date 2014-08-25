using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{

    public GameObject enemySet;
    public GameObject nearBG;
    public Transform enemyPos;
    public float spawnTime = 5.0f;

    private ObjectPool enemyPool;
    private GameObject enemy;
    private bool spawnCheck;
    private float spawnTimeCheck;

    public UILabel scoreText;
    public GameObject uiResult;
    public UILabel resultText;

    float timeForLevel = 0.0f;
    public float timeForLevelLim = 5.0f;
    public float initLevelTime = 5.0f;
    public PlayerControl playerCtrl;

	void Awake()
	{
        enemyPool = new ObjectPool(enemySet, 10);
	}
	
	// Use this for initialization
	void Start () 
    {
        spawnCheck = true;
        spawnTimeCheck = 0.0f;
	}
	
	// Update is called once per frame
	void Update () 
    {
        timeForLevel += Time.deltaTime;

        //Debug.Log(timeForLevel);

        if (timeForLevel > timeForLevelLim)
        {
            if (Time.timeScale < 5.0f)
            {
                Time.timeScale *= 1.2f;
                timeForLevelLim *= 1.2f;
                playerCtrl.hpDam++;
                //Debug.Log(Time.timeScale);
            }
            timeForLevel = 0.0f;
        }

        scoreText.text = (Time.timeSinceLevelLoad * 100.0f).ToString("N0");

        if(spawnCheck)
        {
            enemy = enemyPool.GetGameObject(enemyPos, Vector3.zero);

            //enemy.transform.parent = enemyPos;
            enemy.transform.localScale = new Vector3(1, 1, 1);
            //enemy.transform.localPosition = Vector3.zero;
            enemy.GetComponent<Enemy>().SendMessage("RandomEnemyPosition");
            spawnCheck = false;
            spawnTimeCheck = 0.0f;
        }

        if(spawnTimeCheck > spawnTime && !spawnCheck)
        {
            spawnCheck = true;
        }

        spawnTimeCheck += Time.deltaTime;
	}

    public void LevelReset()
    {
        Time.timeScale = 1.0f;
        timeForLevelLim = initLevelTime;
        timeForLevel = 0.0f;
        playerCtrl.hpDam = 5;
    }

    public void DestroyEnemy(GameObject target)
    {
        GameObject set = null;

        if(target.CompareTag("LastEnemy"))
        {
            set = target.transform.parent.gameObject;
            set = set.transform.parent.gameObject;

            enemyPool.Destroy(set);

//             for (int i = 0; i < set.transform.childCount; ++i)
//             {
//                 for (int j = 0; j < set.transform.GetChild(i).childCount; ++j)
//                 {
//                     set.transform.GetChild(i).GetChild(j).gameObject.SetActive(true);
//                 }
//             }
        }
//         else
//         {
//             target.SetActive(false);
//         }
    }

    public void ReStart()
    {
        Application.LoadLevel("Play");
        uiResult.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void GameOver()
    {
        uiResult.SetActive(true);
        resultText.text = "Your Score is\n" + scoreText.text;
        Time.timeScale = 0.0f;
    }

    void OnApplicationQuit()
    {
        enemyPool.Dispose();
    }
}
