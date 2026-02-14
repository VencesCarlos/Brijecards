using UnityEngine;

public class CharacterController : MonoBehaviour
{

    [SerializeField] Animator anim;
    public bool isPlayer;

    public void Atacar()
    {
        anim.SetTrigger("Attack");
    }

    public void Morir()
    {
        anim.SetBool("Death", true);
    }
}
