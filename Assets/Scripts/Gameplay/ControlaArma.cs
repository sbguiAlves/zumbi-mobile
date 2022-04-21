using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControlaArma : MonoBehaviour
{
    [SerializeField]
    private ReservaExtensivel reservaDeBalas;

    public GameObject Bala;
    public GameObject CanoDaArma;
    public AudioClip SomDoTiro;

    public ControlaInterface scriptControlaInterface;

    private void Start()
    {
        scriptControlaInterface = GameObject.FindObjectOfType(typeof(ControlaInterface)) as ControlaInterface;
    }

    private void Update()
    {

        if (Input.GetButtonDown("Fire1") && !EventSystem.current.IsPointerOverGameObject())
        {
            this.CriarBala();
            ControlaAudio.instancia.PlayOneShot(SomDoTiro);
        }

    }

    private void CriarBala()
    {
        if (this.reservaDeBalas.TemObjeto())
        {
            GameObject bala = this.reservaDeBalas.PegarObjeto();
            bala.transform.position = CanoDaArma.transform.position;
            bala.transform.rotation = CanoDaArma.transform.rotation;
        }
    }
}
