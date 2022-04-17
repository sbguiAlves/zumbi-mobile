using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/*  Event System - Reconhecer a bolinha com um evento de arrastar (IDragHandler)
        - Na UI (Canvas), desmarcar "Raycast Target" para elementos 
        que não é necessário que o jogador interage, economizando tempo
        de processamento e melhorando a performance do jogo.

    Pivot x Ancoras: enquanto o pivot nos fornece uma maneira de achar o ponto
    zero dentro de um elemento da interface, as ancoras nos ajudam a posicionar
    esse objeto em relação ao pai dele.
        - As ancoras fixam um ponto de referência de acordo com o objeto-pai.
    */
public class BotaoAnalogico : MonoBehaviour, IDragHandler
{
    [SerializeField]
    private RectTransform imagemDeFundo;
    [SerializeField]
    private RectTransform imagemBolinha;

    [SerializeField]
    private MeuEventoDinamicoVector2 QuandoMudarOValor;

    public void OnDrag(PointerEventData dadosDoMouse)
    {
        var posicaoMouse = CalcularPosicaoMouse(dadosDoMouse);
        /*Calculando a posição limitada da posicao do mouse*/
        var posicaoLimitada = this.LimitarPosicao(posicaoMouse);
        this.PosicionarJoystick(posicaoLimitada);

        this.QuandoMudarOValor.Invoke(posicaoLimitada); // Valores entre 0 e 1
    }

    private Vector2 LimitarPosicao(Vector2 posicaoMouse)
    {
        //Vetor normalizado
        var posicaoLimitada = posicaoMouse / this.TamanhoDaImagem();

        if (posicaoLimitada.magnitude > 1) //Fora do Limite
        {
            posicaoLimitada = posicaoLimitada.normalized; //Posicao máxima
        }

        return posicaoLimitada;
    }

    private float TamanhoDaImagem()
    {
        return this.imagemDeFundo.rect.width / 2; //centro como referência
    }

    private void PosicionarJoystick(Vector2 posicaoMouse)
    {
        /*  Posição local de acordo com o elemento-pai
            em vez de pegar a posição global de acordo com o canvas
            Pivot - lembrar da palavra "metade da imagem" 
        */
        this.imagemBolinha.localPosition = posicaoMouse * this.TamanhoDaImagem();
    }

    private Vector2 CalcularPosicaoMouse(PointerEventData dadosDoMouse)
    {
        Vector2 posicao;

        /* A classe auxiliar abaixo é um utilitário do Component Rect Transform e
        esta possui o método ScreenPointToLocalPointInRetacle.
            - Este método pega um ponto na tela e o transforma para um ponto local para o retângulo. Desta
            forma, será possível a localização do mouse dentro do quadrado */
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            imagemDeFundo,
            dadosDoMouse.position, //onde o mouse tá em relação a tela
            dadosDoMouse.enterEventCamera,
            out posicao
        );

        return posicao;
    }
}

/* Quando mudar o valor ao interagir com a interface de botão analógico
    - Usado para a Unity reconhecer como evento*/
[Serializable]
public class MeuEventoDinamicoVector2 : UnityEvent<Vector2>
{

}