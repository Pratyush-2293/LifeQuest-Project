using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    void Update()
    {
        Vector3 playerPosition = gameObject.transform.position;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            playerPosition.x++;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            playerPosition.x--;
        }

        gameObject.transform.position = playerPosition;
    }
}
