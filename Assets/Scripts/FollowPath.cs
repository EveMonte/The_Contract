using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    public enum MovementType
    {
        Moving,
        Lerping
    }
    [SerializeField]
    private MovementType Type = MovementType.Moving;
    [SerializeField]
    private PathMotion FirstPath;
    [SerializeField]
    private PathMotion SecondPath;
    [SerializeField]
    private float speed = 40;
    [SerializeField]
    private float maxDistance = .1f;
    [SerializeField]
    private GameObject _platformToBreak;

    private IEnumerator<Transform> pointInPath;

    void Update()
    {
        if(pointInPath == null || pointInPath.Current == null)
        {
            return;
        }

        if(Type == MovementType.Moving)
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

    public void DestroyOD()
    {
        Destroy(gameObject.GetComponent<FollowPath>());
        Destroy(gameObject);
    }

    public void SwitchOffOD()
    {
        pointInPath = null;
        if(SecondPath == null)
        {
            DestroyOD();
        }
    }

    public void SwitchOnOD()
    {
        if (FirstPath == null)
        {
            return;
        }

        pointInPath = FirstPath.GetNextPathPoint();

        pointInPath.MoveNext();

        if (pointInPath.Current == null)
        {
            return;
        }

        transform.position = pointInPath.Current.position;
    }

    public void SwitchOnAfterPuzzle()
    {
        FirstPath = SecondPath;
        SecondPath = null;
        SwitchOnOD();
        _platformToBreak.SendMessage("Ready");
        _platformToBreak.SendMessage("Break", 15);
    }
}
