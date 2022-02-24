using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spher : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Stomp2", 2f);
    }
    private void Stomp2()
    {
        GetComponent<Animator>().SetBool("stomp", true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.GetComponent<DogController>() != null)
        {
            Destroy(collision.gameObject);
        }
    }
}
