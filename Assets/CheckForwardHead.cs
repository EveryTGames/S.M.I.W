

public class CheckForwardHead : Collision_Checker
{


    private void Awake() {
           assignFunction(
            (state) =>
            {

                events.TriggerForwardHeadCheck(state);
            });
    }
    
    
  

   
}
