using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//using Wave.Native;
using Wave.Essence.Hand.StaticGesture;
using Wave.Essence.Hand;
using TMPro;


public class GestureDetection : MonoBehaviour
{
    public TextMeshPro debugText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /* List of types
		GestureType.Fist;
		GestureType.Five;
		GestureType.Point;
		GestureType.OK;
		GestureType.Like;
		GestureType.Victory;
    */
    public void LeftGestureDetected(GestureType type)
    {
        debugText.text = "left: " + type.ToString();
        // TODO trigger functionality
    }

    public void RightGestureDetected(GestureType type)
    {
        debugText.text = "right " + type.ToString();
        // TODO trigger functionality
    }
}
