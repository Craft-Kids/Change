using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move2 : MonoBehaviour
{
    Rigidbody rigid;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();   
    }

    // Update is called once per frame
    void Update()
    {
        rigid.AddForce(this.transform.localRotation * new Vector3(1,0,0) * 10f);

        if(Input.GetKey(KeyCode.RightArrow))
        {
            rigid.AddForce(Vector3.right * 10f);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rigid.AddForce(Vector3.left * 10f);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.Rotate(0, 90, 0);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.Rotate(0, -90, 0);
        }
    }
}
