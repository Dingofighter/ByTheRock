using UnityEngine;
using System.Collections;

public class BrushwoodManager : MonoBehaviour
{

    public bool BPM100 = true, BPM90 = true, BPM80 = true, BPM70 = true, BPM60 = true, GLADE = true;

    public void setBool(int _trigger = -1, bool value = false)
    {
        switch (_trigger)
        {
            default:
                break;
            case 0:
                BPM100 = value;
                break;
            case 1:
                BPM90 = value;
                break;
            case 2:
                BPM80 = value;
                break;
            case 3:
                BPM70 = value;
                break;
            case 4:
                BPM60 = value;
                break;
            case 5:
                GLADE = value;
                break;
        }
    }

    public bool getBool(int _trigger = 0)
    {
        switch (_trigger)
        {
            default:
                Debug.LogError("Forgot to assign what bool to get");
                return false;
            case 0:
                return BPM100;
            case 1:
                return BPM90;
            case 2:
                return BPM80;
            case 3:
                return BPM70;
            case 4:
                return BPM60;
            case 5:
                return GLADE;
        }
    }  
}
