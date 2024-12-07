using System;
using Unity.VisualScripting;

public static class events
{
    //start of events for playerControler --------------------------------------------
    public static event Action<bool> onGroundCheck;
    public static void TriggerGroundCheck(bool state)
    {
        onGroundCheck?.Invoke(state);
    }
    //-------------------
    public static event Action<bool> onForwardLegCheck;

    public static void TriggerForwardLegCheck(bool state)
    {
        onForwardLegCheck?.Invoke(state);
    }
    //-------------------
    public static event Action<bool> onForwardHeadCheck;
    public static void TriggerForwardHeadCheck(bool state)
    {
        onForwardHeadCheck?.Invoke(state);
    }
    //-------------------

    //end of events for playerController---------------------------------------------
}