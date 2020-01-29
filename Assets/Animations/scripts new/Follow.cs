using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour
{
    public Transform target;
	public float smooth;
    Vector3 pos;

	void Start()
	{
		pos = transform.position - target.transform.position;
	}

    void Update(){
//		pos = new Vector3(target.position.x,  target.position.y +4, target.position.z -6);
        //transform.rotation = target.rotation;
        transform.position = Vector3.Lerp(
			transform.position, pos,
        Time.deltaTime * smooth);
    }
}