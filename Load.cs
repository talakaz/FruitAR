using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class Load : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (delayload());

	}
	IEnumerator delayload(){
		yield return new WaitForSeconds (2f);
		SceneManager.LoadSceneAsync (1);

	}

}
