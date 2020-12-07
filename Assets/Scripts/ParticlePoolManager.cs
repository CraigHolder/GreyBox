using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePoolManager : MonoBehaviour
{
    public GameObject obj_success;
    public int i_maxparticles = 5;

    private Queue<GameObject> q_particlepool;
    GameObject[] obj_particles;

    // Start is called before the first frame update
    void Start()
    {
        q_particlepool = new Queue<GameObject>();

        for (int i = 0; i < i_maxparticles; i++)
        {
            GameObject temp = obj_success;
            temp.SetActive(true);
            Instantiate(temp);
        }
        
        obj_particles = GameObject.FindGameObjectsWithTag("Particle");
        for (int x = 0; x < obj_particles.Length; x++)
        {
            q_particlepool.Enqueue(obj_particles[x]);
            obj_particles[x].transform.SetParent(this.gameObject.transform);
        }
    }

    public GameObject GetParticle()
    {
        GameObject obj_return = q_particlepool.Dequeue();


        if (q_particlepool.Count <= 1)
        {
            q_particlepool.Enqueue(obj_success);
        }
        else if (q_particlepool.Count >= i_maxparticles)
        {
            q_particlepool.Dequeue();
        }
        obj_return.SetActive(true);

        return obj_return;
    }

    public void ResetParticle(GameObject obj_particle)
    {

        obj_particle.SetActive(false);
        //bullet.transform.position = new Vector3(-10, 0, 0);
        q_particlepool.Enqueue(obj_particle);
    }

    // Update is called once per frame
    void Update()
    {
        for (int x = 0; x < obj_particles.Length; x++)
        {
            
            if(obj_particles[x].GetComponent<ParticleSystem>().isStopped)
            {
                ResetParticle(obj_particles[x]);
            }
        }
    }
}
