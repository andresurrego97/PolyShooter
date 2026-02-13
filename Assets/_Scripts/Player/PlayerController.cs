using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : /*MonoBehaviour*/ NetworkBehaviour
{
    //public bool HasStateAuthority;

    public Transform root;
    [SerializeField] private RadiousColliderEvents circleCollider;

    [Space]
    [SerializeField] private GameObject selectedSprite;

    private float nearest;
    private float nearestEnemyDistance;
    private PlayerController prevNearestEnemy;
    private PlayerController nearestEnemy;
    private Vector3 enemyDirection;

    private int selectedTweenId = 0;

    private void Awake()
    {
        selectedSprite.transform.localScale = Vector3.zero;

        //if (!HasStateAuthority)
        //    return;

        //circleCollider.OnEnterPlayer += EnterPlayer;
        //circleCollider.OnExitPlayer += ExitPlayer;
    }

    //private void OnDestroy()
    //{
    //    if (!HasStateAuthority)
    //        return;

    //    circleCollider.OnEnterPlayer -= EnterPlayer;
    //    circleCollider.OnExitPlayer -= ExitPlayer;
    //}

    //private void EnterPlayer(PlayerController enemy)
    //{

    //}

    //private void ExitPlayer(PlayerController enemy)
    //{

    //}

    private void Update()
    {
        if (!HasStateAuthority)
            return;

        CheckEnemies();
    }

    private void CheckEnemies()
    {
        if (circleCollider.colliders.Count > 0)
        {
            nearestEnemyDistance = 1000;

            foreach (KeyValuePair<Collider, PlayerController> player in circleCollider.colliders)
            {
                nearest = Vector3.Distance(root.position, player.Value.root.position);

                if (nearest < nearestEnemyDistance)
                {
                    nearestEnemyDistance = nearest;
                    nearestEnemy = player.Value;
                }
            }
        }
        else
        {
            nearestEnemy = null;
        }

        if (prevNearestEnemy != nearestEnemy)
        {
            if (prevNearestEnemy != null)
                prevNearestEnemy.SetSelected(false);

            if (nearestEnemy != null)
                nearestEnemy.SetSelected(true);
        }
        prevNearestEnemy = nearestEnemy;

        if (nearestEnemy != null)
        {
            enemyDirection = nearestEnemy.root.position - root.position;
        }
        root.forward = enemyDirection;
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