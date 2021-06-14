using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    public float MaxY;
    public float MinY;
    public float MaxX;
    public float MinX;
    public float platformSpeed;

    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if(MaxY != 0 && MinY != 0)
        {
            float posAuxY = gameObject.transform.position.y + platformSpeed;

            gameObject.transform.position = new Vector3(gameObject.transform.position.x, posAuxY, gameObject.transform.position.z);

            if (posAuxY > MaxY || posAuxY < MinY)
            {
                platformSpeed *= -1;
            }
        }
        else
        {
            float posAuxX = gameObject.transform.position.x + platformSpeed;

            gameObject.transform.position = new Vector3(posAuxX, gameObject.transform.position.y, gameObject.transform.position.z);

            if (posAuxX > MaxX || posAuxX < MinX)
            {
                platformSpeed *= -1;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.collider.transform.SetParent(transform);
            //transform.position = new Vector3(other.transform.position.x, transform.position.y, transform.position.z);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.collider.transform.SetParent(null);
            //transform.position = new Vector3(other.transform.position.x, transform.position.y, transform.position.z);
        }
    }
}
