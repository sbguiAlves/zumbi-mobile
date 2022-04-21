using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ControlaChefe : MonoBehaviour, IMatavel, IReservavel
{
    private Transform jogador;
    private NavMeshAgent agente;
    private Status statusChefe;
    private AnimacaoPersonagem animacaoChefe;
    private MovimentoPersonagem movimentoChefe;

    public GameObject KitMedicoPrefab;
    public GameObject ParticulaSangueZumbi;
    public Slider SliderVidaChefe;
    public AudioClip SomDeMorte;

    public Image ImagemSlider;
    public Color CorDaVidaMaxima, CorDaVidaMinima;

    public int DanoMinimoChefe = 30, DanoMaximoChefe = 40;

    private IReservaDeObjetos reserva;

    private void Awake()
    {
        animacaoChefe = GetComponent<AnimacaoPersonagem>();
        movimentoChefe = GetComponent<MovimentoPersonagem>();
        agente = GetComponent<NavMeshAgent>();
        statusChefe = GetComponent<Status>();
    }

    private void Start()
    {
        jogador = GameObject.FindWithTag(Tags.Jogador).transform;
        
        agente.speed = statusChefe.VelocidadeMaxZumbi;
        SliderVidaChefe.maxValue = statusChefe.VidaChefe;
        AtualizarInterface();
    }

    public void SetPosicao(Vector3 posicao)
    {
        this.transform.position = posicao;
        this.agente.Warp(posicao); //teletransportar
    }

    private void Update()
    {
        agente.SetDestination(jogador.position);
        animacaoChefe.Movimentar(agente.velocity.magnitude);

        if (agente.hasPath == true)
        {
            bool estouPertoDoJogador = agente.remainingDistance <= agente.stoppingDistance;

            if (estouPertoDoJogador)
            {
                animacaoChefe.Atacar(true);
                Vector3 direcao = jogador.position - transform.position;
                movimentoChefe.Rotacionar(direcao);
            }
            else
            {
                animacaoChefe.Atacar(false);
            }
        }
    }

    private void AtacaJogador()
    {
        int dano = Random.Range(DanoMinimoChefe, DanoMaximoChefe);
        jogador.GetComponent<ControlaJogador>().TomarDano(dano);
    }

    public void TomarDano(int dano)
    {
        statusChefe.VidaChefe -= dano;
        AtualizarInterface();

        if (statusChefe.VidaChefe <= 0)
        {
            Morrer();
        }
    }

    public void ParticulaSangue(Vector3 posicao, Quaternion rotacao)
    {
        Instantiate(ParticulaSangueZumbi, posicao, rotacao);
    }

    public void Morrer()
    {
        animacaoChefe.Morrer();
        movimentoChefe.Morrer();
        this.enabled = false; //script
        agente.enabled = false;
        ControlaAudio.instancia.PlayOneShot(SomDeMorte);

        Instantiate(KitMedicoPrefab, transform.position, Quaternion.identity);
        Invoke("VoltarParaReserva", 2f);
    }

    private void VoltarParaReserva()
    {
        this.reserva.DevolverObjeto(this.gameObject);
    }

    void AtualizarInterface()
    {
        SliderVidaChefe.value = statusChefe.VidaChefe;
        float porcentagemDaVida = (float)statusChefe.VidaChefe / statusChefe.VidaInicialChefe;
        Color corDaVida = Color.Lerp(CorDaVidaMinima, CorDaVidaMaxima, porcentagemDaVida);
        ImagemSlider.color = corDaVida;
    }

    public void SetReserva(IReservaDeObjetos reserva)
    {
        this.reserva = reserva;
    }

    public void AoEntrarNaReserva()
    {
        this.gameObject.SetActive(false);
        this.movimentoChefe.Reiniciar();
        this.enabled = true;
        agente.enabled = true;
        statusChefe.VidaChefe = statusChefe.VidaInicialChefe;
    }

    public void AoSairDaReserva()
    {
        this.gameObject.SetActive(true);
    }
}
