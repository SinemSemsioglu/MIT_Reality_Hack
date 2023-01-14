using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Powers
{
    public static void Push(Vector3 playerPosition, float force, float radius, LayerMask pushableObject)
    {
        Collider[] colliders = Physics.OverlapSphere(playerPosition, radius, pushableObject);

        foreach(Collider collider in colliders)
        {
            Rigidbody rb = collider.gameObject.GetComponent<Rigidbody>();

            rb.AddExplosionForce(force, playerPosition, radius);
        }
    }

    public static void FindPath(GameObject light, Vector3 playerPosition)
    {
        MonoBehaviour.Instantiate(light,playerPosition, light.transform.rotation);
    }

    static bool shrunk = false;

    public static void ShrinkSelf(Transform player, float shrinkMod, float lerpTime, float targetValue)
    {
        if (!shrunk) player.localScale *= shrinkMod;
        //LerpScale(lerpTime, player, targetValue);
    }

    public static void GrowSelf(Transform player, float shrinkMod, float lerpTime, float targetValue)
    {
        if (shrunk == false) player.localScale /= shrinkMod;
        //LerpScale(lerpTime, player, targetValue);
    }

    /*
    static void LerpScale(float lerpTime, Transform player, float targetValue)
    {
        //MonoClass.Instance.StartCoroutine(LerpScale(lerpTime, player, targetValue));
    }
    */
}