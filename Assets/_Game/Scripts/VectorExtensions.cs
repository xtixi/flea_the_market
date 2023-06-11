using System;
using UnityEngine;

namespace YamyStudio.Utilities.Extensions
{
    public static class VectorExtensions
    {
        public static Vector2 XY(this Vector3 v)
        {
            return new Vector2(v.x, v.y);
        }

        public static Vector3 WithX(this Vector3 v, float x)
        {
            return new Vector3(x, v.y, v.z);
        }

        public static Vector3 WithY(this Vector3 v, float y)
        {
            return new Vector3(v.x, y, v.z);
        }

        public static Vector3 WithZ(this Vector3 v, float z)
        {
            return new Vector3(v.x, v.y, z);
        }

        public static Vector2 WithX(this Vector2 v, float x)
        {
            return new Vector2(x, v.y);
        }

        public static Vector2 WithY(this Vector2 v, float y)
        {
            return new Vector2(v.x, y);
        }

        public static Vector3 WithZ(this Vector2 v, float z)
        {
            return new Vector3(v.x, v.y, z);
        }

        // axisDirection - unit vector in direction of an axis (eg, defines a line that passes through zero)
        // point - the point to find nearest on line for
        public static Vector3 NearestPointOnAxis(this Vector3 axisDirection, Vector3 point, bool isNormalized = false)
        {
            if (!isNormalized) axisDirection.Normalize();
            var d = Vector3.Dot(point, axisDirection);
            return axisDirection * d;
        }

        // lineDirection - unit vector in direction of line
        // pointOnLine - a point on the line (allowing us to define an actual line in space)
        // point - the point to find nearest on line for
        public static Vector3 NearestPointOnLine(
            this Vector3 lineDirection, Vector3 point, Vector3 pointOnLine, bool isNormalized = false)
        {
            if (!isNormalized) lineDirection.Normalize();
            var d = Vector3.Dot(point - pointOnLine, lineDirection);
            return pointOnLine + (lineDirection * d);
        }

        public static Vector3 WithAddX(this Vector3 v, float x)
        {
            return new Vector3(v.x + x, v.y, v.z);
        }

        public static Vector3 WithAddY(this Vector3 v, float y)
        {
            return new Vector3(v.x, v.y + y, v.z);
        }

        public static Vector3 WithAddZ(this Vector3 v, float z)
        {
            return new Vector3(v.x, v.y, v.z + z);
        }

        public static Vector2 WithAddX(this Vector2 v, float x)
        {
            return new Vector2(v.x + x, v.y);
        }

        public static Vector2 WithAddY(this Vector2 v, float y)
        {
            return new Vector2(v.x, v.y + y);
        }

        /// <summary>
        /// Check every single value(x, y, z) of Vector3.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static bool IsNaNOrInfinity(this Vector3 v)
        {
            if (float.IsNaN(v.x) || float.IsNaN(v.y) || float.IsNaN(v.z))
            {
                return true;
            }

            return float.IsInfinity(v.x) || float.IsInfinity(v.y) || float.IsInfinity(v.z);
        }
		
        /// <summary>
        /// var finalVelocity = CalculatePhysics(transform.position, target.position, 45); // calculate
        /// rigid.velocity = finalVelocity; // fire!
        /// </summary>
        /// <param name="objectPosition">Throwing object position</param>
        /// <param name="targetPosition">Target position</param>
        /// <param name="initAngle">Throwing init angle</param>
        /// <returns></returns>
        public static Vector3 CalculateShootPhysics(Vector3 objectPosition, Vector3 targetPosition, float initAngle)
        {
            if (initAngle == 0)
            {
                return Vector3.zero;
            }

            try
            {
                Vector3 p = targetPosition;
                float gravity = Physics.gravity.magnitude;

                // Selected angle in radians
                float angle = initAngle * Mathf.Deg2Rad;

                // Positions of this object and the target on the same plane
                Vector3 planarTarget = new Vector3(p.x, 0, p.z);
                Vector3 planarPosition = new Vector3(objectPosition.x, 0, objectPosition.z);

                // Planar distance between objects
                float distance = Vector3.Distance(planarTarget, planarPosition);

                // Distance along the y axis between objects
                float yOffset = objectPosition.y - p.y;

                float initialVelocity = (1 / Mathf.Cos(angle)) *
                                        Mathf.Sqrt((0.5f * gravity * Mathf.Pow(Mathf.Abs(distance), 2)) /
                                                   (Mathf.Abs(distance) * Mathf.Tan(angle) + yOffset));

                Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle),
                    initialVelocity * Mathf.Cos(angle));

                // Rotate our velocity to match the direction between the two objects
                float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPosition) *
                                            (p.x > objectPosition.x ? 1 : -1);

                return Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return Vector3.zero;
            }
        }
    }
}