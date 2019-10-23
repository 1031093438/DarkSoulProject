using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public KeyboardInput pi;

    private GameObject playerHandle;
    private GameObject cameraHandle;
    private GameObject model;
    private Camera mainCamera;
    private float tempEulerX;

    public float horizontal = 100.0f;
    public float vertical = 100.0f;
    // Start is called before the first frame update
    void Awake()
    {
        cameraHandle = transform.parent.gameObject;
        playerHandle = cameraHandle.transform.parent.gameObject;
        tempEulerX = 20;
        model = playerHandle.GetComponent<ActorController>().model;
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 tempModelEuler = model.transform.eulerAngles;

        playerHandle.transform.Rotate(Vector3.up,pi.Jright * horizontal * Time.fixedDeltaTime);
        tempEulerX -= pi.Jup * vertical * Time.fixedDeltaTime;
        cameraHandle.transform.localEulerAngles = new Vector3(Mathf.Clamp(tempEulerX, -40, 30), 0, 0);
        model.transform.eulerAngles = tempModelEuler;

        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, transform.position, 0.3f);
        //mainCamera.transform.eulerAngles = transform.eulerAngles;
        mainCamera.transform.LookAt(cameraHandle.transform);
    }
}
