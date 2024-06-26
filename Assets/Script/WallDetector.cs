using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetector : MonoBehaviour
{
    [Header("Wall Detection")]
    [SerializeField] private Transform[] _wallDetectionPoints;
    [SerializeField] private float _detectionLength = 0.05f;
    [SerializeField] private LayerMask _wallLayerMask;

    public static float orientDetection {get; private set;}  


    public bool DetectWallNearBy(){
        foreach (Transform wallDetectionPoint in _wallDetectionPoints){

            RaycastHit2D hitRight = Physics2D.Raycast(
                wallDetectionPoint.position, //origine
                Vector2.right,            //direction
                _detectionLength,
                _wallLayerMask
            );

            RaycastHit2D hitLeft = Physics2D.Raycast(
                wallDetectionPoint.position, // origine
                Vector2.left,                // direction 
                _detectionLength,
                _wallLayerMask
            );

            if(hitRight.collider != null || hitLeft.collider != null){
                Debug.Log("Contact de WallDetector");
                if(hitRight.collider != null){
                    orientDetection=1;
                }else{
                    orientDetection=-1;
                }
                return true;
            }
        }
        return false;
    }
}
