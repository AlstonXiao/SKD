using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static publicMethods.PublicMethods;

/// <summary>
/// <para>
/// This class is used to identify the status of player
/// </para>
/// enum Available: {free, pickUpAble, cutted, gem} <para/>
/// Updated: 12/25/2019. Add more comment and change it to a enum controlled system<para/>
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

    /// <summary>
    /// Check the current status
    /// </summary>
    /// <returns>current status</returns>
    public Hands Hands_status() {
        return PlayerHands;
    }

    /// <summary>
    /// Change the status of the hand from free to a new status. True if the change succeed, false if the hand is not free
    /// </summary>
    /// <param name="newstatus">New status you want</param>
    /// <returns>true if the change succeed, false if the hand is not free</returns>
    public bool Hands_change(Hands newstatus){
        if (PlayerHands != Hands.free) return false;
        PlayerHands = newstatus;
        return true; 
    }

    /// <summary>
    /// Free the hand from the previous status. Only free when the type match. True when succeed, false when the current status don't match
    /// </summary>
    /// <param name="currentStatus">the status you want to free from</param>
    /// <returns>True when succeed, false when the current status don't match</returns>
    public bool Hands_free(Hands currentStatus){
        if (PlayerHands != currentStatus) return false;
        PlayerHands = Hands.free;
        return true; 
    }
}
