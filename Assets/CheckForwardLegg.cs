

public class CheckForwardLegg : Collision_Checker
{
        private void Awake() {
              assignFunction(
            (state) =>
            {

                events.TriggerForwardLegCheck(state);
            });
    }
    
  

}
