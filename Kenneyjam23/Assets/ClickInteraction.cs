using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClickInteraction : MonoBehaviour
{
    public bool OnClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) == false) return false;

        Chest c = hit.transform.gameObject.GetComponent<Chest>();

        if (c == null) return false;

        return c.Interact(GetComponent<Inventory>());
    }
}
