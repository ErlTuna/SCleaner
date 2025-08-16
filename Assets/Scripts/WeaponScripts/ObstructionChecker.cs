using UnityEngine;

public static class ObstructionChecker
{
    public static bool CheckMuzzleEnvironmentOverlap(Transform muzzleTipCheck, LayerMask environmentLayers, float radius = 0.5f){
        Collider2D overlap = Physics2D.OverlapCircle(muzzleTipCheck.transform.position, radius, environmentLayers);
        if(overlap){
            //Debug.Log("Muzzle is blocked by environment " + overlap);
            return true;
        }
        else {
            //Debug.Log("Muzzle is not blocked by environment");
            return false;
        }
    }

    public static GameObject CheckMuzzleEnemyOverlap(Transform muzzleTipCheck, LayerMask enemyLayer, float radius = 0.5f){
        Collider2D overlap = Physics2D.OverlapCircle(muzzleTipCheck.transform.position, radius, enemyLayer);
        if(overlap){
            //Debug.Log("Muzzle is blocked by enemy " + overlap.gameObject);
            return overlap.gameObject;
        }
        else {
            //Debug.Log("Muzzle is not blocked by enemy");
            return null;
        }
    }

    public static bool CheckWeaponObstructionOverlap(Transform raycastStartPoint, Transform raycastEndPoint, LayerMask[] layers){
        //mixedLayerMask = obstructionLayers |= 1 << enemyLayer;
        //print(mixedLayerMask);
        LayerMask combinedMask = 0; 
        
        foreach (LayerMask mask in layers)
        {
            combinedMask |= mask; // Use the bitwise OR to combine the masks
        }
        RaycastHit2D hit = Physics2D.Linecast(raycastStartPoint.position, raycastEndPoint.position, combinedMask);
        if(hit){
            //Debug.Log("Weapon obstructed by something.");
            return true;
        }
        return false;
    }

    public static bool CheckWeaponObstructionOverlap(Transform raycastStartPoint, Transform raycastEndPoint, LayerMask firstLayerMask, LayerMask secondLayerMask){
        //mixedLayerMask = obstructionLayers |= 1 << enemyLayer;
        //print(mixedLayerMask);
        LayerMask combinedMask = 0; 
        combinedMask |= firstLayerMask;
        combinedMask |= secondLayerMask;

        RaycastHit2D hit = Physics2D.Linecast(raycastStartPoint.position, raycastEndPoint.position, combinedMask);
        if(hit){
            return true;
        }
        return false;
    }


    public static GameObject CheckMuzzleOverlapWithLine(Transform raycastStartPoint, Transform raycastEndPoint, LayerMask firstLayerMask, LayerMask secondLayerMask){
        //mixedLayerMask = obstructionLayers |= 1 << enemyLayer;
        //print(mixedLayerMask);
        LayerMask combinedMask = 0; 
        combinedMask |= firstLayerMask;
        combinedMask |= secondLayerMask;

        RaycastHit2D hit = Physics2D.Linecast(raycastStartPoint.position, raycastEndPoint.position, combinedMask);
        if(hit){
            return hit.collider.gameObject;
        }
        return null;
    }
}
