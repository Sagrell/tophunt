using System.Collections;
using UnityEngine;

// Класс мыши (т.к. проект маленький и дальнейшее добавление мышей и расширение не подразумевается, 
// вся логика полета мыши проходит в этом классе)
public class Bat : MonoBehaviour {
    [Header("Скорость мыши:")]
    public float speed;
    [Header("Настройки полета:")]
    //Частота коллебаний 
    public float frequency;
    //Амблитуда колебаний
    public float magnitude;
    [Header("Префаб сферы:")]
    public GameObject sphere;

    //Текущая сфера
    GameObject currentSphere;
    //Transform игрока
    Transform playerT;

    //Позиция сферы
    Vector3 spherePosition;

    //Кэш переменная
    Transform _transform;

    //Инициализация
    void Start () {
        _transform = transform;
        playerT = FindObjectOfType<PlayerController>().transform;

        //Задается случайная позиция
        _transform.position = playerT.position + PointGenerator.GenerateRandomPointOnAnulus();

        //Генерируется сфера
        GenerateSphere();
        //Запускается мышь
        StartCoroutine(Run());
    }
    //Запустить цикл полета мыши
    IEnumerator Run()
    {
        //Ждет от 1 до 10 сек
        yield return new WaitForSeconds(Random.Range(1f,10f));
        //Летит к месту, где находится игрок в данный момент
        yield return FlyTo(playerT.position);
        //После чего летит к своей сфере
        yield return FlyTo(spherePosition);
    }

    //Лететь к заданным координатам
    IEnumerator FlyTo(Vector3 target)
    {
        //Поворот в сторону цели
        _transform.rotation = Quaternion.LookRotation(target - _transform.position);
        //Полет, пока дистанция до цели не будет минимальной
        while (Vector3.Distance(_transform.position,target)>0.1f)
        {
            //Линейно перемещение мыши в сторону цели
            _transform.position = Vector3.MoveTowards(_transform.position, target, speed*Time.deltaTime);
            //Синусоидное колебание (эффект полета мыши)
            _transform.position += _transform.up * Mathf.Sin(Time.time * frequency) * magnitude;
            yield return null;
        }  
    }

    //Атака по игроку
    public void HitPlayer()
    {
        //Остановить все подпроцессы и лететь к сфере
        StopAllCoroutines();
        StartCoroutine(FlyTo(spherePosition));
    }

    //Генерация сферы
    void GenerateSphere()
    {
        //Уничтожает текущую
        Destroy(currentSphere);
        //Задает новую позицию
        spherePosition = playerT.position + PointGenerator.GenerateRandomPointOnAnulus();
        //Задает текущую сферу
        currentSphere = Instantiate(sphere, spherePosition, Quaternion.identity);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Если мышь прилетела к сфере, то создать новую и запустить все по-новой
        if(other.CompareTag("Sphere"))
        {
            GenerateSphere();
            StopAllCoroutines();
            StartCoroutine(Run());
        }
        //Если мышь коснулась игрока, вызывается метод у мыши и активируется ивент
        if (other.CompareTag("Player"))
        {
            HitPlayer();
            EventManager.TriggerEvent("HitPlayer");
        }
    }
}
