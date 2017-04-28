using UnityEngine;
using System.Collections;

public class FlagResetter : MonoBehaviour {

	void Awake () {
        AllFlags.Instance.Reset();
	}
}
