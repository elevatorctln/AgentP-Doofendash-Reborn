using UnityEngine;

[RequireComponent(typeof(TubeRenderer))]
public class Rope : MonoBehaviour
{
	public float radius = 0.1f;

	private TubeRenderer tubeRenderer;

	private Transform[] ropeJoints;

	private Vector3[] ropeJointPositions;

	public void Awake()
	{
		tubeRenderer = GetComponent<TubeRenderer>();
		ropeJoints = new Transform[base.transform.childCount];
		ropeJointPositions = new Vector3[base.transform.childCount];
		for (int i = 0; i < base.transform.childCount; i++)
		{
			ropeJoints[i] = base.transform.GetChild(i);
		}
	}

	public void Update()
	{
		if (ropeJoints.Length != 0)
		{
			tubeRenderer.enabled = true;
			for (int i = 0; i < ropeJoints.Length; i++)
			{
				ropeJointPositions[i] = ropeJoints[i].localPosition;
			}
			tubeRenderer.SetPoints(ropeJointPositions, radius, Color.white);
		}
	}
}
