using UnityEngine;

namespace WeightlessWayfare.EntityStates.Player
{
    public class PlayerIdleState : EntityState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            animator.Play("Idle");
            InputHandler.instance.Swipe += Swipe;
        }

        private void Swipe(Vector2 obj)
        {
            PlayerFlightState flightState = new PlayerFlightState();
            transform.SetSpriteForward(obj, sprite);
            outer.SetNextState(flightState);
        }

        public override void OnExit()
        {
            InputHandler.instance.Swipe -= Swipe;
            base.OnExit();
        }
    }
}
