using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCaster : MonoBehaviour
{
    public GameObject caster;

    private void Update()
    {
        if(caster != null)
        {
            transform.position = caster.transform.position;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
