using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public void DestroyIt()
    {
        StartCoroutine(DestroyWait());
    }

    private IEnumerator DestroyWait()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
