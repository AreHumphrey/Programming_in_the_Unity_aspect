using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Move : MonoBehaviour
{

    // SerializedField - поле которое будет видимо из Unity Editor, лучше чем public, потому что другие элементы не смогут изменить значение
    [SerializeField] private InputActionAsset InputActions;

    // InputAction - ссылка на действие из InputSystem
    private InputAction moveAction;
    private InputAction jumpAction;

    private Vector2 moveDir;

    private Rigidbody rb;
    [Header("Movement speeds")]
    [Min(0)]
    [SerializeField] float moveSpeed = 5;
    [SerializeField] float jumpSpeed = 5;

    [Header("Jump Trigger")]
    [Range(0.01f, 1)]
    [SerializeField] float boxHeight = 1;
    private BoxCollider bottomTrigger;
    [SerializeField] private int BottomCollisions = 0;

    [Header("Rotation")]
    [Range(0.1f, 10f)]
    [SerializeField] private float rotationSpeed = 10f;
    private float currentAngle = 0f;
    private float newAngle = 0f;

    // Вызывается когда объект включается на сцене, рядом с именем объекта в UnityEditor есть галочка
    private void OnEnable()
    {
        // Установка карты действий
        InputActions.FindActionMap("Player").Enable();
    }

    // Вызывается когда объект выключается на сцене
    void OnDisable()
    {
        InputActions.FindActionMap("Player").Disable();
    }

    // Вызывается когда объект загружается на сцену, даже если он выключен
    void Awake()
    {
        // Определение действий
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");

        rb = GetComponent<Rigidbody>();

        // Постановка нижнего тригера к необходимому размеру
        bottomTrigger = GetComponent<BoxCollider>();
        bottomTrigger.center = -(0.95f + boxHeight / 2) * Vector3.up;
        bottomTrigger.size = new Vector3(0.8f, boxHeight, 0.8f);
    }

    // Вызывается когда перед первым Update для включенного объекта
    void Start()
    {

    }

    // Вызывается один раз за кадр
    // Все что влияет на камеру лучше делать здесь
    void Update()
    {
        moveDir = moveAction.ReadValue<Vector2>().normalized;

        if (jumpAction.WasPressedThisFrame() && BottomCollisions > 0)
        {
            Jump();
        }
    }
    // Вызывается через константный промежуток времени который указан в UnityEditor (0.02 секунды)
    // Все действия связанные с физикой лучше делать здесь
    void FixedUpdate()
    {
        Moveing();
        Rotate();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);

        Gizmos.color = Color.green;
        Gizmos.DrawCube(transform.position - ((0.95f + boxHeight / 2) * Vector3.up), new Vector3(0.8f, boxHeight, 0.8f));
    }

    void OnTriggerEnter(Collider other)
    {
        BottomCollisions += 1;
    }

    void OnTriggerExit(Collider other)
    {
        BottomCollisions -= 1;
    }

    // Ниже самописные функции

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
    }

    void Moveing()
    {
        rb.MovePosition(rb.position + new Vector3(moveDir.x, 0, moveDir.y).normalized * moveSpeed * Time.fixedDeltaTime);
    }

    void Rotate()
    {
        if (moveDir.magnitude > 0.1f)
        {
            newAngle = Mathf.Atan2(moveDir.x, moveDir.y) * Mathf.Rad2Deg;
            newAngle = Mathf.LerpAngle(transform.eulerAngles.y, newAngle, rotationSpeed * Time.fixedDeltaTime);
            transform.rotation = Quaternion.Euler(0, newAngle, 0);
        }
    }
}