using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wave.XR.Settings;


public class Locomotion : MonoBehaviour
{
    public int speed = 3;
    Transform player;
    bool isMoving = false;

    // below 2 lines taken from HandManager script
    const string kHandDirectionLeftX = "HandDirectionLeftX", kHandDirectionLeftY = "HandDirectionLeftY", kHandDirectionLeftZ = "HandDirectionLeftZ";
	const string kHandDirectionRightX = "HandDirectionRightX", kHandDirectionRightY = "HandDirectionRightY", kHandDirectionRightZ = "HandDirectionRightZ";

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] p = GameObject.FindGameObjectsWithTag("Player");
        if (p != null && p.Length == 1) player = p[0].transform;
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
        player.Translate(direction * speed * Time.deltaTime, Space.World);
    }
}
