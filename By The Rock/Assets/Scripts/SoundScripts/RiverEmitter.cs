using UnityEngine;
using System.Collections;

public class RiverEmitter : BaseEmitter
{

    public float _Value;
    public float checkx, checkz;

    public GameObject Ball;
    public GameObject[] _Points = new GameObject[0];
    public Vector3[] _Pos; //x & z
    public float _Length;
    private Transform _Player;
    private Vector3[] _EndPoints;
    public float _MinDist = 0, _MaxDist = 0;
    public float _TotalLength;
    public Vector3 bluePoint;

    public float temp = 0;
    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        _TotalLength = 0;
        _Player = FindObjectOfType<PlayerController>().GetComponent<Transform>();
        _Pos = new Vector3[_Points.Length];

        for (int i = 0; i < _Points.Length; i++)
        {
            _Pos[i] = _Points[i].transform.position;
        }

        _EndPoints = new Vector3[2];
        _EndPoints[0] = _Pos[0];
        _EndPoints[1] = _Pos[_Points.Length - 1];
        bluePoint = _Pos[0];

        _Length = Vector3.Distance(_Pos[0], _Pos[_Pos.Length - 1]);

        for (int i = 0; i < _Points.Length - 1; i++)
        {
            _TotalLength += Vector3.Distance(_Pos[i], _Pos[i + 1]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < _Points.Length - 1; i++)
        {
            Debug.DrawLine(_Points[i].transform.position, _Points[i + 1].transform.position, Color.blue);
        }

        float angle = Vector3.Angle(_EndPoints[0], _EndPoints[1]);
        Vector3 pos = new Vector3(_Player.transform.position.x - _EndPoints[0].x, _Player.transform.position.y - _EndPoints[0].y, _Player.transform.position.z - _EndPoints[0].z);
        pos = Quaternion.Euler(0, -angle, 0) * pos;
        _Length = Vector3.Distance(_EndPoints[0], _EndPoints[1]);

        _Value = Mathf.Max(0, Mathf.Min(1, -pos.z / _Length)) * 100;


        float prevPercentage = 0;

        for (int i = 0; i <= _Points.Length; i++)
        {
            if (i == _Points.Length)
            {
                bluePoint = _Pos[_Points.Length - 1];
                break;
            }

            Vector3 point = new Vector3(_Pos[i].x - _Pos[0].x, _Pos[i].y - _Pos[0].y, _Pos[i].z - _Pos[0].z);
            point = Quaternion.Euler(0, -angle, 0) * point;
            float percentage = -point.z / _Length * 100;

            if (percentage > _Value)
            {
                if (i == 0)
                    break;

                float innerPercentage = percentage - prevPercentage;
                float innerValue = _Value - percentage;
                float ratio = 1 + (innerValue / innerPercentage);

                //Debug.Log("i: " + i + " percent: " + ratio);

                bluePoint.x = _Pos[i - 1].x * (1 - ratio) + _Pos[i].x * (ratio);
                bluePoint.y = _Pos[i - 1].y * (1 - ratio) + _Pos[i].y * (ratio);
                bluePoint.z = _Pos[i - 1].z * (1 - ratio) + _Pos[i].z * (ratio);
                break;
            }
            prevPercentage = percentage;
        }
        Ball.transform.position = bluePoint;
        _3dAttributes.position.x = bluePoint.x;
        _3dAttributes.position.y = bluePoint.y;
        _3dAttributes.position.z = bluePoint.z;
        _EventInstance.set3DAttributes(_3dAttributes);
        _EventInstance.setParameterValue("RiverIntensity", 1.16f);
    }

    void OnDrawGizmos()
    {
        for (int i = 0; i < _Points.Length - 1; i++)
        {
            Debug.DrawLine(_Points[i].transform.position, _Points[i + 1].transform.position, Color.blue);
        }
    }
}