

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// This class is used to identify objects using "Groups"
/// </para>
/// <para>
/// Existing Regedit: pickUpScale
/// </para>
/// Updated: 3/15/2019<para/>
/// Author: Yan Xiao<para/>
/// Attached object: All Objects <para/>
/// Updates: 3/15 add regedit to the identifier (xy)<para/>
/// Updates: 3/29 add pickUpDistance 
/// </summary>
public class Identifier : MonoBehaviour {

    public List<string> group = new List<string>();
    public Dictionary<string, object> regedit = new Dictionary<string, object>();
    public List<string> initRegeditName = new List<string>();
    public List<float> initRegeditData = new List<float>();   

	void Start()
    {
        group.Add("default");
        for (int i = 0; i < initRegeditData.Count; i++) {
            regedit.Add(initRegeditName[i], initRegeditData[i]);
        }
        
    }

    /// <summary>
    /// Get the name of the object written in the scenes
    /// </summary>
    /// <returns>The name</returns>
    public string getName()
    {
        return this.gameObject.name;
    }

    /// <summary>
    /// Set the name of the object
    /// </summary>
    /// <param name="name">Name want to set</param>
    public void setName(string name)
    {
        this.gameObject.name = name;
    }

    /// <summary>
    /// Add a group to the object, an object can have multiple groups 
    /// </summary>
    /// <param name="groupName"></param>
    public void addGroup(string groupName)
    {
        group.Add(groupName);
    }

    /// <summary>
    /// Check if a object belongs to one group
    /// </summary>
    /// <param name="groupName"></param>
    /// <returns></returns>
    public bool isGroup(string groupName)
    {
        return group.Contains(groupName);
    }

    /// <summary>
    /// Delete a specific group from this object
    /// </summary>
    /// <param name="groupName"></param>
    public void deleteGroup(string groupName)
    {
        group.Remove(groupName);
    }

    /// <summary>
    /// Get all groups of this object 
    /// </summary>
    /// <returns></returns>
    public List<string> getAllGroup()
    {
        return group;
    }

    /// <summary>
    /// Put a key value pair to the regedit 
    /// </summary>
    /// <param name="name">Key</param>
    /// <param name="value">value</param>
    public void putRegedit(string name, object value) {
        if (regedit.ContainsKey(name)) { 
            regedit.Remove(name);
        }
        regedit.Add(name, value);
    }

    /// <summary>
    /// Get the key value pair from the regedit by key
    /// </summary>
    /// <param name="name">key</param>
    /// <returns>value</returns>
    public object getRegeditValue(string name) {
        if (!regedit.ContainsKey(name)) {
            return null;
        }
        return regedit[name];
    }

    /// <summary>
    /// Remove a key value pair in regedit
    /// </summary>
    /// <param name="name">key</param>
    public void removeRegedit(string name) {
        regedit.Remove(name);
    }

    /// <summary>
    /// Get the regedit, We should not call is method
    /// </summary>
    /// <returns>regedit</returns>
    public Dictionary<string, object> getRegedit() {
        return regedit;
    }

    /// <summary>
    /// Make a deep copy of other regedit
    /// </summary>
    /// <param name="origin">other regedit</param>
    public void copyIdentifier(Identifier origin) {
        group = new List<string>();
        regedit = new Dictionary<string, object>();
        for (int i = 0; i < origin.getAllGroup().Count; i++) {
            if (origin.getAllGroup()[i] != "default") group.Add(origin.getAllGroup()[i]);
        }
        regedit = new Dictionary<string, object>(origin.getRegedit());
    }
}
