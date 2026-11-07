using UnityEngine;

namespace WeightlessWayfare.EntityStates.Player
{
    public class PlayerFlightState : EntityState
    {
        private const float SPEED = 5f;
        public override void OnEnter()
        {
            base.OnEnter();
            animator.Play("Flight");
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            rigidbody2D.MovePosition(transform.position + (transform.right * SPEED * Time.fixedDeltaTime));
        }
    }
}
