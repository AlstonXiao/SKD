using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This name space provides some useful function tat is needed
/// </summary>
namespace publicMethods
{
    /// <summary>
    /// This class provides some useful function tat is needed <para />
    /// Function lists: identify(objects), findTopParentOfTheType(groupName, currentObject) <para/>
    /// Updated: 3/15/2020 <para/>
    /// Author: Yan Xiao<para/>
    /// Attached object: All scripts<para/>
    /// </summary>
    public static class PublicMethods
    {
        public enum Hands {free, pickUpAble, cutted, gem};
        public enum TypeOfEvent{die, test, doorText, doorTextOut};
        public enum ScreenLockStatus {free, inventory};

        /// <summary>
        /// This method is used to reduce the length of the code. It just simply get the identifier of the object
        /// </summary>
        /// <param name="objects">Objects you want to identify</param>
        /// <returns>The Identifier attached to it</returns>
        public static Identifier identify(GameObject objects) {
            
            return objects.GetComponent<Identifier>();
        }


        /// <summary>
        /// This method is used when we want to get the top parent which have the same group of the original object.<para/>
        /// It is used when a complicated object is build by multiple hierachy
        /// </summary>
        /// <param name="groupName">the name of the group to find parent</param>
        /// <param name="currentObject">current object, might be a small piece of the complicated obejct</param>
        /// <returns></returns>
        public static GameObject findTopParentOfTheType(string groupName, GameObject currentObject) {
            GameObject topParent = currentObject;
            while (topParent.transform.parent != null && topParent.transform.parent.gameObject != null)
            {
                if (identify(topParent.transform.parent.gameObject) == null || !identify(topParent.transform.parent.gameObject).isGroup(groupName))
                {
                    break;
                }
                topParent = topParent.transform.parent.gameObject;
            }
            return topParent;
        }
    }
}