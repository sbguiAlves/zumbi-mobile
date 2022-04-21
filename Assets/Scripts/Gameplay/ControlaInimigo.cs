using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlaInimigo : MonoBehaviour, IMatavel, IReservavel
{
    public GameObject Jogador;
    public GameObject KitMedicoPrefab;
    public GameObject ParticulaSangueZumbi;

    public AudioClip SomDeMorte;

    [HideInInspector]
    public GeradorZumbis meuGerador;

    public int TamanhoDaEsfera = 10;
    public int DanoMinimoZumbi = 20, DanoMaximoZumbi = 30;

    private Vector3 posicaoAleatoria;
    private Vector3 direcao;
    private float contadorVagar;
    private float tempoEntrePosicoesAleatorias = 4;
    private float porcentagemGerarKitMedico = 0.1f; //10%

    private ControlaInterface scriptControlaInterface;
    private AnimacaoPersonagem animacaoInimigo;
    private MovimentoPersonagem movimentaInimigo;
    private Status statusInimigo;

    private IReservaDeObjetos reserva;

    public void SetReserva(IReservaDeObjetos reserva)
    {
        this.reserva = reserva;
    }

    private void Awake()
    {
        animacaoInimigo = GetComponent<AnimacaoPersonagem>();
        movimentaInimigo = GetComponent<MovimentoPersonagem>();
    }

    private void Start()
    {
        Jogador = GameObject.FindWithTag(Tags.Jogador);
        AleatorizarZumbi();

        statusInimigo = GetComponent<Status>();
        scriptControlaInterface = GameObject.FindObjectOfType(typeof(ControlaInterface)) as ControlaInterface;
    }

    private void FixedUpdate()
    {
        scriptControlaInterface.TempoParaSobreviver();

        float distancia = Vector3.Distance(transform.position, Jogador.transform.position);

        movimentaInimigo.Rotacionar(direcao);
        animacaoInimigo.Movimentar(direcao.magnitude);

        if (distancia > statusInimigo.DistanciaParaVagar)
        {
            Vagar();
        }
        else if (distancia > statusInimigo.DistanciaParaPerseguir)
        {
            Perseguir();
            animacaoInimigo.Atacar(false);
        }
        else
        {
            direcao = Jogador.transform.position - transform.position;
            animacaoInimigo.Atacar(true);
        }
    }

    private void Perseguir()
    {
        direcao = Jogador.transform.position - transform.position;
        movimentaInimigo.SetDirecao(direcao);
        movimentaInimigo.Movimentar(statusInimigo.AleatorizarVelocidade());
    }

    private void Vagar()
    {
        contadorVagar -= Time.deltaTime;
        if (contadorVagar <= 0)
        {
            posicaoAleatoria = AleatorizarPosicao();
            contadorVagar += tempoEntrePosicoesAleatorias + Random.Range(-1f, 1f);
        }

        bool ficouPertoOSuficiente = Vector3.Distance(transform.position, posicaoAleatoria) <= 0.05;
        if (ficouPertoOSuficiente == false)
        {
            direcao = posicaoAleatoria - transform.position;
            movimentaInimigo.SetDirecao(direcao);
            movimentaInimigo.Movimentar(statusInimigo.AleatorizarVelocidade());
        }
    }

    private Vector3 AleatorizarPosicao()
    {
        Vector3 posicao = Random.insideUnitSphere * TamanhoDaEsfera;
        posicao += transform.position;
        posicao.y = transform.position.y;

        return posicao;
    }

    private void OnDrawGizmos()
    {
        Vector3 posicao = Random.insideUnitSphere * TamanhoDaEsfera;
        posicao += transform.position;
        posicao.y = transform.position.y;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, TamanhoDaEsfera);
    }

    void AtacaJogador()
    {
        int dano = Random.Range(DanoMinimoZumbi, DanoMaximoZumbi);
        Jogador.GetComponent<ControlaJogador>().TomarDano(dano);
    }

    void AleatorizarZumbi()
    {
        int geraTipoZumbi = Random.Range(1, transform.childCount);
        transform.GetChild(geraTipoZumbi).gameObject.SetActive(true);
    }

    public void TomarDano(int dano)
    {
        statusInimigo.VidaZumbi -= dano;
        if (statusInimigo.VidaZumbi <= 0)
            Morrer();
    }

    public void ParticulaSangue(Vector3 posicao, Quaternion rotacao)
    {
        Instantiate(ParticulaSangueZumbi, posicao, rotacao);
    }

    public void Morrer()
    {
        Invoke("VoltarParaReserva", 2f);
        animacaoInimigo.Morrer();
        movimentaInimigo.Morrer();
        this.enabled = false;

        ControlaAudio.instancia.PlayOneShot(SomDeMorte);
        VerificarGeracaoKitMedico(porcentagemGerarKitMedico);
        scriptControlaInterface.AtualizarQuantidadeDeZumbisMortos();
    }

    private void VerificarGeracaoKitMedico(float porcentagemGeracao)
    {
        if (Random.value <= porcentagemGeracao)
        {
            Instantiate(KitMedicoPrefab, transform.position, Quaternion.identity);
        }
    }

    private void VoltarParaReserva()
    {
        this.reserva.DevolverObjeto(this.gameObject);
    }

    /* Já que dá um trabalhão criar métodos unicos para cada uma das
        classes, é melhor criar métodos a partir da interface, evitando
        repetição de código. Assim, fica melhor para as classes que utilizam
        da classe Reserva
        
        Isso se chama de polimorfismo, onde diferentes objetos que 
        utilizam a mesma chamada de MÉTODOS (SÓ MÉTODOS), porém com comportamentos distintos,
        podem utilizar da interface que serve como contrato entre diversos
        objetos.*/

    public void AoEntrarNaReserva()
    {
        this.movimentaInimigo.Reiniciar();
        this.enabled = true;
        this.gameObject.SetActive(false);
        //statusInimigo.VidaZumbi = statusInimigo.VidaInicialZumbi; //to achando q vai dar problema dps, masvamo deixar de lado até eu melhorar do jeito que eu quero
    }

    public void AoSairDaReserva()
    {
        this.gameObject.SetActive(true);
    }
}
