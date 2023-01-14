using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MaterialSwitch
{
    public static void makeTranslucent()
    {
        GameObject[] barriers = GameObject.FindGameObjectsWithTag("barrier");
        Material translucent = Resources.Load<Material>("Material/M2");
        Debug.Log(translucent);

        foreach (GameObject barrier in barriers)
        {
            if (translucent != null) {
                Renderer r = barrier.GetComponent<Renderer>();
                r.material = translucent;
            }
        }
    }
}
