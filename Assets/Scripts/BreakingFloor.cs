using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingFloor : MonoBehaviour
{

    public float delayBreak;
    public GameObject particleBreak;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x,
                                                        gameObject.transform.position.y-0.05f,
                                                        gameObject.transform.position.z);
            StartCoroutine(DestroyFloor(other));
        }
    }

    IEnumerator DestroyFloor(Collider2D other)
    {
        yield return new WaitForSeconds(delayBreak);

        if(other)
        {
            Instantiate(particleBreak, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
        }
    }
}
