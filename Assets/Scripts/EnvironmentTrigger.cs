using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnvironmentTrigger : MonoBehaviour
{
    public float Lifetime = 6f;

    public List<string> textlist = new List<string>();
    
    [SerializeField]
    private GameObject FloatingText;
    private bool Hasnotbeentrig;
    public Vector3 Offset = new Vector3(0, 2, 0);
    private TextMesh texty;
    [SerializeField]
    private MeshRenderer TextsMesh;
    // Start is called before the first frame update
    void Start()
    {
        TextsMesh.GetComponent<MeshRenderer>();
        Hasnotbeentrig = true;
        texty = FloatingText.GetComponent<TextMesh>();
        TextsMesh.enabled = false;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (Hasnotbeentrig == true)
            {
                System.Random randWord = new System.Random();
                int RandomChosen = randWord.Next(textlist.Count);
                texty.text = textlist[RandomChosen];
                transform.localPosition += Offset;
                TextsMesh.enabled = true;
                Instantiate(FloatingText, transform.position, Quaternion.identity, transform);
                Destroy(gameObject, Lifetime);
                Hasnotbeentrig = false;
            }
        }
    }
}
