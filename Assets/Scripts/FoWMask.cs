using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoWMask : MonoBehaviour {
    Vector3 offSet;
    //public Material mat;

    //public Vector3 posFL, posFR, posBR, posBL;
    private Vector3 _pos1, _pos2, _pos3, _pos4;
    /*public Vector3 posFL
    {
        get {return _pos1; }
        set { _pos1=value; } }
    public Vector3 posFR
    {
        get { return _pos2; }
        set { _pos2 = value; } }
    public Vector3 posBR
    {
        get { return _pos3; }
        set { _pos3 = value; } }
    public Vector3 posBL
    {
        get { return _pos4; }
        set { _pos4 = value; }
    }
    */

    //, pos2, pos3, pos4;
    // Use this for initialization
    void Awake () {
        offSet = transform.position- GameManager.gm.player.transform.position ;
        //mat = this.GetComponent<Renderer>().material;
       // mat.shader = Shader.Find("QuadDeformationShader");
        //posFL = new Vector3(0,0,0);
        //posFR = new Vector3(0, 0, 0);
        //posBR = new Vector3(0, 0, 0);
        //posBL = new Vector3(0, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
        MaskFollow();
    }
    private void FixedUpdate()
    {
       // UpdateMeshPos();
    }

    void MaskFollow()
    {
        transform.position = GameManager.gm.player.transform.position + offSet;
    }

    /*void UpdateMeshPos()
    {
        mat.SetVector("_Position1", posFL);
        mat.SetVector("_Position2", posFR);
        mat.SetVector("_Position3", posBR);
        mat.SetVector("_Position4", posBL);
    }*/
}
