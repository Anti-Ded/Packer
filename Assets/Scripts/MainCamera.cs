using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    GameMain GameMain;

    public Vector3 CameraPosPacking = new Vector3(-0.5f, 2.5f, 0);
    public Vector3 CameraEulerPacking = new Vector3(90, 0, -90);
    public Vector3 CameraPosNoPacking = new Vector3(0, 1.6f, -2);
    public Vector3 CameraEulerNoPacking = new Vector3(10, 0, 0);

    private void Start()
    {
        GameMain = FindObjectOfType<GameMain>();
    }
    void Update()
    {
        Transform CameraTransform = gameObject.GetComponent<Transform>();
        if (GameMain.Packing)
        {
            CameraTransform.position = Vector3.Lerp(CameraTransform.position, CameraPosPacking, 3 * Time.deltaTime);
            CameraTransform.rotation = Quaternion.Lerp(CameraTransform.rotation, Quaternion.Euler(CameraEulerPacking), 3 * Time.deltaTime);
        }
        else
        {
            CameraTransform.position = Vector3.Lerp(CameraTransform.position, CameraPosNoPacking, 3 * Time.deltaTime);
            CameraTransform.rotation = Quaternion.Lerp(CameraTransform.rotation, Quaternion.Euler(CameraEulerNoPacking), 3 * Time.deltaTime);
        }
    }
}
