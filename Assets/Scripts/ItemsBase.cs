using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class Country : System.Object
{
    public string CountryName;
    public Sprite CountryFlag;
    public List<GameObject> Items = new List<GameObject>();
    public List<GameObject> Cases = new List<GameObject>();
    public List<GameObject> Clients = new List<GameObject>();
}

public class ItemsBase : MonoBehaviour
{
    public List<Country> Elements = new List<Country>();
}
