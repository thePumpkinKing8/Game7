using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPush : MonoBehaviour
{
    public float pushPower = 2.0f; // 

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Pushable") && Input.GetKey(KeyCode.E)) // 
        {
            Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                Vector2 pushDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                pushDirection.Normalize();

                //
                rb.AddForce(pushDirection * pushPower, ForceMode2D.Impulse);
            }
        }
    }
}
