using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class provide more detailed physical interation between the player and the world
/// </summary>
public class PhysicsOfPlayer : MonoBehaviour
{
    public float pushPower; // how fast can the player push the object


    /// <summary>
    /// This method is called when the player touch an object
    /// </summary>
    /// <param name="hit">the collision</param>
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // no rigidbody
        if (body == null || body.isKinematic)
        {
            return;
        }

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3)
        {
            return;
        }

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.

        // Apply the push
        body.velocity = pushDir * pushPower;
    }
}
