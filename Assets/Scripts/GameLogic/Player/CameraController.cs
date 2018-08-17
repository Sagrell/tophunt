using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [Header("Скорость поворота камеры:")]
    public float speed = 5f;

    //Текущий поворот
    Vector3 currRotation;

    //Кэш
    Transform _transform;

	void Start () {
        _transform = transform;
        currRotation = _transform.localRotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        //Считывания нажатой клавишы и поворот камеры
        if (Input.GetKey("up"))
        {
            currRotation.x = Mathf.Clamp(currRotation.x - speed * Time.deltaTime, -45f, 18f);
        }
        if (Input.GetKey("left"))
        {
            currRotation.y -= speed * Time.deltaTime;
        }
        if (Input.GetKey("down"))
        {
            currRotation.x = Mathf.Clamp(currRotation.x + speed * Time.deltaTime, -45f, 18f);
        }
        if (Input.GetKey("right"))
        {
            currRotation.y += speed * Time.deltaTime;
        }
        _transform.localRotation = Quaternion.Euler(currRotation); 
    }
}
