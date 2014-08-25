using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class RenderQ : MonoBehaviour {

    public int renderQ = 3500;

	// Use this for initialization
	void Start () {

        transform.renderer.material.renderQueue = renderQ;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
