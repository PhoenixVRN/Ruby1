using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageab : MonoBehaviour
{
    public AudioClip collectedClip;
    private void OnTriggerStay2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();
        if (controller != null)
        {
            controller.ChangeHealth(-1);
            controller.PlaySound(collectedClip);
            if (controller.health <= 0)
            {
                Debug.Log("Game over");
               
            }
        }
    }
}
