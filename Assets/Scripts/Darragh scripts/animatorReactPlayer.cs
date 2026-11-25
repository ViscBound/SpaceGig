using UnityEngine;

public class animatorReactPlayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public void AttackEnd()
    {
        gameObject.GetComponentInParent<AxeAttack>().isAttacking=false;
        
            
    }

}
