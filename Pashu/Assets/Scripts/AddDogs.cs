using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AddDogs : MonoBehaviour
{
    private Stats stats;
    public GameObject[] animalPrefabs;
    public Transform spawnPt;
    public Transform animalParent;
    public Animation animation;
    void Start()
    {
        stats = FindObjectOfType<Stats>();
        animation = GetComponent<Animation>();
        
        InvokeRepeating("AddDog", 10, 10);
    }
    public void AddDog()
    {
        var dogs = FindObjectsOfType<DogController>();
        if (dogs.Length <= 2)
        {
            Vector3 spawnPosition = RandomNavSphere(spawnPt.position, 2, -1);
            GameObject animal = Instantiate(animalPrefabs[Random.Range(0, animalPrefabs.Length)], spawnPosition, Quaternion.identity, animalParent);
            animation["addAnimal"].wrapMode = WrapMode.Once;
            animation.Play("addAnimal");
        }
    }
    // Update is called once per frame
    private void OnMouseDown()
    {


    }
    public Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }
}
