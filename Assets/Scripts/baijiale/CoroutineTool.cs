using System;
using UnityEngine;

public class CoroutineTool : MonoBehaviour
{
	static CoroutineTool _inst;

	public static CoroutineTool inst{
		get{ 
			if (_inst == null) {
				GameObject go = new GameObject ("CoroutineTool");
				_inst = go.AddComponent<CoroutineTool> ();
			}
			return _inst;
		}
	}

	void Start(){
		DontDestroyOnLoad (gameObject);
	}
}