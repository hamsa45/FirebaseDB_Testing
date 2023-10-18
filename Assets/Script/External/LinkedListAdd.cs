using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkedListAdd : MonoBehaviour
{

    public LinkedList<int> n1 = new LinkedList<int>();
    public LinkedList<int> n2 = new LinkedList<int>();

    // Start is called before the first frame update
    void Start()
    {
        n1.AddFirst(2);
        n1.AddLast(4);
        n1.AddLast(3);
        n2.AddFirst(5);
        n2.AddLast(6);
        n2.AddLast(4);

        foreach (var item in n1)
		{
            Debug.Log(item);
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
