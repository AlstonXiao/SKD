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
    /// Function lists: identify(objects) <para/>
    /// Updated: 3/19/2019 <para/>
    /// Author: Yan Xiao<para/>
    /// Attached object: All scripts<para/>
    /// </summary>
    
    public static class PublicMethods
    {
        public enum Hands {free, pickUpAble, cutted, gem};
        public enum TypeOfEvent{die, test, doorText};
        /// <summary>
        /// This method is used to reduce the length of the code. It just simply get the identifier of the object
        /// </summary>
        /// <param name="objects">Objects you want to identify</param>
        /// <returns>The Identifier attached to it</returns>
        public static Identifier identify(GameObject objects) {
            
            return objects.GetComponent<Identifier>();
        }
    }
}