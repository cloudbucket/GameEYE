﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlJugador : MonoBehaviour{
    [SerializeField] public float velocidadcaida;
    [SerializeField] public float velocidadmovi;
    [SerializeField] public float fuerzasalto;
    [SerializeField] public bool terrenofirme;
    [SerializeField] public bool colgado;
    private Rigidbody2D rbd;
    private Rigidbody2D plataformamovil;
    private Animator ani;
    private bool disparadorsalto;
    private int vida = 250;
    private int vidaActual;
    public bool vivo = true;
    private double count = 100;

    void Start(){
        rbd = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        vidaActual = vida;
    }

    void Update(){
        ani.SetFloat("Velocidad", Mathf.Abs(rbd.velocity.x));
        ani.SetFloat("Caida", Mathf.Abs(rbd.velocity.y));
        ani.SetBool("Terrenofirme", terrenofirme);
        ani.SetBool("Colgado", colgado);
        ani.SetBool("Vivo", vivo);
        if (Input.GetKeyDown(KeyCode.UpArrow) && terrenofirme){
            disparadorsalto = true;
        }
    }

    void FixedUpdate(){
        Debug.Log(rbd.velocity.x);
        float p = Input.GetAxis("Horizontal");
        if (vidaActual <= 0){
            vivo = false;
            count -= 1.2;
        }
        if (vivo){
            if (p > 0f){
                transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                rbd.velocity = new Vector2(velocidadmovi, rbd.velocity.y);
            }
            else if (p < 0f){
                transform.localScale = new Vector3(-0.7f, 0.7f, 0.7f);
                rbd.velocity = new Vector2(-velocidadmovi, rbd.velocity.y);
            }
            else{
                rbd.velocity = new Vector2(0, rbd.velocity.y);
            }
            if (disparadorsalto){
                rbd.AddForce(Vector2.up * fuerzasalto, ForceMode2D.Impulse);
                disparadorsalto = false;
            }   
        }
        if (count < 0){
            SceneManager.LoadScene("Menu");
        }
    }

    void OnCollisionExit2D(Collision2D col){
        if (col.gameObject.tag == "Terreno" || col.gameObject.tag == "MovilActual")
        {
            terrenofirme = false;
        }
        if (col.gameObject.tag == "Muro"){
            colgado = false;
            terrenofirme = false;
        }
    }

    void OnCollisionEnter2D(Collision2D col){
        if (col.gameObject.tag == "Terreno" || col.gameObject.tag == "Movil")
        {
            terrenofirme = true;
            colgado = false;
        }
        if (col.gameObject.tag == "Muro"){
            terrenofirme = true;
            colgado = true;
        }
        if (col.gameObject.tag == "Finish"){
            SceneManager.LoadScene("Menu");
        }
    }

    void OnCollisionStay2D(Collision2D col){
        float p = Input.GetAxis("Horizontal");
        if (col.gameObject.tag == "Muro" ){
            rbd.AddForce(Vector2.down * velocidadcaida, ForceMode2D.Impulse);
            terrenofirme = true;
        }
        if (col.gameObject.tag == "Terreno")
        {
            terrenofirme=true;
        }
        if (col.gameObject.tag == "MovilActual" && p == 0f)
        {
            terrenofirme = true;
            plataformamovil = GameObject.FindGameObjectWithTag("MovilActual").GetComponent<Rigidbody2D>();
            rbd.velocity = new Vector2(plataformamovil.velocity.x, rbd.velocity.y);
            Debug.Log(plataformamovil.velocity.x);

        }
    }

    void OnTriggerStay2D(Collider2D col){
        if (col.gameObject.tag == "Espina" && vidaActual>0){
            vidaActual -= 2;
        }
    }

    void OnTriggerEnter2D(Collider2D col){
        if (col.gameObject.tag == "Bala"){
            vidaActual -= 45;
        }
    }

}