using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
    - Utiliza-se a interface a seguir que deve ser implementada
    por todos os objetos que são reserváveis. Dessa maneira,
    a reserva pode olhar apenas para os métodos que fazem
    sentido para ela.
    
    - Interfaces são maneiras de criarmos contratos entre os objetos
    de forma que as classes "clientes" não precisarão se preocupar
    com detalhes de implementação. Ela só se preocupa com o fato
    do objeto que é passado para ela ter ou não ter assinado o cliente.
    */
public interface IReservavel
{
    void SetReserva(IReservaDeObjetos reserva);
    void AoEntrarNaReserva();
    void AoSairDaReserva();
}
