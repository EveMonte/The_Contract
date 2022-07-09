using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    public enum MovementType
    {
        Moveing,
        Lerping
    }

    public MovementType Type = MovementType.Moveing;
    public MovementPass MyPath;
    public float speed = 40;
    public float maxDistance = .1f;

    private IEnumerator<Transform> pointInPath;

    // Start is called before the first frame update
    void Start()
    {
        if(MyPath == null)
        {
            return;
        }

        pointInPath = MyPath.GetNextPathPoint();

        pointInPath.MoveNext();

        if(pointInPath.Current == null)
        {
            return;
        }

        transform.position = pointInPath.Current.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(pointInPath == null || pointInPath.Current == null)
        {
            return;
        }

        if(Type == MovementType.Moveing)
        {
            transform.position = Vector3.MoveTowards(transform.position, pointInPath.Current.position, Time.deltaTime * speed);
        }
        else if(Type == MovementType.Lerping)
        {
            transform.position = Vector3.Lerp(transform.position, pointInPath.Current.position, Time.deltaTime * speed);
        }

        var distanceSqure = (transform.position - pointInPath.Current.position).sqrMagnitude;
        if(distanceSqure < maxDistance * maxDistance)
        {
            pointInPath.MoveNext();
        }
    }
}
