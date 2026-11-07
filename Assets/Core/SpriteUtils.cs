using UnityEngine;

namespace WeightlessWayfare
{
    public static class SpriteUtils
    {
        public static void SetSpriteForward(this Transform transform, Vector2 forward, SpriteRenderer sprite)
        {
            transform.right = forward;
            sprite.flipY = Vector2.Dot(forward.normalized, Vector2.left) > 0;
        }
    }
}
