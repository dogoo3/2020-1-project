using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser_Ray : MonoBehaviour
{
    public Laser _laser;
    public Transform Pos;
    public bool _hit2D;

    // Start is called before the first frame update
    void Awake()
    {
        _laser = transform.parent.gameObject.GetComponent<Laser>();

        if(this.transform.rotation.y<0)
        {
            Pos.transform.position = new Vector3(this.transform.position.x - _laser._range, 0, 0);
        }
        else
        {
            Pos.transform.position = new Vector3(this.transform.position.x + _laser._range, 0, 0);
        }
      
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(transform.position, Pos.position, new Color(1, 0, 0));
        
        if (Physics2D.Linecast(this.transform.position, Pos.position, _laser.layer))
            GameManager.instance._player.Attacked(_laser.LaserDamage);
    }
}
