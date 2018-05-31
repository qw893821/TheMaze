using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoWMask : MonoBehaviour {
    Vector3 offSet;
    public Material mat;
    
    private Vector3 _pos1, _pos2, _pos3, _pos4;
    public Vector3 pos1
    {
        get {return _pos1; }
        set { _pos1=value; } }
    public Vector3 pos2
    {
        get { return _pos2; }
        set { _pos2 = value; } }
    public Vector3 pos3
    {
        get { return _pos3; }
        set { _pos3 = value; } }
    public Vector3 pos4
    {
        get { return _pos4; }
        set { _pos4 = value; } }

    //, pos2, pos3, pos4;
    // Use this for initialization
    void Start () {
        offSet = transform.position- GameManager.gm.player.transform.position ;
        //mat = this.GetComponent<Renderer>().material;
        mat.shader = Shader.Find("QuadDeformationShader");
        _pos1 = new Vector3(0,0,0);
        _pos2 = new Vector3(0, 0, 0);
        _pos3 = new Vector3(0, 0, 0);
        _pos4 = new Vector3(0, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
        MaskFollow();
    }
    private void FixedUpdate()
    {
        UpdateMeshPos();
    }

    void MaskFollow()
    {
        transform.position = GameManager.gm.player.transform.position + offSet;
    }

    void UpdateMeshPos()
    {
        mat.SetVector("_Position1", _pos1);
        mat.SetVector("_Position2", _pos2);
        mat.SetVector("_Position3", _pos3);
        mat.SetVector("_Position4", _pos4);
    }
}
