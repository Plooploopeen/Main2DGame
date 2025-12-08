using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryScript : MonoBehaviour
{
    [SerializeField] InputActionAsset inputActions;
    [SerializeField] GameObject inventory;

    private InputAction menuAction;

    private void Awake()
    {
        menuAction = InputSystem.actions.FindAction("Menu");
    }

    void Start()
    {
        inventory.SetActive(false);
    }

    void Update()
    {
        if (menuAction.WasPressedThisFrame())
        {
            inventory.SetActive(!inventory.activeSelf);

            if (inventory.activeSelf) {Time.timeScale = 0f;}
            else if (!inventory.activeSelf) { Time.timeScale = 1f;}
        }
    }
}
