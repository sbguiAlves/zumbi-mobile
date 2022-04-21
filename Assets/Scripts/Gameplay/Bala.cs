using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour, IReservavel
{
    public float Velocidade = 20;
    public int DanoDaBala = 1;

    private Rigidbody rigidbodyBala;
    public AudioClip SomDeMorte;

    private IReservaDeObjetos reserva;

    private void Start()
    {
        rigidbodyBala = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rigidbodyBala.MovePosition(rigidbodyBala.position + transform.forward * Velocidade * Time.deltaTime);
    }


    /*  - Quando um Trigger bate em um objeto, o método a seguir é chamado, retornando ao qual objeto colidiu.*/
    private void OnTriggerEnter(Collider objetoDeColisao)
    {
        Quaternion rotacaoOpostaABala = Quaternion.LookRotation(-transform.forward);

        switch (objetoDeColisao.tag)
        {
            case Tags.Inimigo:
                ControlaInimigo inimigo = objetoDeColisao.GetComponent<ControlaInimigo>(); //melhorar isso, repetição de código
                inimigo.TomarDano(DanoDaBala);
                inimigo.ParticulaSangue(transform.position, rotacaoOpostaABala);
                break;

            case Tags.ChefeDeFase:
                ControlaChefe chefe = objetoDeColisao.GetComponent<ControlaChefe>();
                chefe.TomarDano(DanoDaBala);
                chefe.ParticulaSangue(transform.position, rotacaoOpostaABala);
                break;
        }
        Invoke("VoltarParaReserva", 5f);

    }

    private void VoltarParaReserva()
    {
        this.reserva.DevolverObjeto(this.gameObject);
    }

    /* Indica para qual reserva os objetos fizeram o contrato*/
    public void SetReserva(IReservaDeObjetos reserva)
    {
        this.reserva = reserva;
    }

    public void AoEntrarNaReserva()
    {
        this.gameObject.SetActive(false);
    }

    public void AoSairDaReserva()
    {
        this.gameObject.SetActive(true);
    }
}
