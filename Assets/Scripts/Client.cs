using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client : MonoBehaviour
{
    public int ClientType;
    public int ClientMoney;

    private void Start()
    {
        ClientMoney = Random.Range(20, 60);
    }
}
