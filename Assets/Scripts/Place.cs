using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place : MonoBehaviour
{
    GameMain GameMain;
    public Renderer Renderer;

    public GameObject Item;
    void Start()
    {
        GameMain = FindObjectOfType<GameMain>();
        GameMain.Places.Add(gameObject);
        Hide();
    }

    public void Hide()
    {
        Renderer.enabled = false;
    }

    public void Show()
    {
        if(!Item || Item.transform == GameMain.ItemInHand)
           Renderer.enabled = true;
    }
    private void OnTriggerExit(Collider other)
    {

        if (Item && Item.GetComponent<Item>().Places.Contains(gameObject))
            other.gameObject.GetComponent<Item>().Places.Remove(gameObject);
        if (Item && other.gameObject == Item)
            Item = null;
        if (GameMain.ItemInHand == other.gameObject.transform || other.GetComponent<Item>().moving)
        {
            if (other.gameObject.GetComponent<Item>().Places.Count <= other.gameObject.GetComponent<Item>().PlacesCountNeeded)
                other.gameObject.GetComponent<Item>().Place = null;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
            if (!GameMain.Case.GetComponent<Case>().spawned)
            {
                if (!other.GetComponent<Item>().Places.Contains(gameObject))
                    other.GetComponent<Item>().Places.Add(gameObject);
                Item = other.gameObject;
            }
        else
            {
                if (!Item && !other.GetComponent<Item>().Places.Contains(gameObject) && 
                    (GameMain.ItemInHand == other.gameObject.transform || other.GetComponent<Item>().moving))
                {
                    other.GetComponent<Item>().Places.Add(gameObject);
                    Item = other.gameObject;
                }
            }
    }
 }
