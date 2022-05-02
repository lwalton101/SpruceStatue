using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;
	[SerializeField] private float xOffset;
	[SerializeField] private float yOffset;

	// Update is called once per frame
	void Update()
    {
	    if (playerTransform != null)
	    {
		    transform.position = new Vector3(playerTransform.position.x + xOffset, playerTransform.position.y + yOffset, -10);
	    }
    }
}
