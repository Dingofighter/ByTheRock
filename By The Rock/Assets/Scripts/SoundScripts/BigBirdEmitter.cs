using UnityEngine;
using System.Collections;


public class BigBirdEmitter : BaseEmitter
{
    public static BigBirdEmitter instance = null;
    private string _param = "Birds";
    public Transform _player;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    protected override void Start()
    {
        _player = FindObjectOfType<PlayerController>().GetComponent<Transform>();
        base.Start();

    }

    private void Update()
    {
        _3dAttributes.position.x = _player.transform.position.x;
        _3dAttributes.position.y = _player.transform.position.y;
        _3dAttributes.position.z = _player.transform.position.z;
        _EventInstance.set3DAttributes(_3dAttributes);
    }

}


