using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    List<Controller> _controllers;

    void Awake()
    {
        _controllers = FindObjectsOfType<Controller>().ToList();

        int index = 1;
        foreach (var controller in _controllers)
        {
            controller.SetIndex(index);
            index++;
        }
    }

    void Update()
    {
        foreach (var controller in _controllers)
        {
            if (controller.IsAssigned == false && controller.AnyButtonDown())
            {
                AssignController(controller);
            }
        }
    }

    void AssignController(Controller controller)
    {
        controller.IsAssigned = true;
        Debug.Log("Assigned Controller " + controller.gameObject.name);
        FindObjectOfType<PlayerManager>().AddPlayerToGame(controller);
    }
}
