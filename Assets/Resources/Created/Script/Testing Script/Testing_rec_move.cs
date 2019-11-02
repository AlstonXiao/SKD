using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a example of using the receive movement 
/// </summary>
public class Testing_rec_move : ReceiveMovement
{
    public int x;
    public int y;
    public int z;
    // Start is called before the first frame update
    void Start()
    {
        initReceiveMovement(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        updatePosition();
        x = Check_position_x();
        y = Check_position_y();
        z = Check_position_z();
    }
}
