using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using MyLib;

public class Item : MonoBehaviour
{
    GameMain GameMain;
    RaycastHit hit;
    Ray ray;
    public int PlacesCountNeeded =1;
    public bool Unmovable = false;
    public bool moving = true;


    public Vector3 Pos;

    public GameObject Place;
    public List<GameObject> Places = new List<GameObject>();
    public List<Collider> Colliders = new List<Collider>();
    List<Collider> CaseColliders = new List<Collider>();

    void Start()
    {
        GameMain = FindObjectOfType<GameMain>();
        GameMain.Items.Add(gameObject);
        Pos = transform.position;
    }

    void Update()
    {
        //Если ячеек нужное количество, то присваиваем предмету место
        if (Places.Count == PlacesCountNeeded)
            Place = Places[My.Closest(gameObject, Places)];

        //Двигаем предмет по курсору
        if (GameMain.ItemInHand == gameObject.transform)
        {
            if (GameMain.Mouse)
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            else ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            if (Physics.Raycast(ray, out hit, 10,LayerMask.GetMask("Table")))
                transform.position = new Vector3(hit.point.x, 0.67f, hit.point.z);
        }
        else //Если предмет не схвачен, то его либо в ячейку двигать, либо на место
        {
            if (Place)
            {
                transform.SetParent(Place.transform, true);
                transform.position = Vector3.Lerp(transform.position, Place.transform.position, 5 * Time.deltaTime);

                if (Vector3.Distance(transform.position, Place.transform.position) > 0.01f)
                    moving = true;
                else moving = false;
            }
            else
            {
                transform.SetParent(null);
                transform.position = Vector3.Lerp(transform.position, Pos, 3 * Time.deltaTime);

                if (Vector3.Distance(transform.position, Pos) > 0.01f)
                    moving = true;
                else moving = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item" && !Colliders.Contains(other))
            Colliders.Add(other);
        if (other.tag == "Case" && !CaseColliders.Contains(other))
            CaseColliders.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Item")
            Colliders.Remove(other);
        if (other.tag == "Case")
            CaseColliders.Remove(other);
    }
}
