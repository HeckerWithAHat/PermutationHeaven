using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public  void Fire(GameObject projectileToFire, Quaternion firingAngle)
    {
        //projectileToFire.GetComponent<Rigidbody>().AddForce(firingAngle * Vector3.forward);
        projectileToFire.GetComponent<Rigidbody>().velocity = firingAngle * Vector3.forward * 100;

    }
}
