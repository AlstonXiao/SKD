using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static publicMethods.PublicMethods;

/// <summary>
/// <para>
/// This class is used to identify the status of player
/// </para>
/// Hands Available: pick up object, place gem <para/>
/// Updated: 2/4/2019. Added some comment<para/>
/// Author: Yan Xiao<para/>
/// Attached object: player<para/>
/// </summary>
public class player_status : MonoBehaviour {
    public Hands    PlayerHands;
    public int      lowest_position;
    pickUpObject    pick_script;
    placeGem        place_script;
    Vector3         initial_position;
    
    
	// Use this for initialization
	void Start () {
        pick_script = this.GetComponent<pickUpObject>();
        place_script = this.GetComponent<placeGem>();
        initial_position = transform.position;
	}
	
    /// <summary>
    /// This method is used to check if the user's hands is available to interact with other things
    /// </summary>
    /// <returns></returns>
    public bool Hands_avaliable() {
        return (PlayerHands == Hands.free);
    }

    public Hands Hands_status() {
        return PlayerHands;
    }

    public bool Hands_change(Hands newstatus){
        if (PlayerHands != Hands.free) return false;
        PlayerHands = newstatus;
        return true; 
    }

    public bool Hands_free(Hands currentStatus){
        if (PlayerHands != currentStatus) return false;
        PlayerHands = Hands.free;
        return true; 
    }


    void Update() {
        
    }
}
