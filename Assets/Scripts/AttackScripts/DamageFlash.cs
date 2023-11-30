using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    public enum RenderType { MeshRenderer, SkinnedMeshRenderer}

    public RenderType type;
    [SerializeField]
    MeshRenderer meshrenderer;
    [SerializeField]
    SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField]
    SkinnedMeshRenderer skinmeshref;
    Enemy enemyref;
    [SerializeField]
    float flashTime = .15f;
    [SerializeField]
    Material flashMaterial;
    [SerializeField]
    Material originalMat;
    [SerializeField]
    Material skinmeshreforiginalMat;

    void OnEnable()
    {
        Enemy.EnemyHit += FlashEnemy;
    }

    void OnDisable()
    {
        Enemy.EnemyHit -= FlashEnemy;
    }

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
                    skinmeshref = GetComponent<SkinnedMeshRenderer>();
                }
                originalMat = skinnedMeshRenderer.material;
                skinmeshreforiginalMat = skinmeshref.material;
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
                break;
            case RenderType.SkinnedMeshRenderer:
                skinnedMeshRenderer.material = flashMaterial;
                skinmeshref.material = flashMaterial;
                Invoke("flashStop", flashTime);
                break;
        }
        
    }

    void flashStop()
    {
        switch (type)
        {
            case RenderType.MeshRenderer:
                meshrenderer.material = originalMat;
                break;
            case RenderType.SkinnedMeshRenderer:
                skinnedMeshRenderer.material = originalMat;
                skinmeshref.material = skinmeshreforiginalMat;
                break;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
