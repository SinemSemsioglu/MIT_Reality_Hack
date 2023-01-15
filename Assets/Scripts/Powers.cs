using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powers : MonoBehaviour
{
    public static Powers Instance;

    [SerializeField] GameObject pushParticle;
    [SerializeField] GameObject pathParticle;
    [SerializeField] GameObject shrinkParticle;
    [SerializeField] Transform spawnParticlePos;

    private void Start()
    {
        Instance = this;
    }
    public void Push(Transform player, float force, float radius, LayerMask pushableObject)
    {
        Collider[] colliders = Physics.OverlapSphere(player.position, radius, pushableObject);

        Instantiate(pushParticle, spawnParticlePos.position, pushParticle.transform.rotation);

        foreach(Collider collider in colliders)
        {
            Rigidbody rb = collider.gameObject.GetComponent<Rigidbody>();

            rb.AddExplosionForce(force, player.position, radius);
        }
    }

    public void FindPath(GameObject light, Vector3 playerPosition)
    {
        Instantiate(light,playerPosition, light.transform.rotation);

        Instantiate(pathParticle, spawnParticlePos.position, pushParticle.transform.rotation);
    }

    bool shrunk = false;

    public void ShrinkSelf(Transform player, float shrinkMod, float lerpTime)
    {
        if (!shrunk)
        {
            StartCoroutine(LerpScale(lerpTime, player, shrinkMod));
        }
    }

    public void GrowSelf(Transform player, float growMod, float lerpTime)
    {
        if (shrunk == false) StartCoroutine(LerpScale(lerpTime, player, growMod));
    }
    
    IEnumerator LerpScale(float lerpTime, Transform playerTransform, float mod)
    {
        Instantiate(shrinkParticle, spawnParticlePos.position, pushParticle.transform.rotation);
        float time = 0;
        float origScale = playerTransform.localScale.x;
   
        while (time < lerpTime)
        {
            float newScale = Mathf.Lerp(origScale, origScale * mod, time/lerpTime);
            playerTransform.localScale = new Vector3(newScale, newScale, newScale);

            yield return null;
            time += Time.deltaTime;
        }
        float value = origScale * mod;
        playerTransform.localScale = new Vector3(value, value, value);
    }
}