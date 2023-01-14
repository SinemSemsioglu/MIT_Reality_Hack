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
                Material original = r.material;
                r.material = translucent;

                StartCoroutine(returnToOriginalColor(r, original));
            }
        }
    }

    IEnumerator returnToOriginalColor(Renderer r, Material original) {
         yield return new WaitForSeconds(waitSecs);
         Debug.Log("returning to original material" + original);
         r.material = original;
    }
}
