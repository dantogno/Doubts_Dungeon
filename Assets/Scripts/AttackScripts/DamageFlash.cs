using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    MeshRenderer meshrenderer;
    [SerializeField]
    float flashTime = .15f;
    [SerializeField]
    Material flashMaterial;
    Material originalMat;
    // Start is called before the first frame update
    void Start()
    {
        meshrenderer = GetComponent<MeshRenderer>();
        if(meshrenderer == null)
        {
            meshrenderer = GetComponentInChildren<MeshRenderer>();
        }
        originalMat = meshrenderer.material;
    }

    public void Flash()
    {
        meshrenderer.material = flashMaterial;
        Invoke("flashStop", flashTime);
    }

    void flashStop()
    {
        meshrenderer.material = originalMat;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
