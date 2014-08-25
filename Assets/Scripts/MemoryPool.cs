using UnityEngine;
using System.Collections;



//-------------------------------------------------------------------------
// 메모리 풀 클래스
// 용도 : 특정 게임오브젝트를 실시간으로 생성과 삭제하지 않고, 
//      : 미리 생성해 둔 게임오브젝트를 재활용하는 클래스입니다.
//-------------------------------------------------------------------------
public class MemoryPool : IEnumerable, System.IDisposable
{


    //-------------------------------------------------------------------------
    // 아이템 클래스
    //-------------------------------------------------------------------------
    class Item
    {
        public bool active; // 사용중인지 여부
        public GameObject gameObject;
    }

    Item[] table;



    //-------------------------------------------------------------------------
    // 열거자 기본 재정의
    //-------------------------------------------------------------------------
    public IEnumerator GetEnumerator()
    {
        if (table == null)
            yield break;

        int count = table.Length;

        for (int i = 0; i < count; i++)
        {
            Item item = table[i];

            if (item.active)
                yield return item.gameObject;
        }
    }



    //-------------------------------------------------------------------------
    // 메모리 풀 생성
    // original : 미리 생성해 둘 원본소스
    // count : 풀 최고 갯수
    //-------------------------------------------------------------------------
    public void Create(Object original, int count)
    {
        Dispose();
        table = new Item[count];

        for (int i = 0; i < count; i++)
        {
            Item item = new Item();
            item.active = false; // 미사용 상태
            item.gameObject = GameObject.Instantiate(original) as GameObject;
            item.gameObject.SetActive(false); // 보이지 않게
            table[i] = item;
        }
    }



    //-------------------------------------------------------------------------
    // 새 아이템 요청 - 쉬고 있는 객체를 반환한다.
    //-------------------------------------------------------------------------
    public GameObject NewItem()
    {
        if (table == null)
            return null;

        int count = table.Length;

        for (int i = 0; i < count; i++)
        {
            Item item = table[i];
            if (item.active == false)
            {
                item.active = true;
                item.gameObject.SetActive(true);
                return item.gameObject;
            }
        }

        return null;
    }



    //-------------------------------------------------------------------------
    // 아이템 사용종료 - 사용하던 객체를 쉬게한다.
    // gameObject : NewItem으로 얻었던 객체
    //-------------------------------------------------------------------------
    public void RemoveItem(GameObject gameObject)
    {
        if (table == null || gameObject == null)
            return;

        int count = table.Length;

        for (int i = 0; i < count; i++)
        {
            Item item = table[i];
            if (item.gameObject == gameObject)
            {
                item.active = false;
                item.gameObject.SetActive(false);
                break;
            }
        }
    }



    //-------------------------------------------------------------------------
    // 모든 아이템 사용종료 - 모든 객체를 쉬게한다.
    //-------------------------------------------------------------------------
    public void ClearItem()
    {
        if (table == null)
            return;

        int count = table.Length;

        for (int i = 0; i < count; i++)
        {
            Item item = table[i];

            if (item != null && item.active)
            {
                item.active = false;
                item.gameObject.SetActive(false);
            }
        }
    }



    //-------------------------------------------------------------------------
    // 메모리 풀 삭제
    //-------------------------------------------------------------------------
    public void Dispose()
    {
        if (table == null)
            return;

        int count = table.Length;

        for (int i = 0; i < count; i++)
        {
            Item item = table[i];
            GameObject.Destroy(item.gameObject);
        }

        table = null;
    }



    //-------------------------------------------------------------------------
}




//-------------------------------------------------------------------------
//사용예제
//-------------------------------------------------------------------------
// public class Test : MonoBehaviour
// {
//     public GameObject monsterPrefab; // 복제 할 원본소스
//     MemoryPool pool = new MemoryPool();
// 
//     void Start()
//     {
//         int poolCount = 100;
//         pool.Create(monsterPrefab, poolCount); // 메모리 풀 생성
//     }
// 
//     void OnApplicationQuit()
//     {
//         pool.Dispose(); // 메모리 풀 삭제
//     }
// 
//     void TestFunc()
//     {
//         GameObject monster1 = pool.NewItem(); // 객체 활성화
//         GameObject monster2 = pool.NewItem(); // 객체 활성화
// 
//         // 사용하는 코드는 ~ 여기저기~~ 막 그냥~!
// 
//         if (monster1 != null)
//         {
//             pool.RemoveItem(monster1); // 객체비활성화
//         }
//     }
// 
// 
// }

//-------------------------------------------------------------------------


