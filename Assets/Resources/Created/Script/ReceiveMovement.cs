using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// <para>
/// This class is the parent class for all class that need to 
/// receive information from the receiver with type position
/// </para>
/// Updated: 3/29/2019<para/>
/// Author: Yan Xiao<para/>
/// Change Log: 3/29 copied from receive rotation and finished<para/>
/// Attached object: receiver (type:movement)<para/>
/// Notes: Need to create a new class that inherit from this parent method.
/// </summary>
public class ReceiveMovement : MonoBehaviour
{
    public GameObject receiver; // the receiver object
    private float last_position_x;
    private int isMovingX;
    private float threshold_x_per_sec;


    private float last_position_y;
    private int isMovingY;
    private float threshold_y_per_sec;

    private float last_position_z;
    private int isMovingZ;
    private float threshold_z_per_sec;

    /// <summary>
    /// This is the general constructor of the class. Can call this parent constructor to set the receiver
    /// </summary>
    /// <param name="receiver">the receiver that want to get information</param>
    protected void initReceiveMovement(GameObject receiver) {
        this.receiver = receiver;
        threshold_x_per_sec = 0.2F;
        threshold_y_per_sec = 0.2f;
        threshold_z_per_sec = 0.2f;
    }

    // Use this for initialization
    void Start() {
    }

    /// <summary>
    /// If child classes want to override update method, need to call updatePosition()
    /// </summary>
    void Update() {
        updatePosition();
    }

    /// <summary>
    /// This method returns a int corresponding to the moving status of X axis. 
    /// <para/> 
    /// -1: move in negative x direction<para/>   0: not moving <para/>  1: move in positive x direction
    /// </summary>
    /// <returns>-1, 0 or 1</returns>
    public int Check_position_x() {
        return isMovingX;

    }

    /// <summary>
    /// This method returns a int corresponding to the moving status of Y axis. 
    /// <para/> 
    /// -1: move in negative y direction<para/>   0: not moving <para/>  1: move in positive y direction
    /// </summary>
    /// <returns>-1, 0 or 1</returns>
    public int Check_position_y() {
        return isMovingY;

    }

    /// <summary>
    /// This method returns a int corresponding to the moving status of Z axis. 
    /// <para/> 
    /// -1: move in negative z direction<para/>   0: not moving<para/>  1: move in positive z direction
    /// </summary>
    /// <returns>-1, 0 or 1</returns>
    public int Check_position_z() {
        return isMovingZ;

    }

    /// <summary>
    /// Private method that update the isMoving.
    /// </summary>
    protected void updatePosition() {
        float nowx = receiver.transform.position.x;
        if (Mathf.Abs((nowx - last_position_x) / Time.deltaTime) > threshold_x_per_sec) {
            if (nowx - last_position_x > 0) isMovingX = 1;
            else isMovingX = -1;
        }
        else {
            isMovingX = 0;
        }
        last_position_x = nowx;

        float nowy = receiver.transform.position.y;
        if (Mathf.Abs((nowy - last_position_y) / Time.deltaTime) > threshold_y_per_sec) {
            if (nowy - last_position_y > 0) isMovingY = 1;
            else isMovingY = -1;
        }
        else {
            isMovingY = 0;
        }
        last_position_y = nowy;

        float nowz = receiver.transform.position.z;
        if (Mathf.Abs((nowz - last_position_z) / Time.deltaTime) > threshold_z_per_sec) {
            if (nowz - last_position_z > 0) isMovingZ = 1;
            else isMovingZ = -1;
        }
        else {
            isMovingZ = 0;
        }

        last_position_z = nowz;
    }

    /// <summary>
    /// Set the threshold of position x 
    /// </summary>
    /// <param name="threshold">Meters per second</param>
    public void set_threshold_x(float threshold) {
        threshold_x_per_sec = threshold;
    }

    /// <summary>
    /// Set the threshold of position y
    /// </summary>
    /// <param name="threshold">Meters per second</param>
    public void set_threshold_y(float threshold) {
        threshold_y_per_sec = threshold;
    }

    /// <summary>
    /// Set the threshold of position z
    /// </summary>
    /// <param name="threshold">Meters per second</param>
    public void set_threshold_z(float threshold) {
        threshold_z_per_sec = threshold;
    }
}
