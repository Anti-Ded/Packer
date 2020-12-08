using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyLib;

public class GameMain : MonoBehaviour
{
    public bool Packing = false;
    public bool CaseOpen = false;
    public bool Mouse =true;
    
    public Transform ItemInHand;
    public GameObject Case;
    public GameObject CloseCaseButton;

    public List<GameObject> Places = new List<GameObject>();
    public List<GameObject> Items = new List<GameObject>();

    GameClients GameClients;
    private Touch touch;

    public GameObject ErrorPrefab;
    public float ScreenCoeff;

    private void Start()
    {
        GameClients = FindObjectOfType<GameClients>();
        ScreenCoeff = 450 / (float)(Screen.width);
    }

    void Update()
    {
            //Берём в руки предмет
        if (Packing && CaseOpen && (Input.GetMouseButtonDown(0) || Input.touchCount>0))
        {
            RaycastHit hit;
            Ray ray;
            if (Input.touchCount > 0)
                Mouse = false;

            if (Mouse)
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            else ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            
            if (Physics.Raycast(ray, out hit) 
                && hit.transform.tag =="Item" 
                && !hit.transform.gameObject.GetComponent<Item>().Unmovable)
            {
                ItemInHand = hit.transform;

                foreach (GameObject a in Places)
                    a.GetComponent<Place>().Show();
            }
        }
        
        //Включаем показ мест для предметов
        if (Packing && CaseOpen && Input.GetMouseButtonUp(0) && ItemInHand)
        {
            ItemInHand = null;
            foreach (GameObject a in Places)
                a.GetComponent<Place>().Hide();
        }
    }

    public void CloseCase()
    {
        StartCoroutine(TestCase());
    }
    private IEnumerator TestCase()
    {
        CaseOpen = false;
        yield return new WaitForSeconds(1);
        bool Good = true;
        foreach (GameObject a in Items)
            if (!a)
                Items.Remove(a);

        foreach (GameObject a in Items)
        {
            //достаточно ли у каждого предмета ячеек
            if (!(a.GetComponent<Item>().Places.Count >= a.GetComponent<Item>().PlacesCountNeeded) && !a.GetComponent<Item>().Unmovable)
            {
                Good = false;
                if(a.GetComponent<Item>().Place)
                    Instantiate(ErrorPrefab, a.transform.position, a.transform.rotation, a.GetComponent<Item>().Place.transform);
                else Instantiate(ErrorPrefab, a.transform.position, a.transform.rotation);
            }
            //удаляем пустые коллайдеры (подстраховка)
            foreach (Collider b in a.GetComponent<Item>().Colliders)
                if (!b)
                    a.GetComponent<Item>().Colliders.Remove(b);
            //проверяем не пересекается ли с кем предмет во время закрытия
            if (a.GetComponent<Item>().Colliders.Count != 0)
            {
                Good = false;
                Instantiate(ErrorPrefab, a.transform.position, a.transform.rotation, a.GetComponent<Item>().Place.transform);
            }
        }
        if (!Good)
        {
            CaseOpen = true;
            CloseCaseButton.SetActive(true);
        }
        else
        {
            Packing = false;
            StartCoroutine(GameClients.Restart(false));
        }
    }
}
