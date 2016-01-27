using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Surroundings :  MonoBehaviour
{
	public List<Transform> surroundingBlocks;

	public void CullBlocks (Transform addBlock)
	{
		if (!surroundingBlocks.Contains (addBlock)) {
			surroundingBlocks.Add (addBlock);
		}
	}
}
