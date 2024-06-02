using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// It's just a quick and dirty script to generate a random level to make it easier to understand where we are going.
/// Please ignore this from evalutation point of view.
/// </summary>
public class PatchGenerator : MonoBehaviour {

	[SerializeField]
	private Transform leftBound;
	[SerializeField]
	private Transform bottomBound;

	[SerializeField]
	private List<GameObject> props;
	[SerializeField]
	private int propCountPerArea;

	void Start() {
		float left = leftBound.position.x;
		float bottom = bottomBound.position.y;
		for (int i = 0; i < propCountPerArea; i++) {
			Instantiate(
				props[Random.Range(0, props.Count)],
				new Vector3(Random.Range(left, -left), Random.Range(bottom, -bottom), 0),
				Quaternion.identity,
				transform
			);
		}
	}
}
