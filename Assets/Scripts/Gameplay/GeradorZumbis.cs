using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeradorZumbis : MonoBehaviour
{
    [SerializeField]
    private ReservaFixa reserva;
    public LayerMask LayerZumbi;
    public float TempoGerarZumbi = 1;
    public Transform[] PosicoesSpawnParaZumbis;

    private GameObject jogador;
    private float distanciaDeSpawn = 1f; /*Tamanho da área circular de ataque do zumbi*/
    private float contadorTempo = 0;
    private float distanciaDoJogadorParaSpawn = 10f;
    private float contadorAumentarDificuldade, tempoProximoAumentoDeDificuldade = 20; //em segundos

    private void Start()
    {
        jogador = GameObject.FindWithTag(Tags.Jogador);
        contadorAumentarDificuldade = tempoProximoAumentoDeDificuldade;
    }

    private void Update()
    {
        //Debug.LogFormat("Máximo de zumbis: {0}\nVivos: {1}", quantidadeMaximaZumbisVivos, quantidadeDeZumbisVivos);
        bool possoGerarZumbisPelaDistancia = Vector3.Distance(transform.position, jogador.transform.position) > 
            distanciaDoJogadorParaSpawn;

        if (possoGerarZumbisPelaDistancia)
        {
            contadorTempo += Time.deltaTime;

            if (contadorTempo >= TempoGerarZumbi)
            {
                StartCoroutine(GerarNovoZumbi());
                contadorTempo = 0;
            }
        }

        if (Time.timeSinceLevelLoad > contadorAumentarDificuldade)
        {
            contadorAumentarDificuldade = Time.timeSinceLevelLoad +
                tempoProximoAumentoDeDificuldade;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        int numeroDeSpawns = PosicoesSpawnParaZumbis.Length;

        for (int i = 0; i < numeroDeSpawns; i++)
        {
            Gizmos.DrawWireSphere(PosicoesSpawnParaZumbis[i].position, distanciaDeSpawn);
        }
    }

    private IEnumerator GerarNovoZumbi()
    {
        for (int i = 0; i < PosicoesSpawnParaZumbis.Length; i++)
        {
            Vector3 posicaoDeCriacao = AleatorizarPosicao();
            Transform gerador = PosicoesSpawnParaZumbis[i];
            Collider[] colisores = Physics.OverlapSphere(posicaoDeCriacao, 1, LayerZumbi);

            while (colisores.Length > 0)
            {
                posicaoDeCriacao = AleatorizarPosicao();
                colisores = Physics.OverlapSphere(posicaoDeCriacao, 1, LayerZumbi);
                yield return null;
            }
            //ControlaInimigo zumbi = Instantiate(Zumbi, gerador.position + posicaoDeCriacao, Quaternion.identity).GetComponent<ControlaInimigo>();
            if (this.reserva.TemObjeto())
            {
                GameObject zumbi = this.reserva.PegarObjeto();
                zumbi.transform.position = gerador.position + posicaoDeCriacao;

                var controleZumbi = zumbi.GetComponent<ControlaInimigo>();
                controleZumbi.meuGerador = this;
            }
        }
    }

    private Vector3 AleatorizarPosicao()
    {
        Vector3 posicao = Random.insideUnitSphere * distanciaDeSpawn;
        posicao += transform.position;
        posicao.y = 0;

        return posicao;
    }
}
