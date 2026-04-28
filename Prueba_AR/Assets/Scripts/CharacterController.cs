using UnityEngine;

public class CharacterController : MonoBehaviour
{

    [SerializeField] Animator anim;
    public bool isPlayer;

    [SerializeField] AudioClip[] sonidos;
    //  0   Idle
    //  1   Attack
    //  2   Die
    AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = sonidos[0];
        audioSource.Play();
    }

    public void Atacar()
    {
        anim.SetTrigger("Attack");
        //audioSource.clip = sonidos[1];
        audioSource.PlayOneShot(sonidos[1]);
    }

    public void Morir()
    {
        anim.SetBool("Death", true);
        audioSource.PlayOneShot(sonidos[2]);
    }
}
