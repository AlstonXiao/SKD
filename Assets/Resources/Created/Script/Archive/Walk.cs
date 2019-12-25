using UnityEngine;
using System.Collections;

/// <summary>
/// <para>
/// This class is used to make the player walk according to key pressed
/// </para>
/// Updated: 3/19/2019. Added some comment<para/>
/// TODO: Add more comment<para/>
/// Author: Xiao Fei, Yan Xiao<para/>
/// Attached object: player<para/>
/// Updates: 3/19 integrated lowest position check<para/>
/// </summary>
public class Walk : MonoBehaviour
{

    public float speed;
    public float gravity;
    public float lowest_position;
    public Vector3 initial_position;

    [Header("Debug")]
    public Vector3 movement;

    //public float slopeForce;
    //public float slopeForceRayLength;
    //public bool isJumping=false;

    float pushPower = 2.0f;
    private CharacterController charController;

    void Start(){
    }


    protected void FixedUpdate()
    {

        if (Cursor.lockState == CursorLockMode.None)
        {
            return;
        }

        //CharacterController cc = this.GetComponent<CharacterController>();
        charController = this.GetComponent<CharacterController>();
        float moveHorizontal = Input.GetAxis("Horizontal") * 1.0f * Time.deltaTime;
        float moveVertical = Input.GetAxis("Vertical") * 1.0f * Time.deltaTime;

        // get the facing of the current object
        float rot = this.transform.localEulerAngles.y / 360 * 2 * Mathf.PI;
        float horizontal = moveVertical * Mathf.Sin(rot) + moveHorizontal * Mathf.Cos(rot);
        float vertical = moveVertical * Mathf.Cos(rot) - moveHorizontal * Mathf.Sin(rot);

        // transform the movement to xyz coordinate and give them a spead
        movement = new Vector3(horizontal, gravity, vertical);
        charController.Move(movement * speed);

        if (transform.position.y < lowest_position)
        {
            transform.position = initial_position;
        }
    }


    void OnControllerColliderHit(ControllerColliderHit hit)
    {

        //charController = this.GetComponent<CharacterController>();
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

