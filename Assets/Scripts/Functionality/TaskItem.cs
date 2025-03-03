using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskItem : MonoBehaviour
{
    public string taskTitle = "";
    public string taskDescription = "";
    public enum Difficulty { easy, medium, hard};
    public Difficulty taskDifficulty = Difficulty.easy;

    public Text titleText = null;
    public Text descriptionText = null;
    public int taskIndex;

    public GameObject diffCounterEasy = null;
    public GameObject diffCounterMedium = null;
    public GameObject diffCounterHard = null;

    private void Start()
    {
        titleText.text = taskTitle;
        descriptionText.text = taskDescription;
        SetDifficultyCounters();
    }

    private void SetDifficultyCounters()
    {
        if(taskDifficulty == Difficulty.easy)
        {
            diffCounterEasy.gameObject.SetActive(true);
        }
        else if(taskDifficulty == Difficulty.medium)
        {
            diffCounterEasy.gameObject.SetActive(true);
            diffCounterMedium.gameObject.SetActive(true);
        }
        else
        {
            diffCounterEasy.gameObject.SetActive(true);
            diffCounterMedium.gameObject.SetActive(true);
            diffCounterHard.gameObject.SetActive(true);
        }
    }

    public void OnCompleteButton()
    {
        // Add complete functionality
        Debug.Log("Task Completed!");
        TaskListMenu.instance.RemoveTaskItemData(taskTitle);
        SaveManager.instance.SavePlayerTasksData();

        Destroy(gameObject);
    }

    public void OnAbandonButton()
    {
        // Maybe give a warning before ?
        Debug.Log("Task Abandoned!");
        TaskListMenu.instance.RemoveTaskItemData(taskTitle);
        SaveManager.instance.SavePlayerTasksData();

        Destroy(gameObject);
    }
}
