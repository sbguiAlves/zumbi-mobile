using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeradorChefe : MonoBehaviour
{
    public ReservaFixa reservaDeChefes;

    private ControlaInterface scriptControlaInterface;
    private float tempoParaProximaGeracao = 0;

    public float tempoEntreGeracoes = 60;
    public AudioClip AlertaDeChefao;
    public Transform[] PosicoesPossiveisDeSpawn;

    private Transform jogador;

    private void Start()
    {
        tempoParaProximaGeracao = tempoEntreGeracoes;
        scriptControlaInterface = GameObject.FindObjectOfType(typeof(ControlaInterface)) as ControlaInterface;
        jogador = GameObject.FindWithTag(Tags.Jogador).transform;
    }

    private void Update()
    {
        if (Time.timeSinceLevelLoad > tempoParaProximaGeracao)
        {
            if (this.reservaDeChefes.TemObjeto())
            {
                ControlaAudio.instancia.PlayOneShot(AlertaDeChefao);

                Vector3 posicaoDeCriacao = CalcularPosicaoMaisDistanteDoJogador();
                var chefe = this.reservaDeChefes.PegarObjeto();
                var controleChefe = chefe.GetComponent<ControlaChefe>();
                controleChefe.SetPosicao(posicaoDeCriacao); //mover o chefe de forma que ele sabe onde tá o jogador

                scriptControlaInterface.AparecerTextoChefeCriado();
                tempoParaProximaGeracao = Time.timeSinceLevelLoad + tempoEntreGeracoes;
            }
        }
    }

    private Vector3 CalcularPosicaoMaisDistanteDoJogador()
    {
        Vector3 posicaoDeMaiorDistancia = Vector3.zero;
        float maiorDistancia = 0;
        //para cada um dos elementos, faça isso apenas uma vez no frame atual
        foreach (Transform posicao in PosicoesPossiveisDeSpawn)
        {
            float distanciaEntreJogador = Vector3.Distance(posicao.position, jogador.position);
            if (distanciaEntreJogador > maiorDistancia)
            {
                maiorDistancia = distanciaEntreJogador;
                posicaoDeMaiorDistancia = posicao.position;
            }

        }
        return posicaoDeMaiorDistancia;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        int numeroDeSpawns = PosicoesPossiveisDeSpawn.Length;

        for (int i = 0; i < numeroDeSpawns; i++)
        {
            Gizmos.DrawWireSphere(PosicoesPossiveisDeSpawn[i].position, 1);
        }
    }
}
