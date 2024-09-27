using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskListMenu : Menu
{
    public static TaskListMenu instance = null;

    public GameObject createTaskMenu = null;
    public TaskItem taskItemPrefab = null;
    public TaskItem taskItem;
    public Transform taskListTransform = null;

    // Task Creation Menu
    public InputField titleInput = null;
    public InputField descriptionInput = null;
    public Toggle easyToggle = null;
    public Toggle mediumToggle = null;
    public Toggle hardToggle = null;

    // Save Data
    public PlayerTasksData playerTasksData = new PlayerTasksData();
    private bool tasksLoaded = false;

    private void Awake()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one TaskListMenu!");
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        //LoadPlayerTasks();
    }

    public void LoadPlayerTasks()
    {
        if (!tasksLoaded)
        {
            SaveManager.instance.LoadPlayerTasksData();

            for (int i = 0; i < playerTasksData.playerTaskItemDatas.Count; i++)
            {
                taskItem = Instantiate(taskItemPrefab);
                taskItem.transform.SetParent(taskListTransform);

                taskItem.taskTitle = playerTasksData.playerTaskItemDatas[i].taskTitle;
                taskItem.taskDescription = playerTasksData.playerTaskItemDatas[i].taskDescription;
                taskItem.taskDifficulty = playerTasksData.playerTaskItemDatas[i].difficulty;
            }

            tasksLoaded = true;
        }
    }

    public void RemoveTaskItemData(string taskTitle)
    {
        for(int i = 0; i < playerTasksData.playerTaskItemDatas.Count; i++)
        {
            if(playerTasksData.playerTaskItemDatas[i].taskTitle == taskTitle)
            {
                playerTasksData.playerTaskItemDatas.RemoveAt(i);
            }
        }
    }

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

        // Setting Difficulty Counters
        if (easyToggle.isOn)
        {
            taskItem.taskDifficulty = TaskItem.Difficulty.easy;
        }
        else if (mediumToggle.isOn)
        {
            taskItem.taskDifficulty = TaskItem.Difficulty.medium;
        }
        else
        {
            taskItem.taskDifficulty = TaskItem.Difficulty.hard;
        }

        // Saving Task Item Data
        PlayerTaskItemData taskItemData = new PlayerTaskItemData();
        taskItemData.taskTitle = taskItem.taskTitle;
        taskItemData.taskDescription = taskItem.taskDescription;
        taskItemData.difficulty = taskItem.taskDifficulty;
        playerTasksData.playerTaskItemDatas.Add(taskItemData);

        SaveManager.instance.SavePlayerTasksData();

        // Resetting values of Task Creation Menu
        titleInput.text = "";
        descriptionInput.text = "";

        createTaskMenu.gameObject.SetActive(false);
    }

    public void OnCreateTaskCancelButton()
    {
        createTaskMenu.gameObject.SetActive(false);
    }

    public void OnBackButton()
    {
        TurnOff(true);
    }
}


[System.Serializable]
public class PlayerTasksData
{
    public List<PlayerTaskItemData> playerTaskItemDatas = new List<PlayerTaskItemData>();
}

[System.Serializable]
public class PlayerTaskItemData
{
    public string taskTitle;
    public string taskDescription;
    public TaskItem.Difficulty difficulty;
}