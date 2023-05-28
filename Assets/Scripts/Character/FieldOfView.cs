using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FieldOfView : RewardingObject
{
    public float radius;
    [Range(0, 360)]
    public float angle;

    public GameObject playerRef;
    Character character;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer, stunned;
    // Start is called before the first frame update
    private void Awake()
    {
        character = gameObject.GetComponent<Character>();
    }
    void Start()
    {
        playerRef = gameObject;
        StartCoroutine(FOVRoutine());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void processRewardPerTimestep()
    {
        FieldOfViewCheck();
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            if(!stunned)
            {
                FieldOfViewCheck();
            }            
        }
    }

    public Character[] FieldOfViewCheck()
    {
        int enemyTeam;
        List<Character> enemylist = new List<Character>();
        enemyTeam = character.team == 1 ? 0 : 1;
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);
        //Vector3[] enemyPositions = new Vector3[character.enemyPositions.Length];
        //int[] enemyInSight = new int[character.enemyPositions.Length];
        foreach (Collider collider in rangeChecks)
        {
            Character colliderCharacter = collider.GetComponent<Character>();

            if (colliderCharacter != null)
            {
                Transform target = colliderCharacter.transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;
                if (colliderCharacter.team == enemyTeam)
                {
                    if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
                    {
                        float distanceToTarget = Vector3.Distance(transform.position, target.position);
                        if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                        {
                            enemylist.Add(colliderCharacter);
                        }
                    }
                }
            }
        }
        return enemylist.ToArray();
    }
}
