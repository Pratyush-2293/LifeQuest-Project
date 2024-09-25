using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskItem : MonoBehaviour
{
    public string taskTitle = "";
    public string taskDescription = "";

    public Text titleText = null;
    public Text descriptionText = null;

    private void Start()
    {
        titleText.text = taskTitle;
        descriptionText.text = taskDescription;
    }

    public void OnCompleteButton()
    {
        // Add complete functionality
        Debug.Log("Task Completed!");
        Destroy(gameObject);
    }

    public void OnAbandonButton()
    {
        // Maybe give a warning before ?
        Debug.Log("Task Abandoned!");
        Destroy(gameObject);
    }
}
