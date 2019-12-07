using UnityEngine;

namespace ForceField2D {
	public static class ForceField2D {

		/// <summary>
		/// Rotates the vector2 by angle
		/// </summary>
		/// <returns>The vector2.</returns>
		/// <param name="v">V.</param>
		/// <param name="angle">Angle (In degrees).</param>
		public static Vector2 RotateVector2(this Vector2 v, float angle) {
			float radian = angle*Mathf.Deg2Rad * -1f;
			float _x = v.x*Mathf.Cos(radian) - v.y*Mathf.Sin(radian);
			float _y = v.x*Mathf.Sin(radian) + v.y*Mathf.Cos(radian);
			return new Vector2(_x,_y);
		}
	
	}
}
