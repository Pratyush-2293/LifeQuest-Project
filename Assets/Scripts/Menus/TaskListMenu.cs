using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskListMenu : MonoBehaviour
{
    public GameObject createTaskMenu = null;
    public TaskItem taskItemPrefab = null;
    public TaskItem taskItem;
    public Transform taskListTransform = null;

    // Task Creation Menu
    public InputField titleInput = null;
    public InputField descriptionInput = null;

    public void OnCreateNewTaskButton()
    {
        createTaskMenu.gameObject.SetActive(true);
    }

    public void OnCreateTaskButton()
    {
        // Instantiating and Adding to layout
        taskItem = Instantiate(taskItemPrefab);
        taskItem.transform.SetParent(taskListTransform);

        // Filling in values
        taskItem.taskTitle = titleInput.text;
        taskItem.taskDescription = descriptionInput.text;

        // Resetting values of Task Creation Menu
        titleInput.text = "";
        descriptionInput.text = "";

        createTaskMenu.gameObject.SetActive(false);
    }

    public void OnCreateTaskCancelButton()
    {
        createTaskMenu.gameObject.SetActive(false);
    }
}
