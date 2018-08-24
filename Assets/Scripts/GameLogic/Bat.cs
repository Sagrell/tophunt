using System.Collections;
using UnityEngine;

// Класс мыши (т.к. проект маленький и дальнейшее добавление мышей и расширение не подразумевается, 
// вся логика полета мыши проходит в этом классе)
public class Bat : MonoBehaviour {
    [Header("Скорость мыши:")]
    public float speed;
    [Header("Настройки полета:")]
    //Скорость поворота
    public float rotationSpeed;
    [Header("Префаб сферы:")]
    public GameObject sphere;

    //Текущая сфера
    GameObject currentSphere;
    //Transform игрока
    Transform playerT;

    //Позиция сферы
    Vector3 spherePosition;
    //Направление мыши
    Vector3 currDirection;
    //Кэш переменная
    Transform _transform;

    //Инициализация
    void Start () {
        _transform = transform;
        playerT = FindObjectOfType<PlayerController>().transform;
        currDirection = Vector3.zero;

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
        //yield return new WaitForSeconds(Random.Range(1f,10f));
        //Направляет мышь изначально в сторону игрока
        currDirection = (playerT.position - _transform.position).normalized;
        //Летит к месту, где находится игрок в данный момент
        yield return FlyTo(playerT.position);
        //После чего летит к своей сфере
        yield return FlyTo(spherePosition);
    }

    //Лететь к заданным координатам
    IEnumerator FlyTo(Vector3 target)
    {
        //Нужный вектор направления
        Vector3 targetDirection = (target - _transform.position).normalized;

        //Полет, пока дистанция до цели не будет минимальной
        while (Vector3.Distance(_transform.position,target)>0.1f)
        {
            //Вращение мыши на вектор направления
            _transform.rotation = Quaternion.LookRotation(currDirection);
            //Поворот вектора направления в сторону нужного
            currDirection = Vector3.RotateTowards(currDirection, targetDirection, rotationSpeed*Time.deltaTime, 0.0f);
            //Двигает мышь по вектору направления со скоростью speed
            _transform.position += currDirection * (speed * Time.deltaTime);
            //Перерасчет вектора направления
            targetDirection = (target - _transform.position).normalized;
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
