using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatfrom : MonoBehaviour
{
    public bool CanMove;

    [SerializeField]
    private float speed;

    [SerializeField]
    private int startpoint;

    [SerializeField]
    private Transform[] points;

    int i;

    bool reverse;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = points[startpoint].position;
        i = startpoint;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Vector3.Distance(transform.position, points[i].position) <= 0.01f)
        {
            CanMove = false;

            if (i == points.Length - 1)
            {
                reverse = true;
                i--;
                return;
            }
            else if(i == 0)
            {
                reverse = false;
                i++;
                return;
            }
            if (reverse)
            {
                i--;
            }
            else
            {
                i++;
            }
        }
        if (CanMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
        }
    }
}
