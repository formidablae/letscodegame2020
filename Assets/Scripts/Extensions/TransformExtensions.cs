using UnityEngine;

namespace Apocalypse {
    public static class TransformExtensions {
        public static void FlipToDirection2D(this Transform transform, in Vector2 moveVec) {
            if (moveVec == Vector2.zero)
                return;

            float angle = Mathf.Atan2(0f, moveVec.x) * Mathf.Rad2Deg + 180f;

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
        }
    }
}
