using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClients : MonoBehaviour
{
    GameMain GameMain;
    ItemsBase ItemsBase;
    AllGUI AllGUI;

    public GameObject PackingAgreeButton;
    public GameObject PackingDisagreeButton;

    public GameObject CurrentClient;
    public int CurrentCountry;

    Transform ClientPoint;
    Transform CasePoint;
    public List<GameObject> Clients = new List<GameObject>();

    private GameObject SpawnNewCLient()
    {
        int CountryNumber = Random.Range(0, ItemsBase.Elements.Count);
        int ClientNumber = Random.Range(0, ItemsBase.Elements[CountryNumber].Clients.Count);
        GameObject NewCLient = Instantiate(ItemsBase.Elements[CountryNumber].Clients[ClientNumber], ClientPoint.position, ClientPoint.rotation);
        Clients.Add(NewCLient);
        return NewCLient;
    }
    private GameObject SpawnNewCase(int CountryNumber)
    {
        //Снача чистим старые списки и удаляем предыдущие предметы
        foreach(GameObject a in GameMain.Items)
            Destroy(a);
        GameMain.Items.Clear();
        foreach (GameObject a in GameMain.Places)
            Destroy(a);
        GameMain.Places.Clear();

        int CaseNumber = Random.Range(0, ItemsBase.Elements[CountryNumber].Cases.Count);
        GameObject NewCase = Instantiate(ItemsBase.Elements[CountryNumber].Cases[CaseNumber], CasePoint.position, CasePoint.rotation);
        return NewCase;
    }

    void Start()
    {
        AllGUI = FindObjectOfType<AllGUI>();
        GameMain = FindObjectOfType<GameMain>();
        ItemsBase = FindObjectOfType<ItemsBase>();
        ClientPoint = GameObject.Find("ClientPoint").transform;
        CasePoint = GameObject.Find("CasePoint").transform;

        //создаём трёх первых клиентов и просим их двигаться
        for (int i = 0; i < 3; i++)
            SpawnNewCLient().GetComponent<Animation>().Play("Come " + (2-i));
        StartCoroutine(Prestart());
    }

    private IEnumerator Prestart()
    {
        //Создаём первый кейс и двигаем его
        yield return new WaitForSeconds(1);
        CurrentClient = Clients[0];
        GameMain.Case = SpawnNewCase(CurrentClient.GetComponent<Client>().ClientType);
        AllGUI.ClientArrived(CurrentClient);
        //Включаем кнопки выбора
        yield return new WaitForSeconds(1);
        PackingAgreeButton.SetActive(true);
        PackingDisagreeButton.SetActive(true);
    }

    //Игрок согласился на паковку
    public void Packing()
    {
        AllGUI.ClientGone();
        GameMain.Packing = true;
        GameMain.CaseOpen = true;
        PackingAgreeButton.SetActive(false);
        PackingDisagreeButton.SetActive(false);
        GameMain.CloseCaseButton.SetActive(true);
        GameMain.Case.GetComponent<Case>().spawned = true;
    }

    //Игрок отказался
    public void Disagree()
    {
        AllGUI.ClientGone();
        PackingAgreeButton.SetActive(false);
        PackingDisagreeButton.SetActive(false);
        StartCoroutine(Restart(true));
    }

    public IEnumerator Restart(bool Disagree)
    {
        //если отказ
        if (Disagree)
        {
            CurrentClient.GetComponent<Animation>().Play("Sad");
            GameMain.Case.GetComponent<Animation>().Play("CaseOff");
            Clients.Remove(CurrentClient);
        }
        //Если выполнил паковку успешно
        else 
        {
            AllGUI.Money(CurrentClient.GetComponent<Client>().ClientMoney);
            CurrentClient.GetComponent<Animation>().Play("Happy");
            GameMain.Case.GetComponent<Animation>().Play("CaseDone");
            Clients.Remove(CurrentClient);
        }
        yield return new WaitForSeconds(2);

        //создаём нового и убиваем старого клиента
        SpawnNewCLient();
        Destroy(CurrentClient);
        CurrentClient = Clients[0];
        CurrentCountry = CurrentClient.GetComponent<Client>().ClientType;
        //Все клиенты двигаются
        for (int i=0;i<3;i++)
            Clients[i].GetComponent<Animation>().Play("Come "+(2-i));
        //Удаляем старый кейс и создаём новый
        Destroy(GameMain.Case);
        GameMain.Case = SpawnNewCase(CurrentCountry);

        AllGUI.ClientArrived(CurrentClient);

        yield return new WaitForSeconds(0.5f);

        PackingAgreeButton.SetActive(true);
        PackingDisagreeButton.SetActive(true);
    }
}
