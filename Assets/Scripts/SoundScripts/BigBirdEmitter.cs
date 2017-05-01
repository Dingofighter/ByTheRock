using UnityEngine;
using System.Collections;

public class BigBirdEmitter : BaseEmitter
{
    public static BigBirdEmitter instance = null;
    private string _param = "Birds";
    public enum area {HUNTING, BRUSHWOOD, GLADE, HIDEOUT, VILLAGE, FORESTPASS};
    public area areas;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    protected override void Start()
    {
        base.Start();
        ChangeArea();
    }

    public void ChangeArea()
    {
        float paramVal = 0f;

        switch (areas)
        {
            case area.HUNTING:
                paramVal = 0.8f;
                break;
            case area.BRUSHWOOD:
                paramVal = 1.6f;
                break;
            case area.GLADE:
                paramVal = 0f;
                break;
            case area.HIDEOUT:
                paramVal = 3.2f;
                break;
            case area.VILLAGE:
                paramVal = 0f;
                break;
            case area.FORESTPASS:
                paramVal = 2.4f;
                break;
        }
                _EventInstance.setParameterValue(_param, paramVal);
    }
}
	

