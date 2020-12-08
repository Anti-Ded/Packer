using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Case : MonoBehaviour
{
    GameMain GameMain;
    GameClients GameClients;
    ItemsBase ItemsBase;
    public bool Open = false;
    public int CaseType = 0;
    public bool spawned = false;
    List<GameObject> AllItems = new List<GameObject>();


    void Start()
    {
        GameMain = FindObjectOfType<GameMain>();
        GameClients = FindObjectOfType<GameClients>();
        ItemsBase = FindObjectOfType<ItemsBase>();

        int Country = GameClients.CurrentClient.GetComponent<Client>().ClientType;
        List<Place> CasePlaces = new List<Place>();
        
        //Создание нового кейса и его наполнение
        CasePlaces.AddRange(FindObjectsOfType<Place>());
        List<GameObject> CountryItems = new List<GameObject>();
        CountryItems.AddRange(ItemsBase.Elements[GameClients.CurrentCountry].Items);
        //в каждрй ячейке создаётся каждый предмет страны, а потом удаляем лишние
        foreach (Place a in CasePlaces)
        {
            for (int i = 0; i < ItemsBase.Elements[GameClients.CurrentCountry].Items.Count; i++)
            {
                GameObject NewItem = Instantiate(CountryItems[i], a.transform);
                NewItem.transform.position = a.transform.position;
                NewItem.transform.eulerAngles = Vector3.zero;
                AllItems.Add(NewItem);
            }
        }
        //все предметы перемешиваются
        AllItems = MyLib.My.Shuffle(AllItems);
        //все предметы сортируются по возрастанию количества требуемых ячеек
        AllItems = AllItems.ToArray().OrderBy(x => x.GetComponent<Item>().PlacesCountNeeded).ToList();
        //разворачиваем, чтобы по убыванию количества требуемых ячеек
        AllItems.Reverse();
        //корутина, чтобы дать время предметам схватиться за ячейки
        StartCoroutine(Prestart());
    }
    
    private IEnumerator Prestart()
    {
        yield return new WaitForSeconds(0.1f);
        
        while (AllItems.Count > 0)
        {
            if (AllItems[0])
            {
                //удаляем тех, у кого количество ячеек меньше, чем надо
                if (AllItems[0].GetComponent<Item>().Places.Count != (2 * AllItems[0].GetComponent<Item>().PlacesCountNeeded))
                {
                    GameObject temp = AllItems[0];
                    AllItems.RemoveAt(0);
                    GameMain.Items.Remove(temp);
                    Destroy(temp);
                    continue;
                }
                foreach (Collider a in AllItems[0].GetComponent<Item>().Colliders)
                {
                    //так как у нас первыми идут самые большие, то они удаляют всех, кто их касается
                    // так же все мелкие и одинаковые перемешаны, потому в каждой клетке остаётся только 1
                    if (a)
                    {
                        Collider temp = a;
                        AllItems.Remove(temp.gameObject);
                        GameMain.Items.Remove(temp.gameObject);
                        Destroy(temp.gameObject);
                    }
                }
                AllItems.RemoveAt(0);
            }
            else AllItems.RemoveAt(0);
        }
        int MoveCount = 2;
        //теперь берём два предмета и ставим их отдельно
        foreach (GameObject a in GameMain.Items)
        {
            a.GetComponent<Item>().Colliders.Clear();
            if (MoveCount > 0)
            {
                MoveCount--;
                if (MoveCount == 1)
                    a.transform.position = new Vector3(-1, 0.6f, -0.3f);
                else
                    a.transform.position = new Vector3(-1, 0.6f, 0.3f);
                a.GetComponent<Item>().Pos = a.transform.position;
                foreach (GameObject b in a.GetComponent<Item>().Places)
                {
                    b.GetComponent<Place>().Item = null;
                }
                a.GetComponent<Item>().Places.Clear();
            }
            else // остальные предметы делаем не движимыми
            {
                a.GetComponent<Item>().Unmovable = true;
                int ClosestPlaceIndex = MyLib.My.Closest(a, a.GetComponent<Item>().Places);
                a.GetComponent<Item>().Place = a.GetComponent<Item>().Places[ClosestPlaceIndex];
                foreach (GameObject b in a.GetComponent<Item>().Places)
                    b.GetComponent<Place>().Item = a;
            }
        }
    }
    void Update()
    {
        if (GameMain.CaseOpen)
        {
            if (!Open)
                GetComponent<Animation>().Play("Open");
                Open = true;
        }
        else
        {
            if (Open)
                GetComponent<Animation>().Play("Close");
            Open = false;
        }
    }
}
