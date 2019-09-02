using UnityEngine;
using System.Collections;

public class MoviePanel : MonoBehaviour {

	public GameObject[] movies;

	// Use this for initialization
	void OnEnable()
	{
		System.Random random = new System.Random ();
		int r = random.Next (movies.Length);
		for (var i = 0; i < movies.Length; i++)
		{
			if (i == r) {
				movies[i].SetActive(true);
			} else {
				movies[i].SetActive(false);	
			}	
		}
	}

	// Update is called once per frame
	void Update()
	{
	}
}
