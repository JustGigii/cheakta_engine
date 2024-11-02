using cakeslice;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraControll : MonoBehaviour
{

    [SerializeField][Range(0.0f, 0.5f)] float smoothTime = 0.03f;
    [SerializeField][Range(0.0f, 0.5f)] float scorllsmoothTime = 0.01f;
    [SerializeField][Range(0.0f, 10f)] float mouseSensitivity = 3.5f;
    [SerializeField][Range(0.0f, 10f)] float ScrollSensitivity = 3.5f;

    public bool lockCursor;
    public RTLTMPro.RTLTextMeshPro place;

    private Vector2 currentMouseDeltaVelocity = Vector2.zero;
    private Vector2 currentMouseDelta;
    private Vector2 cameraPitch;
    private float zoom;
    private float cameraVelocity = 0f;
    private Camera camera;
    private GameObject allTransforms;
    private int transformsInsex;
    //public bool IsMove;


    // Start is called before the first frame update
    void Start()
    {
        ValuePasser.IsMove = true;
        this.camera = GetComponent<Camera>();
        this.cameraPitch = new Vector2(this.transform.rotation.x, this.transform.rotation.y);
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        allTransforms = GameObject.Find("Transforms");
        transformsInsex = 0;
        place.text = allTransforms.transform.GetChild(transformsInsex).gameObject.name;
        camera.transform.position = allTransforms.transform.GetChild(transformsInsex).transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ValuePasser.IsMove = !ValuePasser.IsMove;
            Cursor.lockState =(!ValuePasser.IsMove)? CursorLockMode.Confined: CursorLockMode.Locked;
            Cursor.visible = !ValuePasser.IsMove;
        }

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            transformsInsex = (transformsInsex + 1 == allTransforms.transform.childCount) ? 0 : transformsInsex + 1;
            camera.transform.position = allTransforms.transform.GetChild(transformsInsex).position;
            place.text = allTransforms.transform.GetChild(transformsInsex).gameObject.name;
        }
        if (ValuePasser.IsMove ==true) CemeraMovement();
        Zoom();

    }

    private void Zoom()
    {
        zoom = camera.fieldOfView - (Input.GetAxis("Mouse ScrollWheel") * ScrollSensitivity * 10);
        zoom = Mathf.Clamp(zoom, 15f, 60f);
        camera.fieldOfView = Mathf.SmoothDamp(camera.fieldOfView, zoom, ref cameraVelocity, scorllsmoothTime);
    }

    private void CemeraMovement()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, smoothTime);

        cameraPitch.y -= currentMouseDelta.y * mouseSensitivity;
        cameraPitch.y = Mathf.Clamp(cameraPitch.y, -90.0f, 90.0f);
        cameraPitch.x += currentMouseDelta.x * mouseSensitivity;
        cameraPitch.x = Mathf.Clamp(cameraPitch.x, -80.0f, 220.0f);

        this.transform.localEulerAngles = new Vector3(cameraPitch.y, cameraPitch.x, 0f);
    }

    
}
