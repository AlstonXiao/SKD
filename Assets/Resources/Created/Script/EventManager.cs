using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using static publicMethods.PublicMethods;



[System.Serializable]
public class MyUnityEvent : UnityEvent<EventInfo>{}

public class EventManager : MonoBehaviour
{
    private Dictionary<TypeOfEvent, MyUnityEvent> eventDictionary;
    private static EventManager _eventManager;
    public static EventManager eventManager
    {
        get{
            if (_eventManager == null){
                _eventManager = GameObject.FindObjectOfType<EventManager>();
            
            if (!_eventManager)
                {
                    Debug.LogError ("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    _eventManager.Init(); 
                }
            }
            return _eventManager;
        }
    }

    void Init(){
        eventDictionary = new Dictionary<TypeOfEvent, MyUnityEvent>();
    }

    public static void startListen(TypeOfEvent theType, UnityAction<EventInfo> listener){
        MyUnityEvent thisevent = null;
        if (eventManager.eventDictionary.TryGetValue(theType, out thisevent)){
            thisevent.AddListener(listener);
        } else {
            thisevent = new MyUnityEvent();
            thisevent.AddListener(listener);
            eventManager.eventDictionary.Add(theType, thisevent);
        }

    }

    public static void StopListening (TypeOfEvent theType, UnityAction<EventInfo> listener)
    {
        if (_eventManager == null) return;
        MyUnityEvent thisEvent = null;
        if (eventManager.eventDictionary.TryGetValue (theType, out thisEvent))
        {
            thisEvent.RemoveListener (listener);
        }
    }

    public static void TriggerEvent (TypeOfEvent theType, EventInfo triggerInfo)
    {
        MyUnityEvent thisEvent = null;
        if (eventManager.eventDictionary.TryGetValue (theType, out thisEvent))
        {
            thisEvent.Invoke(triggerInfo);
        }
    }

}
