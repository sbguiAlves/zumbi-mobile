using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentoPersonagem : MonoBehaviour
{
    /* Só as classes filhas do MovimentoPersonagem conseguem configurar o valor
    para o atributo Direcao. E o get é público */
    public Vector3 Direcao { get; protected set; }

    private Rigidbody meuRigidbody;

    void Awake()
    {
        meuRigidbody = GetComponent<Rigidbody>();
    }

    /* Passar o Vector2 da interface gráfica do botão analógico
    para movimentar a jogadora*/
    public void SetDirecao(Vector2 direcao)
    {
        this.Direcao = new Vector3(direcao.x, 0, direcao.y);
    }

    public void SetDirecao(Vector3 direcao)
    {
        this.Direcao = direcao.normalized;
    }

    public void Movimentar(float velocidade)
    {
        /*  - A variável "direcao" é limitada por um valor que vai de -1 a 1 em X e Z.
            - Ao deixar de normalizá-la, o inimigo moverá em direções em Vector3 que são bem maiores que as bases.
            - Assim, a normalização pega um VETOR de algum tamanho e transforma em outro que sempre terá tamanho máximo 1.
            - Portanto, utiliza-se o método "direcao.normalized. */

        meuRigidbody.MovePosition
            (meuRigidbody.position + //posição pela física
            Direcao * velocidade * Time.deltaTime); //Posição Destino. Direção já está normalizada antes em "Botão Analógico"
    }

    public void Rotacionar(Vector3 direcao)
    {
        /*  - Quaterninon: calcula a rotação ao utilizar os eixos (X, Y, Z) e um eixo imaginário
            - LookRotation: passa uma posição e calcula o quanto tenho que rotacionar para olhar para esta posição

            - A direção em alguns momentos será zero, pois a jogadora está parada, disparando
            o LookRotation no Debug. Portanto, utiliza-se a condição a seguir         */
        if (direcao != Vector3.zero)
        {
            Quaternion novaRotacao = Quaternion.LookRotation(direcao);
            meuRigidbody.MoveRotation(novaRotacao);
        }
    }

    public void Morrer()
    {
        meuRigidbody.constraints = RigidbodyConstraints.None;
        meuRigidbody.velocity = Vector3.zero;
        meuRigidbody.isKinematic = false;
        GetComponent<Collider>().enabled = false;
    }

    public void Reiniciar()
    {
        meuRigidbody.isKinematic = true;
        GetComponent<Collider>().enabled = true;
    }

}
