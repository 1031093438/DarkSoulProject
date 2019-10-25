using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public KeyboardInput pi;
    public Image lockDot;

    private GameObject playerHandle;
    private GameObject cameraHandle;
    private GameObject model;
    private Camera mainCamera;
    private float tempEulerX;
    [SerializeField]
    private GameObject lockTarget;

    public float horizontal = 100.0f;
    public float vertical = 100.0f;

    public bool lockState;
    // Start is called before the first frame update
    void Awake()
    {
        cameraHandle = transform.parent.gameObject;
        playerHandle = cameraHandle.transform.parent.gameObject;
        tempEulerX = 20;
        model = playerHandle.GetComponent<ActorController>().model;
        mainCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        lockDot.enabled = false;
        lockState = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(lockTarget == null)
        {
            Vector3 tempModelEuler = model.transform.eulerAngles;

            playerHandle.transform.Rotate(Vector3.up,pi.Jright * horizontal * Time.fixedDeltaTime);
            tempEulerX -= pi.Jup * vertical * Time.fixedDeltaTime;
            cameraHandle.transform.localEulerAngles = new Vector3(Mathf.Clamp(tempEulerX, -40, 30), 0, 0);
            model.transform.eulerAngles = tempModelEuler;
        }
        else
        {
            Vector3 tempForward = lockTarget.transform.position - model.transform.position;
            tempForward.y = 0;
            playerHandle.transform.forward = tempForward;
        }


        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, transform.position, 0.3f);
        //mainCamera.transform.eulerAngles = transform.eulerAngles;
        mainCamera.transform.LookAt(cameraHandle.transform);
    }

    public void LockUnlock()
    {

        //try to lock
        Vector3 modelOrigin1 = model.transform.position;
        Vector3 modelOrigin2 = modelOrigin1 + new Vector3(0, 1.0f, 0);
        Vector3 boxCenter = modelOrigin2 + model.transform.forward * 5.0f;

        Collider[] cols = Physics.OverlapBox(boxCenter, new Vector3(0.5f, 0.5f, 5.0f), model.transform.rotation, LayerMask.GetMask("Enemy"));
        if(cols.Length == 0)
        {
            lockTarget = null;
            lockDot.enabled = false;
            lockState = false;
        }
        else
        {
            foreach (var col in cols)
            {
                if(lockTarget == col.gameObject)
                {
                    lockTarget = null;
                    lockDot.enabled = false;
                    lockState = false;
                    break;
                }
                lockTarget = col.gameObject;
                lockDot.enabled = true;
                lockState = true;
            }
        }

    }
}
