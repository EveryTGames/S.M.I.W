
public class CheckOnGround : Collision_Checker
{
        private void Awake() {
           assignFunction(
            (state) =>
            {

                events.TriggerGroundCheck(state);
            });
    }
}
