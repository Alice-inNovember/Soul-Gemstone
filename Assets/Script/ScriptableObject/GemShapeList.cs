using System.Collections.Generic;
using UnityEngine;

namespace Script.ScriptableObject
{
	[CreateAssetMenu(fileName = "GemShapeList", menuName = "ScriptableObject/GemShapeList", order = int.MaxValue)]
	public class GemShapeList : UnityEngine.ScriptableObject
	{
		public List<GameObject> shapeList;
	}
}
