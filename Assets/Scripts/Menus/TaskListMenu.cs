using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskListMenu : MonoBehaviour
{
    public GameObject createTaskMenu = null;

    public void OnCreateNewTaskButton()
    {
        createTaskMenu.gameObject.SetActive(true);
    }

    public void OnCreateTaskCancelButton()
    {
        createTaskMenu.gameObject.SetActive(false);
    }
}
