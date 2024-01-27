using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Animations;

//Needed!
public class EnvironmentTrigger : MonoBehaviour
{
    public float Lifetime = 3f;

    public List<string> textlist = new List<string>();

    [SerializeField]
    private Animator floatupText;

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
        
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (Hasnotbeentrig == true && floatupText != null)
            {
                System.Random randWord = new System.Random();
                int RandomChosen = randWord.Next(textlist.Count);
                texty.text = textlist[RandomChosen];
               
                floatupText.SetTrigger("Wiggle");
                
               
                Destroy(gameObject, Lifetime);
                Hasnotbeentrig = false;
            }
        }
    }
}
