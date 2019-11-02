using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// This class is the parent class for all class that need to 
/// receive information from the receiver with type rotation
/// </para>
/// Updated: 3/1/2019<para/>
/// Author: Yan Xiao<para/>
/// Change Log: 3/1 added this class and everything <para/>
/// Attached object: receiver (type:rotation) <para/>
/// Notes: Need to create a new class that inherit from this parent method.
/// </summary>
public class ReceiveRotation : MonoBehaviour {

    public GameObject receiver; // the receiver object
    private float last_rotation_x;
    private int   isRotateX;
    private float threshold_x_per_sec;


    private float last_rotation_y;
    private int   isRotateY;
    private float threshold_y_per_sec;

    private float last_rotation_z;
    private int   isRotateZ;
    private float threshold_z_per_sec;

    /// <summary>
    /// This is the general constructor of the class. Can call this parent constructor to set the receiver
    /// </summary>
    /// <param name="receiver">the receiver that want to get information</param>
    protected void initReceiveRotation(GameObject receiver) {
        this.receiver = receiver;
        threshold_x_per_sec = 10;
        threshold_y_per_sec = 10;
        threshold_z_per_sec = 10;
    }

    // Use this for initialization
    void Start() {
        initReceiveRotation(this.gameObject);
    }

    /// <summary>
    /// If child classes want to override update method, need to call updateRotation()
    /// </summary>
    void Update() {
        updateRotation();
    }

    /// <summary>
    /// This method returns a int corresponding to the rotation status of X axis. 
    /// <para/> 
    /// -1: rotate in counter-clockwise<para/>   0: not rotating <para/>  1: rotate in clock-wise.
    /// </summary>
    /// <returns>-1, 0 or 1</returns>
    public int Check_rotation_x() {
        return isRotateX;

    }

    /// <summary>
    /// This method returns a int corresponding to the rotation status of Y axis. 
    /// <para/> 
    /// -1: rotate in counter-clockwise<para/>   0: not rotating <para/>  1: rotate in clock-wise.
    /// </summary>
    /// <returns>-1, 0 or 1</returns>
    public int Check_rotation_y() {
        return isRotateY;

    }

    /// <summary>
    /// This method returns a int corresponding to the rotation status of Z axis. 
    /// <para/> 
    /// -1: rotate in counter-clockwise<para/>   0: not rotating <para/>  1: rotate in clock-wise.
    /// </summary>
    /// <returns>-1, 0 or 1</returns>
    public int Check_rotation_z() {
        return isRotateZ;

    }
    
    /// <summary>
    /// Private method that update the isRotate.
    /// </summary>
    protected void updateRotation() {
        float nowx = receiver.transform.eulerAngles.x;
        if (Mathf.Abs(nowx - last_rotation_x) <= 180) {
            if (Mathf.Abs((nowx - last_rotation_x) / Time.deltaTime) > threshold_x_per_sec) {
                if (nowx - last_rotation_x > 0) isRotateX = 1;
                else isRotateX = -1;
            }
            else {
                isRotateX = 0;
            }
        } else {
            isRotateX = 0;
        }
        last_rotation_x = nowx;

        float nowy = receiver.transform.eulerAngles.y;
        if (Mathf.Abs(nowy - last_rotation_y) <= 180) {
            if (Mathf.Abs((nowy - last_rotation_y) / Time.deltaTime) > threshold_y_per_sec) {
                if (nowy - last_rotation_y > 0) isRotateY = 1;
                else isRotateY = -1;
            }
            else {
                isRotateY = 0;
            }
        } else {
            isRotateY = 0;
        }
        last_rotation_y = nowy;

        float nowz = receiver.transform.eulerAngles.z;
        if (Mathf.Abs(nowz - last_rotation_z) <= 180) {
            if (Mathf.Abs((nowz - last_rotation_z) / Time.deltaTime) > threshold_z_per_sec) {
                if (nowz - last_rotation_z > 0) isRotateZ = 1;
                else isRotateZ = -1;                    
            }
            else {
                isRotateZ = 0;
            }

        } else {
            isRotateZ = 0;
        }
        last_rotation_z = nowz;
    }

    /// <summary>
    /// Set the threshold of rotation x 
    /// </summary>
    /// <param name="threshold">Degrees per second</param>
    public void set_threshold_x(float threshold){
        threshold_x_per_sec = threshold;
    }

    /// <summary>
    /// Set the threshold of rotation y
    /// </summary>
    /// <param name="threshold">Degrees per second</param>
    public void set_threshold_y(float threshold) {
        threshold_y_per_sec = threshold;
    }

    /// <summary>
    /// Set the threshold of rotation z
    /// </summary>
    /// <param name="threshold">Degrees per second</param>
    public void set_threshold_z(float threshold) {
        threshold_z_per_sec = threshold;
    }


}
