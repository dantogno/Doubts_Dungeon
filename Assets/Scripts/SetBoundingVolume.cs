using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SetBoundingVolume : MonoBehaviour
{
    public GameObject boundingVolumeObject;

    void Start()
    {
        // Get the Cinemachine Confiner component attached to the Cinemachine Virtual Camera
        CinemachineConfiner confiner = GetComponentInChildren<CinemachineVirtualCamera>().GetComponent<CinemachineConfiner>();

        // Set the Bounding Volume to the combined volume of all colliders
        if (confiner != null && boundingVolumeObject != null)
        {
            // Combine the colliders into a mesh collider
            Collider combinedCollider = CreateMeshCollider(boundingVolumeObject.GetComponentsInChildren<Collider>());

            // Set the bounding volume as the combined collider
            confiner.m_BoundingVolume = combinedCollider;

            // Invalidate the cache
            confiner.InvalidatePathCache();
        }
    }

    Collider CreateMeshCollider(Collider[] colliders)
    {
        // Combine the meshes of all colliders
        Mesh combinedMesh = CalculateCombinedMesh(colliders);

        // Create a new empty GameObject to hold the combined collider
        GameObject combinedObject = new GameObject("CombinedMeshCollider");

        // Add a MeshCollider to the combined object with the combined mesh
        MeshCollider meshCollider = combinedObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = combinedMesh;

        // Return the combined collider
        return meshCollider;
    }

    Mesh CalculateCombinedMesh(Collider[] colliders)
    {
        // Initialize a new Mesh
        Mesh combinedMesh = new Mesh();

        // Combine the meshes of all colliders
        CombineInstance[] combineInstances = new CombineInstance[colliders.Length];
        for (int i = 0; i < colliders.Length; i++)
        {
            // Set up CombineInstance
            combineInstances[i].mesh = GetMeshFromCollider(colliders[i]);
            combineInstances[i].transform = colliders[i].transform.localToWorldMatrix;
        }

        // Combine the meshes into one mesh
        combinedMesh.CombineMeshes(combineInstances, true, true);

        return combinedMesh;
    }

    Mesh GetMeshFromCollider(Collider collider)
    {
        MeshFilter meshFilter = collider.gameObject.GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            return meshFilter.sharedMesh;
        }
        else
        {
            Debug.LogWarning("MeshFilter component not found on collider GameObject: " + collider.gameObject.name);
            return null;
        }
    }

    #region Make combined mesh
    //void Start()
    //{
    //    // Get the Cinemachine Confiner component attached to the Cinemachine Virtual Camera
    //    CinemachineConfiner confiner = GetComponentInChildren<CinemachineVirtualCamera>().GetComponent<CinemachineConfiner>();

    //    // Set the Bounding Volume to the combined volume of all colliders
    //    if (confiner != null && boundingVolumeObject != null)
    //    {
    //        // Combine the colliders into a mesh collider
    //        Collider combinedCollider = CreateMeshCollider(boundingVolumeObject.GetComponentsInChildren<Collider>());

    //        // Set the bounding volume as the combined collider
    //        confiner.m_BoundingVolume = combinedCollider;

    //        // Invalidate the cache
    //        confiner.InvalidatePathCache();
    //    }
    //}

    //Collider CreateMeshCollider(Collider[] colliders)
    //{
    //    // Combine the meshes of all colliders
    //    Mesh combinedMesh = CalculateCombinedMesh(colliders);

    //    // Create a new empty GameObject to hold the combined collider
    //    GameObject combinedObject = new GameObject("CombinedMeshCollider");

    //    // Add a MeshCollider to the combined object with the combined mesh
    //    MeshCollider meshCollider = combinedObject.AddComponent<MeshCollider>();
    //    meshCollider.sharedMesh = combinedMesh;

    //    // Return the combined collider
    //    return meshCollider;
    //}

    //Mesh CalculateCombinedMesh(Collider[] colliders)
    //{
    //    // Initialize a new Mesh
    //    Mesh combinedMesh = new Mesh();

    //    // Combine the meshes of all colliders
    //    CombineInstance[] combineInstances = new CombineInstance[colliders.Length];
    //    for (int i = 0; i < colliders.Length; i++)
    //    {
    //        // Create a new GameObject to hold the collider's mesh
    //        GameObject tempObject = new GameObject("TempColliderMesh");
    //        tempObject.transform.SetParent(boundingVolumeObject.transform); // Fixed the variable name here
    //        tempObject.transform.position = colliders[i].transform.position;
    //        tempObject.transform.rotation = colliders[i].transform.rotation;

    //        // Copy the collider's mesh to the temporary GameObject
    //        MeshFilter meshFilter = tempObject.AddComponent<MeshFilter>();
    //        meshFilter.sharedMesh = colliders[i].gameObject.GetComponent<MeshFilter>().sharedMesh;

    //        // Set up CombineInstance
    //        combineInstances[i].mesh = meshFilter.sharedMesh;
    //        combineInstances[i].transform = tempObject.transform.localToWorldMatrix;

    //        // Destroy the temporary GameObject
    //        Destroy(tempObject);
    //    }

    //    // Combine the meshes into one mesh
    //    combinedMesh.CombineMeshes(combineInstances, true, true);

    //    return combinedMesh;
    //}
    #endregion

    #region Old

    //void Start()
    //{
    //    // Get the Cinemachine Confiner component attached to the Cinemachine Virtual Camera
    //    CinemachineConfiner confiner = GetComponentInChildren<CinemachineVirtualCamera>().GetComponent<CinemachineConfiner>();

    //    // Set the Bounding Volume to the combined volume of all colliders
    //    if (confiner != null && boundingVolumeObject != null)
    //    {
    //        // Calculate the combined mesh of all colliders
    //        Mesh combinedMesh = CalculateCombinedMesh(boundingVolumeObject.GetComponentsInChildren<Collider>());

    //        // Create an empty GameObject to represent the combined volume
    //        GameObject combinedVolumeObject = new GameObject("CombinedVolume");
    //        combinedVolumeObject.transform.position = combinedMesh.bounds.center;

    //        // Add a MeshCollider to the combined volume GameObject with the combined mesh
    //        MeshCollider combinedCollider = combinedVolumeObject.AddComponent<MeshCollider>();
    //        combinedCollider.sharedMesh = combinedMesh;

    //        // Assign the combined volume GameObject's collider to the Bounding Volume
    //        confiner.m_BoundingVolume = combinedCollider;

    //        // Invalidate the cache
    //        confiner.InvalidatePathCache();
    //    }
    //}

    //Mesh CalculateCombinedMesh(Collider[] colliders)
    //{
    //    // Create a new Mesh
    //    Mesh combinedMesh = new Mesh();

    //    // Combine the meshes of all colliders
    //    CombineInstance[] combineInstances = new CombineInstance[colliders.Length];
    //    for (int i = 0; i < colliders.Length; i++)
    //    {
    //        combineInstances[i].mesh = colliders[i].gameObject.GetComponent<MeshFilter>().sharedMesh;
    //        combineInstances[i].transform = colliders[i].gameObject.transform.localToWorldMatrix;
    //    }
    //    combinedMesh.CombineMeshes(combineInstances, true, true);

    //    return combinedMesh;
    //}
    #endregion
}
