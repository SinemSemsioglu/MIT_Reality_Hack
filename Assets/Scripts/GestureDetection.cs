using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//using Wave.Native;
using Wave.Essence.Hand.StaticGesture;
using Wave.Essence.Hand;
using TMPro;

public class GestureDetection : MonoBehaviour
{
    [Header("Push")]
    [SerializeField] float pushForce = 1500;
    [SerializeField] float pushRadius = 15;
    [SerializeField] float cooldown = 1;
    [Header("Size")]
    [SerializeField] float shrinkMod = .25f;
    [SerializeField] float lerpTime = 3f;
    float growMod;

    [Header("Path Find")]
    [SerializeField] GameObject pathFinder;

    [Header ("Other")]
    [SerializeField] TextMeshPro debugText;
    [SerializeField] Transform user;
    [SerializeField] LayerMask pushableLayer;

    GestureType leftGesture;
    GestureType rightGesture;
    [SerializeField] GameObject leftHand;
    [SerializeField] GameObject rightHand;

    Vector3 leftPreviousPos;
    Vector3 rightPreviousPos;

    Vector3 leftVelocity;
    Vector3 rightVelocity;

    bool canUsePower = true;

    [SerializeField] Powers p;

    [Header ("Locomotion")]
    public Locomotion loc;

    [Header ("Gesture Indicators")]
    public Renderer gestureIndicatorLeft;
    public Renderer gestureIndicatorRight;
    Material detectedMat;
    Material notDetectedMat;

    

    private void Start()
    {
        growMod = 1 / shrinkMod;
        canUsePower = true;

        //p.FindPath(pathFinder, transform.position);

        detectedMat = Resources.Load<Material>("Material/GestureDetected");
        notDetectedMat = Resources.Load<Material>("Material/GestureNotDetected");

    }

    // Update is called once per frame
    void Update()
    {
        //GetHandVelocity();
     
        CheckDualFists();
        //CheckDuelPeace();
        CheckThumbsUP();
    }

    void GetHandVelocity()
    {
        //Left hand
        leftVelocity = (leftHand.transform.position - leftPreviousPos) / Time.deltaTime;
        leftPreviousPos = leftHand.transform.position;

        //Right hand
        rightVelocity = (rightHand.transform.position - rightPreviousPos) / Time.deltaTime;
        rightPreviousPos = rightHand.transform.position;

        if (debugText != null) debugText.text = "LH velocity : " + leftVelocity.magnitude + " : RH velocity : " + rightVelocity.magnitude;
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
        leftGesture = type;

        //Reloading scene, delete later
        //if (type == GestureType.Like) SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        if(debugText != null) debugText.text = "left: " + type.ToString();
        if(type == GestureType.Victory && p != null) p.makeTranslucent();
        setIndicators(type, true);
    }

    public void RightGestureDetected(GestureType type)
    {
        rightGesture = type;

        if(debugText != null) debugText.text = "right " + type.ToString();

        if (type == GestureType.Five && loc != null) loc.toggleMovement(true);
        if (type == GestureType.Fist && loc !=  null) loc.toggleMovement(false);
        if (type == GestureType.Like && loc != null) {
            //loc.toggleGravity();
            loc.increaseSpeed();
        }

        setIndicators(type, false);
    }

    void CheckDualFists()
    {
        if (canUsePower && leftGesture == GestureType.Fist && rightGesture == GestureType.Fist)
        {
            p.Push(transform, pushForce, pushRadius, pushableLayer);
            StartCoroutine(Cooldown());
        }
    }

    void CheckDuelPeace()
    {
        if (canUsePower && leftGesture == GestureType.Victory && rightGesture == GestureType.Victory)
        {
            p.FindPath(pathFinder, transform.position);
            StartCoroutine(Cooldown());
        }
    }

    //Make flip flop so that can grow
    bool isShrunk = false;
    void CheckThumbsUP()
    {
        if (!isShrunk && canUsePower && leftGesture == GestureType.OK)
        {
            p.ShrinkSelf(transform, shrinkMod, lerpTime);
            StartCoroutine(Cooldown());
            isShrunk = true;
        }
        if (isShrunk && canUsePower && leftGesture == GestureType.OK)
        {
            p.GrowSelf(transform, growMod, lerpTime);
            StartCoroutine(Cooldown());
            isShrunk = false;
        }
    }

    IEnumerator Cooldown()
    {
        canUsePower = false;
        yield return new WaitForSeconds(cooldown);
        canUsePower = true;
    }

    private void setIndicators (GestureType type, bool isLeft) {
        if(type == GestureType.Unknown && notDetectedMat) {
            if(isLeft && gestureIndicatorLeft != null) gestureIndicatorLeft.material = notDetectedMat;
            else if (gestureIndicatorRight != null) gestureIndicatorRight.material = notDetectedMat;
        } else if (detectedMat) {
             if(isLeft && gestureIndicatorLeft != null) gestureIndicatorLeft.material = detectedMat;
            else if (gestureIndicatorRight != null) gestureIndicatorRight.material = detectedMat;
        }
    }
}
