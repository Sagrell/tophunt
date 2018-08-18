using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Класс отвечающий за генерацию координат
public static class PointGenerator {
    //Константы
    const float MIN_DISTANCE = 20f;
    const float MAX_DISTANCE = 100f;
    const float MIN_HEIGHT = 15f;
    const float MAX_HEIGHT = 35f;


    public static Vector3 GenerateRandomPointOnAnulus()
    {
        //Генерируется дистанция и высота
        float distance = Random.Range(MIN_DISTANCE, MAX_DISTANCE);
        float height = Random.Range(MIN_HEIGHT, MAX_HEIGHT);
        //Генерируется случайный угол
        float angle = Random.Range(0f, 360f);
        //Генерируются необходимые координаты
        return new Vector3(Mathf.Cos(angle) * distance, height, Mathf.Sin(angle) * distance);
    }
}
