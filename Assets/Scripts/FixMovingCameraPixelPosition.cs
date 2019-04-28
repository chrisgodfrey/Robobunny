using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixMovingCameraPixelPosition : MonoBehaviour
{

    // Force the camera to follow the player around the level
    // 

    public GameObject player;
    public Camera main_camera;

    void Update()
    {
        if (player != null)
        {
            float player_x = player.transform.position.x;
            float player_y = player.transform.position.y;

            float rounded_x = RoundToNearestPixel(player_x);
            float rounded_y = RoundToNearestPixel(player_y);

            // move camera x & y to follow player up & down
            //Vector3 new_pos = new Vector3(rounded_x, rounded_y, -10.0f); // this is 2d, so my camera is that far from the screen.

            // only move camera x to lock camera y position in-place, like in super mario world
            Vector3 new_pos = new Vector3(rounded_x, main_camera.transform.position.y, -10.0f); // this is 2d, so my camera is that far from the screen.

            main_camera.transform.position = new_pos;
        }
    }
    public float pixelToUnits = 16f;

    public float RoundToNearestPixel(float unityUnits)
    {
        float valueInPixels = unityUnits * pixelToUnits;
        valueInPixels = Mathf.Round(valueInPixels);
        float roundedUnityUnits = valueInPixels * (1 / pixelToUnits);
        return roundedUnityUnits;
    }
}
