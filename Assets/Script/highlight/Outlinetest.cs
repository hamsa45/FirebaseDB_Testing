using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Outlinetest : MonoBehaviour
{
	//public Shader outlineShader;
	//public Color outlineColor = Color.green;
	//public float outlineWidth = 0.002f;

	//private Material outlineMaterial;

	//void Start()
	//{
	//    outlineMaterial = new Material(outlineShader);
	//    outlineMaterial.SetColor("_OutlineColor", outlineColor);
	//    outlineMaterial.SetFloat("_Outline", outlineWidth);

	//    Renderer renderer = GetComponent<Renderer>();
	//    renderer.material = outlineMaterial;
	//}

	public Renderer Renderer { get; private set; }
	public SpriteRenderer SpriteRenderer { get; private set; }
	public SkinnedMeshRenderer SkinnedMeshRenderer { get; private set; }
	public MeshFilter MeshFilter { get; private set; }

	public int color;
	public bool eraseRenderer;

	private void Awake()
	{
		Renderer = GetComponent<Renderer>();
		SkinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
		SpriteRenderer = GetComponent<SpriteRenderer>();
		MeshFilter = GetComponent<MeshFilter>();
	}

	void OnEnable()
	{
		camOutlineEffect.Instance?.AddOutline(this);
	}

	void OnDisable()
	{
		camOutlineEffect.Instance?.RemoveOutline(this);
	}

	private bool visible;
	private void OnBecameVisible()
	{
		visible = true;
	}
	private void OnBecameInvisible()
	{
		visible = false;
	}
	public bool IsVisible => visible;

	private Material[] _SharedMaterials;
	public Material[] SharedMaterials
	{
		get
		{
			if (_SharedMaterials == null)
				_SharedMaterials = Renderer.sharedMaterials;

			return _SharedMaterials;
		}
	}
}
