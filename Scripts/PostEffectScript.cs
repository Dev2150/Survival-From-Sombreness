using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PostEffectScript : MonoBehaviour {

	const int MAX_SHADERS = 7;
	private Material mat;
    private Material[] mats;
    private Shader[] shaders;
	private int i_currentMaterial = 0;

//	// BH
//	public Shader shader;
//	public Transform t_blackhole;
//	public float f_ratio;
//	public float f_radius;
//
//
//	Camera cam;
//	Vector3 wtsp;
//	Vector2 pos;
//	Material _material;
//
//	Material material {
//		get {
//			if (_material == null) {
//				_material = new Material (shader);
//				_material.hideFlags = HideFlags.HideAndDontSave;
//			}
//			return _material;
//		}
//	}
//
//
//
//
//	void OnEnable () {
//		cam = GetComponent<Camera> ();
//		f_ratio = 1f / cam.aspect; 
//	}
//
//	void OnDisable() {
//		if (_material) {
//			DestroyImmediate (_material);
//		}
//	}

	void Awake () {
		mat =  Resources.Load ("Camera/TOX0", typeof(Material)) as Material;
        mats = new Material[7];
        for (int i = 0; i < MAX_SHADERS; i++)
			mats [i] = Resources.Load ("Camera/TOX" + i, typeof(Material)) as Material;
        mats[0] = mat;

        shaders = new Shader[7];
        for (int i = 0; i < MAX_SHADERS; i++)
			shaders[i] = Shader.Find ("Custom/TOX" + i);
    }



	void OnRenderImage ( RenderTexture src, RenderTexture dst) {
		// src -> standard fully rendered scene
		// : would normally be sent directly to the monitor
		for (int i = 0; i < MAX_SHADERS; i++)
			if (i_currentMaterial == i) {
				Graphics.Blit (src, dst, mats[i]);//mats [i]);
			}

		// processing
//		if (shader && material && t_blackhole) {
//			wtsp = cam.WorldToScreenPoint (t_blackhole.position);
//
//			// if BH is in front of camera
//			if (wtsp.z > 0) {
//				pos = new Vector2 (wtsp.x / cam.pixelWidth, wtsp.y / cam.pixelHeight);
//				// apply shader parameters
//				_material.SetVector("_SetPosition", pos);
//				_material.SetFloat ("_SetRatio", f_ratio);
//				_material.SetFloat ("f_radius", f_radius);
//				_material.SetFloat("_Distance", Vector3.Distance(t_blackhole.position, transform.position));
//
//				Graphics.Blit (src, dst, material);
			}
		//}
//	}

	public void changeTo (int i) {
		//Camera.RenderWithShader (shaders [i], "");
		//mat.CopyPropertiesFromMaterial(mats[i]);
		//if (i >= MAX_SHADERS) return;
		i_currentMaterial = i;
		//mat = mats [i];
		mat.shader = shaders[i];
	}
}
