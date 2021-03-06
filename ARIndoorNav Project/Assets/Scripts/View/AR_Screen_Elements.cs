﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AR_Screen_Elements : MonoBehaviour
{

    public GameObject _2DArrow;
    public Camera _ARCamera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HideDirectionIndicator()
    {
        _2DArrow.SetActive(false);
    }

    public void IndicateDirection(Vector3 nextCorner)
    {
        // Source: https://answers.unity.com/questions/1037969/arrows-pointing-to-offscreen-enemy.html
        var screenPos = _ARCamera.WorldToViewportPoint(nextCorner);

        if (screenPos.x >= 0 && screenPos.x <= 1 && screenPos.y >= 0 && screenPos.y <= 1 && screenPos.z >= 0)
        {
            _2DArrow.SetActive(false);
            return;
        }

        _2DArrow.SetActive(true);

        var onScreenPos = new Vector2(screenPos.x - 0.5f, screenPos.y - 0.5f) * 2;
        var max = Mathf.Max(Mathf.Abs(onScreenPos.x), Mathf.Abs(onScreenPos.y));
        onScreenPos = (onScreenPos / (max * 2)) + new Vector2(0.5f, 0.5f);

        float x = 0;
        float y = 0;

        /**
         * This part of the code is to counteract a bug found within the app
         * The bug was that after a 90° turn away from the next corner of the path (nextCorner) 
         * the x and y axis would flip their respective values. (For example, if the corner is on the left side
         * the 2D arrow would jump from the left side of the screen to the right)
         * The y axis is still not fixed.
         */
        // Looking away from corner
        if (screenPos.z < Camera.main.nearClipPlane)
        {
            // Right side
            if (screenPos.x >= 0)
                x = 0;
            // Left side
            else
                x = Screen.width;
            y = onScreenPos.y * Screen.height;
        }
        // Looking towards corner
        else
        {
            x = onScreenPos.x * Screen.width;
            y = onScreenPos.y * Screen.height;
        }

        var arrowPos = new Vector3(x, y, 0);
        _2DArrow.transform.position = arrowPos;
    }
}
