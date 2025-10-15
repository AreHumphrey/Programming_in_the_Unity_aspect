using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class RLLaunch : MonoBehaviour
{
    [SerializeField] private InputActionAsset InputActions;
    [SerializeField] private float coolDown = 2f;
    [SerializeField] private Transform launchPoint;
    [SerializeField] private GameObject rocket;
    private bool canShoot = true;
    private InputAction launchAction;
    private void OnEnable()
    {
        // Установка карты действий
        InputActions.FindActionMap("RocketLauncher").Enable();
    }

    // Вызывается когда объект выключается на сцене
    void OnDisable()
    {
        InputActions.FindActionMap("RocketLauncher").Disable();
    }

    void Awake()
    {
        launchAction = InputSystem.actions.FindAction("Launch");
    }

    IEnumerator Launch()
    {
        canShoot = false;
        Debug.Log("Shot");
        Instantiate(rocket, launchPoint);
        yield return new WaitForSeconds(coolDown);
        canShoot = true;
    }
    void Update()
    {
        if (launchAction.IsPressed() && canShoot)
        {
            StartCoroutine(Launch());
        }
    }
}
