using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wave.XR.Settings;


public class Locomotion : MonoBehaviour
{
    public float speed = 3f;
    float originalSpeed;
    Transform player;
    Rigidbody playerR;
    bool isMoving = false;
    bool useRigidbody = true;

    // below 3 lines taken from HandManager script
    const string kHandDirectionLeftX = "HandDirectionLeftX", kHandDirectionLeftY = "HandDirectionLeftY", kHandDirectionLeftZ = "HandDirectionLeftZ";
	const string kHandDirectionRightX = "HandDirectionRightX", kHandDirectionRightY = "HandDirectionRightY", kHandDirectionRightZ = "HandDirectionRightZ";
	const string kWristAngularVelocityLeftX = "WristAngularVelocityLeftX", kWristAngularVelocityLeftY = "WristAngularVelocityLeftY", kWristAngularVelocityLeftZ = "WristAngularVelocityLeftZ";

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] p = GameObject.FindGameObjectsWithTag("Player");
        if (p != null && p.Length == 1) {
            player = p[0].transform;
            playerR = p[0].GetComponent<Rigidbody>();
        }

        originalSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving) {
            movePlayerInHandDirection();
        }
    }

    public void toggleMovement(bool state) {
        isMoving = state;
        if (!state) speed = originalSpeed;
    }

    void movePlayerInHandDirection() {
        Vector3 direction = new Vector3();

        // below lines taken from HandManager
        float direction_x = 0, direction_y = 0, direction_z = 0;
        SettingsHelper.GetFloat(kHandDirectionRightX, ref direction_x);
		SettingsHelper.GetFloat(kHandDirectionRightY, ref direction_y);
		SettingsHelper.GetFloat(kHandDirectionRightZ, ref direction_z);
        direction.x = direction_x;
		direction.y = direction_y;
		direction.z = direction_z;

        if (useRigidbody) {
            playerR.MovePosition(player.position + direction * speed * Time.deltaTime);
        } else {
            player.Translate(direction * speed * Time.deltaTime, Space.World);
        }

        // left hand angular velocity to turn
        //float velocity_x = 0, velocity_y = 0, velocity_z = 0;

        //SettingsHelper.GetFloat(kWristAngularVelocityLeftX, ref velocity_x);
		//SettingsHelper.GetFloat(kWristAngularVelocityLeftY, ref velocity_y);
		//ettingsHelper.GetFloat(kWristAngularVelocityLeftZ, ref velocity_z);
        //Debug.Log(velocity_y);
    }

    public void toggleGravity() {
        if(playerR != null) playerR.useGravity = !playerR.useGravity;
    }

    public void increaseSpeed() {
        speed += 0.5f;
    }
}
