using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentoPersonagem : MonoBehaviour
{
    private Rigidbody meuRigidbody;
    private Vector3 direcao;

    void Awake()
    {
        meuRigidbody = GetComponent<Rigidbody>();
    }

    /* Passar o Vector2 da interface gráfica do botão analógico
    para movimentar a jogadora*/
    public void SetDirecao(Vector2 direcao)
    {
        this.direcao = new Vector3(direcao.x, 0, direcao.y);
    }

    public void SetDirecao(Vector3 direcao)
    {
        this.direcao = direcao;
    }

    public void Movimentar(float velocidade)
    {
        /*  - A variável "direcao" é limitada por um valor que vai de -1 a 1 em X e Z.
            - Ao deixar de normalizá-la, o inimigo moverá em direções em Vector3 que são bem maiores que as bases.
            - Assim, a normalização pega um VETOR de algum tamanho e transforma em outro que sempre terá tamanho máximo 1.
            - Portanto, utiliza-se o método "direcao.normalized. */

        meuRigidbody.MovePosition
            (meuRigidbody.position + //posição pela física
            direcao.normalized * velocidade * Time.deltaTime); //Posição Destino 
    }

    public void Rotacionar(Vector3 direcao)
    {
        /*  - Quaterninon: calcula a rotação ao utilizar os eixos (X, Y, Z) e um eixo imaginário
            - LookRotation: passa uma posição e calcula o quanto tenho que rotacionar para olhar para esta posição */

        Quaternion novaRotacao = Quaternion.LookRotation(direcao);
        meuRigidbody.MoveRotation(novaRotacao);
    }

    public void Morrer()
    {
        //eu prefiro fazer um pop no inimigo quando ele tiver no chão. Aqui ele tá sumindo pra baixo
        //ou ele cai e dps de um tempo ele disolve pra baixo, muitas possibilidades hmmm
        meuRigidbody.constraints = RigidbodyConstraints.None;
        meuRigidbody.velocity = Vector3.zero;
        meuRigidbody.isKinematic = false; //pra gravidade funcionar
        GetComponent<Collider>().enabled = false;
    }

}
