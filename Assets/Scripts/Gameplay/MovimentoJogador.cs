using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*HERANCA: MovimentoJogador(filho) herda funções de MovimentoPersonagem(pai), assim como os métodos de MonoBehavior*/
public class MovimentoJogador : MovimentoPersonagem
{
    [SerializeField]
    private AudioSource audio;

    //Eventos para a animação com o som de passos, sincronizando corretamente
    public void AudioPasso()
    {
        audio.Play();
    }

    public void RotacaoJogador()
    {
        Vector3 posicaoMiraJogador = this.Direcao;
        posicaoMiraJogador.y = transform.position.y;
        Rotacionar(posicaoMiraJogador);
    }
}