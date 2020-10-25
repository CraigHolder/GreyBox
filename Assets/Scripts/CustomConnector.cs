using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomConnector : MonoBehaviour
{
    public Transform obj_master;
    public Transform obj_attach;
    Collider col_attach;
    Collider col_master;

    // Start is called before the first frame update
    void Start()
    {
        col_attach = obj_attach.GetComponent<Collider>();
        col_master = obj_master.GetComponent<Collider>();
        Physics.IgnoreCollision(col_attach, col_master);
    }

    // Update is called once per frame
    void Update()
    {
        obj_attach.position.Set(obj_master.position.x, obj_master.position.y, obj_master.position.z);
    }
}
