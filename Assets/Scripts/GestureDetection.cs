using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using Wave.Native;
using Wave.Essence.Hand.StaticGesture;

public class GestureDetection : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (HandManager.Instance != null) {
            // left hand
            //Debug.Log("left hand " + WXRGestureHand.GetSingleHandGesture(true));
            Debug.Log("left hand " + CustomGestureProvider.GetHandGesture(true));

            // right hand
            //Debug.Log("right hand " + WXRGestureHand.GetSingleHandGesture(false));
            Debug.Log("right hand " + CustomGestureProvider.GetHandGesture(false));
        //}
    }
}
