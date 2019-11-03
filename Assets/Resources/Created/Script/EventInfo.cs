using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventInfo {
    public string EventDescription;
}

public class DebugEventInfo : EventInfo {
    public int VerbosityLevel;

}
public class UnitDeathEventInfo : EventInfo {
    public GameObject UnitGO;
}
public class NoInfo : EventInfo {
    
}

public class doorTextInfo : EventInfo {
    
}