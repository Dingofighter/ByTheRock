using UnityEngine;
using System.Collections;

public class ScatterManager : MonoBehaviour
{

    public int chance = 60;
    public int _timer = 0;
    public int rand;
    private int previous, now;
    public GameObject[] _Scatters = new GameObject[0];
    public Transform _player;
    public GameObject active;


    // Use this for initialization
    void Start ()
    {
        _player = FindObjectOfType<PlayerController>().GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (active == null)
        {
            _timer++;

            if (_timer >= 30)
            {
                rand = Random.Range(0, chance);
               if (rand == 1)
                {
                    now = Random.Range(0, _Scatters.Length - 1);

                    while (now == previous)
                    {
                        now = Random.Range(0, _Scatters.Length - 1);
                    }       

                    var _scatter = Instantiate(_Scatters[now]);
                    _scatter.transform.parent = gameObject.transform;
                    active = _scatter;
                    previous = now;
                }
                _timer = 0;
            }
        }
    }

    public Vector3 PlayerPosition()
    {
        return _player.transform.position;
    }
}
