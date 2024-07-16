using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GemShapeList", menuName = "ScriptableObject/GemShapeList", order = int.MaxValue)]
public class GemShapeList : ScriptableObject
{
	public List<GameObject> shapeList;
}
