using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class RoleData  {

    private const string PREFABPATH = "Prefabs/";
    public RoleType RoleType { get; private set; }
    public GameObject RolePrefab { get; private set; }
    public GameObject ArrowPrefab { get; private set; }
    public Vector3 SpawnPosition { get; private set; }
    public GameObject ExplosionEffect { get; private set; }

    public RoleData(RoleType roleType,string rolePath,string arrowPath,string explosionPath,Transform spawnPos)
    {
        RoleType = roleType;
        RolePrefab = Resources.Load(PREFABPATH+rolePath) as GameObject;
        ArrowPrefab = Resources.Load(PREFABPATH+arrowPath) as GameObject;
        ExplosionEffect = Resources.Load(PREFABPATH + explosionPath) as GameObject;
        ArrowPrefab.GetComponent<Arrow>().explosionEffect = ExplosionEffect;
        SpawnPosition = spawnPos.position;
    }


}
