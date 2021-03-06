﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    public GameObject arrowPrefab;
    private Animator anim;
    private Transform leftHandTrans;
    private Vector3 shootDir;
    private PlayerManager playerMng;
    private int layerMask = 1 << 9;
    private bool isShoot = false;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        leftHandTrans = transform.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 L Clavicle/Bip001 L UpperArm/Bip001 L Forearm/Bip001 L Hand");

    }
	
	// Update is called once per frame
	void Update () {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (isShoot == false)
                {
                    isShoot = true;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    bool isCollider = Physics.Raycast(ray, out hit, 1000f, layerMask);
                    if (isCollider)
                    {
                        Vector3 targetPoint = hit.point;
                        targetPoint.y = transform.position.y;
                        shootDir = targetPoint - transform.position;
                        transform.rotation = Quaternion.LookRotation(shootDir);
                        anim.SetTrigger("Attack");
                        Invoke("Shoot", 0.4f);
                    }

                }

            }
        }

	}

    public void SetPlayerMng(PlayerManager playerMng)
    {
        this.playerMng = playerMng;
    }


    private void Shoot()
    {
        playerMng.Shoot(arrowPrefab, leftHandTrans.position, Quaternion.LookRotation(shootDir));
        isShoot = false;       
    }
}
