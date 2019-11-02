
using UnityEngine;
using System.Collections;

/// <summary>
/// <para>
/// This is used to make the camera following the charactor.
/// </para>
/// Updated: 2/4/2019<para/>
/// Author: Yan Xiao<para/>
/// Attached object: Main Camera <para/>
/// </summary>
public class CameraController : MonoBehaviour
{
    public GameObject player;
    public double movingAmplitute = 0.2;
    public float movingPeroid = 15;

    private Vector3 offset;
    private Vector3 movingOffset = new Vector3(0, 0, 0);

    void Start()
    {
        offset = transform.position - player.transform.position;

    }
  
    void LateUpdate()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0){
            movingOffset.y = (float)movingAmplitute * Mathf.Sin(((float)Time.frameCount) / movingPeroid);
        }
        transform.position = player.transform.position + offset + movingOffset;
    }
}
