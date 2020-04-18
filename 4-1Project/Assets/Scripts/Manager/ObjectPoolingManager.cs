using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    public static ObjectPoolingManager instance;

    public Queue<EnergyBall> queue_energyball = new Queue<EnergyBall>();
    public Queue<Laser> queue_randomlaser = new Queue<Laser>();

    public Laser laser;
    public EnergyBall energyBall;
    public int poolingCount = 10;

    private void Awake()
    {
        instance = this;

        for (int i = 0; i < poolingCount; i++)
        {
            EnergyBall t_object = Instantiate(energyBall, Vector2.zero, Quaternion.identity);
            queue_energyball.Enqueue(t_object);
            t_object.gameObject.SetActive(false);
        }

        Laser temp = Instantiate(laser, Vector2.zero, Quaternion.identity);
        queue_randomlaser.Enqueue(temp);
        temp.gameObject.SetActive(false);
    }

    public void InsertQueue(EnergyBall _object, Queue<EnergyBall> _queue) // Second Paramerer is put object Queue(poolingmanager queue)
    {
        _queue.Enqueue(_object);
        _object.gameObject.SetActive(false);
    }

    public void InsertQueue(Laser _object, Queue<Laser> _queue) // Second Paramerer is put object Queue(poolingmanager queue)
    {
        _queue.Enqueue(_object);
        _object.gameObject.SetActive(false);
    }

    public EnergyBall GetQueue(Queue<EnergyBall> _queue)
    {
        if (queue_energyball.Count != 0)
        {
            EnergyBall t_object = _queue.Dequeue();
            t_object.gameObject.SetActive(true);
            return t_object;
        }
        return null;
    }

    public Laser GetQueue(Queue<Laser> _queue)
    {
        if (queue_randomlaser.Count != 0)
        {
            Laser t_object = _queue.Dequeue();
            return t_object;
        }
        return null;
    }
}
