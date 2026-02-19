using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class NetPlayerController : NetworkBehaviour
{
    [Networked] public Teams Team { get; set; } = Teams.None;
    [Networked] public string NetName { get; set; }

    [Space]
    [SerializeField] private Transform root;
    [SerializeField] private NetRadiousColliderEvents circleCollider;

    [Space]
    [SerializeField] private GameObject radious;
    [SerializeField] private GameObject selectedSprite;

    [Space]
    [SerializeField] private MeshRenderer render;

    [HideInInspector] public bool enemySelected = false;
    private float nearest;
    private float nearestEnemyDistance;
    private NetPlayerController prevNearestEnemy;
    private NetPlayerController nearestEnemy;
    private Vector3 enemyDirection = Vector3.forward;

    private int selectedTweenId = 0;

    private void Awake()
    {
        selectedSprite.transform.localScale = Vector3.zero;

        radious.SetActive(false);
    }

    public void Init(Teams team, string name)
    {
        Team = team;
        NetName = name;
    }

    public override void Spawned()
    {
        switch (Team)
        {
            case Teams.TeamA:
                render.material.color = Color.orangeRed;
                break;

            case Teams.TeamB:
                render.material.color = Color.blueViolet;
                break;
        }

        if (HasStateAuthority)
            radious.SetActive(true);
    }

    private void Update()
    {
        CheckEnemies();
    }

    private void CheckEnemies()
    {
        if (circleCollider.colliders.Count > 0)
        {
            nearestEnemyDistance = 1000;

            foreach (KeyValuePair<Collider, NetPlayerController> player in circleCollider.colliders)
            {
                if (player.Value == null)
                {
                    circleCollider.colliders.Remove(player.Key);
                    continue;
                }

                nearest = Vector3.Distance(root.position, player.Value.root.position);

                if (nearest < nearestEnemyDistance)
                {
                    nearestEnemyDistance = nearest;
                    nearestEnemy = player.Value;
                    enemySelected = true;
                }
            }
        }
        else
        {
            nearestEnemy = null;
            enemySelected = false;
        }

        if (HasStateAuthority)
        {
            if (prevNearestEnemy != nearestEnemy)
            {
                if (prevNearestEnemy != null)
                    prevNearestEnemy.SetSelected(false);

                if (nearestEnemy != null)
                    nearestEnemy.SetSelected(true);
            }
            prevNearestEnemy = nearestEnemy;
        }

        if (enemySelected)
        {
            enemyDirection = nearestEnemy.root.position - root.position;
            root.rotation = Quaternion.LookRotation(enemyDirection, root.up);
        }
    }

    public void SetSelected(bool on)
    {
        LeanTween.cancel(selectedTweenId);

        if (on)
        {
            selectedTweenId = LeanTween.scale(selectedSprite, Vector3.one * 2, 0.5f).setEaseOutBack().id;
        }
        else
        {
            selectedTweenId = LeanTween.scale(selectedSprite, Vector3.zero, 0.25f).setEaseInBack().id;
        }
    }
}