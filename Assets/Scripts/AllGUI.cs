﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllGUI : MonoBehaviour
{
    public int CurrentMoney;
    public GameObject ClientFrame;
    public TMPro.TextMeshProUGUI MoneyValue;
    public TMPro.TextMeshProUGUI ClientMoneyValue;
    public Image ClientCountry;
    GameMain GameMain;
    ItemsBase ItemsBase;

    private void Start()
    {
        ItemsBase = FindObjectOfType<ItemsBase>();
        GameMain = FindObjectOfType<GameMain>();
        CurrentMoney = PlayerPrefs.GetInt("CurrentMoney", 0);
        Money();
    }
    public void ClientArrived(GameObject CurrentClient)
    {
        Client Client = CurrentClient.GetComponent<Client>();
        ClientFrame.SetActive(true);
        Client.ClientMoney = GameMain.Case.GetComponent<Case>().CaseType * Random.Range(20, 30);
        ClientMoneyValue.text = Client.ClientMoney.ToString();
        ClientCountry.sprite = ItemsBase.Elements[Client.ClientType].CountryFlag;
    }

    public void ClientGone()
    {
        ClientFrame.SetActive(false);
    }
    public void Money(int AddMoney = 0)
    {
        CurrentMoney += AddMoney;
        MoneyValue.text = CurrentMoney.ToString();
        PlayerPrefs.SetInt("CurrentMoney", CurrentMoney);
    }
}
