using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwitch : MonoBehaviour
{
    int waitSecs = 10;

    public void makeTranslucent()
    {
        GameObject[] barriers = GameObject.FindGameObjectsWithTag("barrier");
        Material translucent = Resources.Load<Material>("Material/M2");

        // TODO currently changes material of all barriers
        foreach (GameObject barrier in barriers)
        {
            if (translucent != null) {
                Renderer r = barrier.GetComponent<Renderer>();
                Collider c = barrier.GetComponent<Collider>();
                c.isTrigger = true;
                Material original = r.material;
                r.material = translucent;

                StartCoroutine(returnToOriginalColor(c, r, original));
            }
        }
    }

    IEnumerator returnToOriginalColor(Collider c, Renderer r, Material original) {
         yield return new WaitForSeconds(waitSecs);
         Debug.Log("returning to original material" + original);
         r.material = original;
         c.isTrigger = false;
    }
}
