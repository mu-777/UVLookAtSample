using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerWASD : MonoBehaviour {

    public KeyCode ControlActivateKey = KeyCode.LeftShift;

    const float MOUSE_SCALE_X = 2.0f;
    const float MOUSE_SCALE_Y = -2.0f;

    private Vector3 _initPosition;
    private Quaternion _initRotation;

    // Use this for initialization
    void Start() {
        _initPosition = Camera.main.transform.position;
        _initRotation = Camera.main.transform.rotation;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKey(ControlActivateKey)) {
            var EulerRotate = Camera.main.transform.rotation.eulerAngles;
            float deltaX = Input.GetAxis("Mouse X");
            float deltaY = Input.GetAxis("Mouse Y");
            EulerRotate.x += deltaY * MOUSE_SCALE_Y;
            EulerRotate.y += deltaX * MOUSE_SCALE_X;
            Camera.main.transform.rotation = Quaternion.Euler(EulerRotate);

            var deltaTranslation = Vector3.zero;
            var translateScale = 0.1f;
            deltaTranslation.x += Input.GetKey(KeyCode.D) ? translateScale : Input.GetKey(KeyCode.A) ? -translateScale : 0;
            deltaTranslation.y += Input.GetKey(KeyCode.E) ? translateScale : Input.GetKey(KeyCode.C) ? -translateScale : 0;
            deltaTranslation.z += Input.GetKey(KeyCode.W) ? translateScale : Input.GetKey(KeyCode.S) ? -translateScale : 0;
            Camera.main.transform.Translate(deltaTranslation, Space.Self);
        } else if (Input.GetKeyDown(KeyCode.Space)) {
            Camera.main.transform.position = _initPosition;
            Camera.main.transform.rotation = _initRotation;
        }
    }
}
