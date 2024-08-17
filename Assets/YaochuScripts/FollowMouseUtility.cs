using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouseUtility : MonoBehaviour
{

        void Update()
        {
            // Get the mouse position in screen coordinates
            Vector3 mousePosition = Input.mousePosition;

            // Convert the mouse position to world coordinates
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            // Set the object's position to the mouse position, keeping the z-axis at 0
            transform.position = new Vector3(mousePosition.x, mousePosition.y, 0f);
        }
}
