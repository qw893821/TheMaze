using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager:MonoBehaviour {
    private static TeamManager _instance;
    public static TeamManager tm
    {
        get { return _instance; }
    }
	public List<GameObject> team;

    public void AddMember(GameObject go)
    {
        
        if (GOCheck(go))
        {
            team.Add(go);
        }
    }

    public bool GOCheck(GameObject go)
    {
        if (go.tag=="Teammember")
        {
            return false;
        }
        else { return true; }
    }

    public void RemoveMember(GameObject go)
    {
        if (team.Contains(go))
        {
            team.Remove(go);
        }
    }

    private void Start()
    {
        if (!_instance)
        {
            _instance = this;
        }
        else { Destroy(this); }
    }

    private void Update()
    {
        
    }
}
