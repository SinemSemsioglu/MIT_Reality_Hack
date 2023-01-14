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
    public Locomotion loc;

    public Renderer gestureIndicatorLeft;
    public Renderer gestureIndicatorRight;
    
    Material detectedMat;
    Material notDetectedMat;

    MaterialSwitch materialSwitcher;

    // Start is called before the first frame update
    void Start()
    {
        detectedMat = Resources.Load<Material>("Material/GestureDetected");
        notDetectedMat = Resources.Load<Material>("Material/GestureNotDetected");
        
        MaterialSwitch[] materialSwitchers = FindObjectsOfType<MaterialSwitch>();
        if(materialSwitchers != null && materialSwitchers.Length == 1) materialSwitcher = materialSwitchers[0];
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
        if(debugText != null) debugText.text = "left: " + type.ToString();
        // TODO trigger functionality
        if(type == GestureType.OK && materialSwitcher != null) materialSwitcher.makeTranslucent();
        setIndicators(type, true);
    }

    public void RightGestureDetected(GestureType type)
    {
         if(debugText != null) debugText.text = "right " + type.ToString();
        // TODO trigger functionality
        if (type == GestureType.Five && loc != null) loc.toggleMovement(true);
        if (type == GestureType.Fist && loc !=  null) loc.toggleMovement(false);

        setIndicators(type, false);
    }

    private void setIndicators (GestureType type, bool isLeft) {
         // setting the indicators
        if(type == GestureType.Unknown && notDetectedMat) {
            if(isLeft) gestureIndicatorLeft.material = notDetectedMat;
            else gestureIndicatorRight.material = notDetectedMat;
        } else if (detectedMat) {
             if(isLeft) gestureIndicatorLeft.material = detectedMat;
            else gestureIndicatorRight.material = detectedMat;
        }

        // todo return to non-detection state after a while?
    }
}
