using UnityEngine;
using System.Collections;

public class MonsterU : Monster {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public override void Move()
    {
        base.Move();
    }
    public new IEnumerator Taunt()
    {
        yield return new WaitForSeconds(1);
    }
}
