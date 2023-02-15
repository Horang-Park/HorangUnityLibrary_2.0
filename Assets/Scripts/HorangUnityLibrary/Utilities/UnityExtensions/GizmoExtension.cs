using UnityEngine;

namespace Horang.HorangUnityLibrary.Utilities.UnityExtensions
{
	public static class GizmoExtension
	{
		/// <summary>
		/// Draw wired fan shape on XZ plane.
		/// </summary>
		/// <param name="origin">Shape start point</param>
		/// <param name="direction">Shape drawing direction</param>
		/// <param name="radius">Fan shape radius</param>
		/// <param name="angleRange">Fan shape theta</param>
		/// <param name="step">Step count</param>
		public static void DrawWireFanShape(Vector3 origin, Vector3 direction, float radius, float angleRange, int step = 3)
		{
			radius = radius < 0.0f ? 0.0f : radius;
			angleRange = Mathf.Clamp(angleRange, 1.0f, 360.0f);
			
			var sourceAngle = GetAnglesFromDirection(origin, direction);
			var startPosition = origin;
			var stepAngle = angleRange / step;
			var angle = sourceAngle - angleRange * 0.5f;
			
			for (var drawRound = 0; drawRound <= step; drawRound++)
			{
				var radianAngle = Mathf.Deg2Rad * angle;
				var endPosition = origin;
				
				endPosition += new Vector3(radius * Mathf.Cos(radianAngle), 0, radius * Mathf.Sin(radianAngle));

				Gizmos.DrawLine(startPosition, endPosition);

				angle += stepAngle;
				startPosition = endPosition;
				
				// Draw line to origin
				Gizmos.DrawLine(endPosition, origin);
			}
			
			// Close fan shape line to origin
			Gizmos.DrawLine(startPosition, origin);
		}
		
		private static float GetAnglesFromDirection(Vector3 p, Vector3 d)
		{
			var forwardLimitPosition = p + d;
			var sourceAngle = Mathf.Rad2Deg * Mathf.Atan2(forwardLimitPosition.z - p.z, forwardLimitPosition.x - p.x);

			return sourceAngle;
		}
	}
}