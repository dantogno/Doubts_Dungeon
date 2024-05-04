using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    public enum RenderType { MeshRenderer, SkinnedMeshRenderer}

    public RenderType type;

    MeshRenderer meshrenderer;
    SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField]
    
    Enemy enemyref;
    [SerializeField]
    float flashTime = .15f;
    [SerializeField]
    Material flashMaterial;
    Material originalMat;
    public GameObject hitMarker;


    void FlashEnemy()
    {
        Flash();
       
    }
    // Start is called before the first frame update
    void Start()
    {
        switch (type)
        {
            case RenderType.MeshRenderer:
                meshrenderer = GetComponent<MeshRenderer>();
                if (meshrenderer == null)
                {
                    meshrenderer = GetComponentInChildren<MeshRenderer>();
                }
                originalMat = meshrenderer.material;
                break;
            case RenderType.SkinnedMeshRenderer:
                skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
                if (skinnedMeshRenderer== null)
                {
                    skinnedMeshRenderer= GetComponentInChildren<SkinnedMeshRenderer>();
                }
                originalMat = skinnedMeshRenderer.material;
                break;
        }
    }

    public void Flash()
    {
        switch (type)
        {
            case RenderType.MeshRenderer:
                meshrenderer.material = flashMaterial;
                Invoke("flashStop", flashTime);
                Instantiate(hitMarker, this.transform);
                break;
            case RenderType.SkinnedMeshRenderer:
                skinnedMeshRenderer.material = flashMaterial;
                Invoke("flashStop", flashTime);
                Instantiate(hitMarker, this.transform);
                break;
        }
        
    }

    void flashStop()
    {
        switch (type)
        {
            case RenderType.MeshRenderer:
                meshrenderer.material = originalMat;
                //hitMarker.SetActive(false);
                break;
            case RenderType.SkinnedMeshRenderer:
                skinnedMeshRenderer.material = originalMat;
                //hitMarker.SetActive(false);
                break;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
