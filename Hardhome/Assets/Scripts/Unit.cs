using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    const float minPathUpdateTime = .2f;
    const float pathUpdateMoveThreshold = .5f;
    public Transform target;
    public float speed = 3f;
    public float turnDst = 5;
    public float turnSpeed = 3;
    public float stoppingDst = 10;
    float fMovX, fMovY;

    AIPath path;
    Animator animator;

    private void Start()
    {
        StartCoroutine(UpdatePath());
        target = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    public void OnPathFound(Vector2[] waypoints, bool pathSuccessful)
    {
        if (pathSuccessful && transform != null)
        {
            path = new AIPath(waypoints, transform.position, turnDst, stoppingDst);
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator UpdatePath()
    {
        if (Time.timeSinceLevelLoad < .3f)
        {
            yield return new WaitForSeconds(.3f);
        }

        PathRequestManager goCurrentAStar = GameObject.FindGameObjectWithTag("A*").GetComponent<PathRequestManager>();
        goCurrentAStar.RequestPath(transform.position, target.position, OnPathFound);

        float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector2 targetPosOld = target.position;

        while (true)
        {
            yield return new WaitForSeconds(minPathUpdateTime);
            if (((Vector2)target.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
            {
                goCurrentAStar.RequestPath(transform.position, target.position, OnPathFound);
                targetPosOld = target.position;
            }
        }
    }

    IEnumerator FollowPath()
    {
        bool followingPath = true;
        int pathIndex = 0;

        float speedPercent = 1;
        Vector2 curMovDir = new Vector2((path.lookPoints.Length > 0 ? path.lookPoints[0].x : 0) - transform.position.x,
            (path.lookPoints.Length > 0 ? path.lookPoints[0].y : 0) - transform.position.y);

        while (followingPath &&
            path.lookPoints.Length > 0)
        {
            Vector2 pos2D = new Vector2(transform.position.x, transform.position.y);

            while (path.turnBoundaries[pathIndex].HasCrossedLine(pos2D))
            {
                if (pathIndex == path.finishLineIndex)
                {
                    followingPath = false;
                    break;
                }
                else
                {
                    pathIndex++;
                }
            }

            if (followingPath)
            {
                if (pathIndex >= path.slowDownIndex &&
                    stoppingDst > 0)
                {
                    speedPercent = Mathf.Clamp01(path.turnBoundaries[path.finishLineIndex].DistanceFromPoint(pos2D) / stoppingDst);

                    if (speedPercent < 0.01f)
                    {
                        followingPath = false;
                    }
                }

                //Quaternion targetRotation = Quaternion.LookRotation(path.lookPoints[pathIndex] - (Vector2)transform.position);
                //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
                //transform.Translate(Vector3.forward * Time.deltaTime * speed * speedPercent, Space.Self);
                Vector2 nextMovDir = new Vector2(path.lookPoints[pathIndex].x - transform.position.x, path.lookPoints[pathIndex].y - transform.position.y);
                Vector2 movDirection = Vector2.Lerp(curMovDir, nextMovDir, Time.deltaTime * turnSpeed);
                curMovDir = movDirection;
                transform.position += (Vector3)(movDirection.normalized * Time.deltaTime * speed * speedPercent); //Vector2.MoveTowards(transform.position, movDirection, Time.deltaTime * speed * speedPercent);

                if (Mathf.Abs(movDirection.x) > .5 ||
                    Mathf.Abs(movDirection.y) > .5)
                {
                    if (movDirection.x > 0) fMovX = 1.0f;
                    else if (movDirection.x < 0) fMovX = -1.0f;

                    if (movDirection.y > 0) fMovY = 1.0f;
                    else if (movDirection.y < 0) fMovY = -1.0f;

                    animator.SetFloat("movX", fMovX);
                    animator.SetFloat("movY", fMovY);
                    animator.SetBool("boolEPWalking", true);
                }
                else
                {
                    animator.SetBool("boolEPWalking", false);
                }
            }
           
            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            path.DrawWithGizmos();
        }
    }
}
