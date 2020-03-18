using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static publicMethods.PublicMethods;

/// <summary>
/// <para>
/// This class is used to identify the status of player
/// </para>
/// Hands enum Available: {free, pickUpAble, cutted, gem} <para/>
/// Screen enum Available: {free, inventory} <para/>
/// Updated: 3/17/2020. Add the status of the screen <para/>
/// Updated: 12/25/2019. Add more comment and change it to a enum controlled system<para/>
/// Updated: 2/4/2019. Added some comment<para/>
/// Author: Yan Xiao<para/>
/// Attached object: player<para/>
/// </summary>
public class player_status : MonoBehaviour {
    public Hands            PlayerHands;
    public ScreenLockStatus screenStatus;
    public int              lowest_position;
    Vector3                 initial_position;
    
    
	// Use this for initialization
	void Start () {
        initial_position = transform.position;
	}
	
    /// <summary>
    /// This method is used to check if the user's hands is available to interact with other things
    /// </summary>
    /// <returns></returns>
    public bool Hands_available() {
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
    /// To see if the player can swap items from inventory. 
    /// </summary>
    /// <returns>True if the hand is free, or holding things that can be put in inventory</returns>
    public bool Hands_swap_able()
    {
        return PlayerHands == Hands.free || PlayerHands == Hands.pickUpAble || PlayerHands == Hands.cutted;
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

    /// <summary>
    /// Check if the screen is free to move around 
    /// </summary>
    /// <returns>return true if screen is free</returns>
    public bool Scree_free()
    {
        return screenStatus == ScreenLockStatus.free;
    }

    /// <summary>
    /// Lock the screen to a state. So some actions are limited
    /// </summary>
    /// <param name="newstatus">new status we want to change the screen</param>
    /// <returns>True if success, false otherwise</returns>
    public bool Screen_lock(ScreenLockStatus newstatus)
    {
        if (screenStatus != ScreenLockStatus.free) return false;
        screenStatus = newstatus;
        return true;
    }

    /// <summary>
    /// Unlock the screen to "free" status and player can move around
    /// </summary>
    /// <param name="currentStatus">The status you want to unlock screen from</param>
    /// <returns>true if success</returns>
    public bool Screen_unloock(ScreenLockStatus currentStatus)
    {
        if (screenStatus != currentStatus) return false;
        screenStatus = ScreenLockStatus.free;
        return true;
    }
}
