using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    //Скорость
    public float speed = 5f;

    //Двигается ли игрок?
    bool isMoving;
    //Его текущая позиция
    Vector3 currPosition;
    //Аниматор камеры
    Animator cameraAnimator;

    //Кэш переменная Transform
    Transform _RBTransform;
    // Инициализация
    void Start () {
        _RBTransform = GetComponent<Rigidbody>().transform;
        currPosition = _RBTransform.position;
        cameraAnimator = GetComponentInChildren<Camera>().GetComponent<Animator>();
        isMoving = false;
    }
	
	void Update () {
        //Считывания нажатой клавишы,перемещение игрока и анимация ходьбы
        isMoving = false;
        if (Input.GetKey("w"))
        {
            isMoving = true;
            currPosition.z += speed * Time.deltaTime;
        }
        if (Input.GetKey("a"))
        {
            isMoving = true;
            currPosition.x -= speed/2 * Time.deltaTime;
        }
        if (Input.GetKey("s"))
        {
            isMoving = true;
            currPosition.z -= speed * Time.deltaTime;
        }
        if (Input.GetKey("d"))
        {
            isMoving = true;
            currPosition.x += speed/2 * Time.deltaTime;
        }
        cameraAnimator.SetBool("isMoving", isMoving);
        _RBTransform.position = currPosition;
    }
}
