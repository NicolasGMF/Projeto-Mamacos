using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Player : MonoBehaviour
{
    //variaveis
    [SerializeField] private float spd = 1.5f; //velocidade de movimento
    [SerializeField] private float jumph = 25; //altura pulo
    [SerializeField] private float spdangular = 7;
    float hsp;
    float vsp;
    float shift;
    float jump;
    float angulotempo = 0;
    float angulonovo;
    float anguloanimationtime = 60;
    private Animator animator; //objeto animator
    private Rigidbody rb; //objeto rigidbody 
    private ParticleSystem ps;
    //Ray ray; //raycast pra ver se pode pular
    void Start()
    {

        animator = GetComponentInChildren<Animator>(); //pegar o objeto animator do filho
        rb = GetComponent<Rigidbody>(); //pegar o objeto rigidbody
        ps = GetComponent<ParticleSystem>();
        angulotempo = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //pegar botoes
        hsp = Input.GetAxis("Player1_Horizontal");
        vsp = Input.GetAxis("Player1_Vertical");
        shift = Input.GetAxis("Player1_Shift");
        jump = Input.GetAxis("Player1_Jump");
        //Mudar animação
        if (hsp != 0 || vsp != 0) animator.SetBool("andando", true);
        else animator.SetBool("andando", false);
        //correr
        if (shift > 0)
        {
            vsp = vsp * 2;
            hsp = hsp * 2;
            animator.SetFloat("velocidade", 2);
        }
        else
        {
            animator.SetFloat("velocidade", 1);
        }
        //controlar velocidade na diagonal
        if (hsp != 0 && vsp != 0)
        {
            hsp = hsp / 1.3f;
            vsp = vsp / 1.3f;
        }
        //angulo do personagem
        if (angulotempo > 0) angulotempo-= Time.deltaTime*100;

        if (vsp == 0 && hsp < 0) angulonovo = 0; 
        if (vsp < 0 && hsp < 0) angulonovo = 45; 
        if (vsp < 0 && hsp == 0) angulonovo = 90;
        if (vsp < 0 && hsp > 0) angulonovo = 135;
        if (vsp == 0 && hsp > 0) angulonovo = 180;
        if (vsp > 0 && hsp > 0) angulonovo = 225; 
        if (vsp > 0 && hsp == 0) angulonovo = 270;
        if (vsp > 0 && hsp < 0) angulonovo = 315; 


        //movimentação mais fluida
        if (angulotempo <= 0)
        {
            if (transform.eulerAngles.y != angulonovo)
            {
                //arrumar erro de volta 360 pra 0
                if (Mathf.Abs(transform.eulerAngles.y - angulonovo)> 180)
                { 
                    transform.eulerAngles = new Vector3(0, angulonovo / 2 + transform.eulerAngles.y / 2 + 180, 0);
                }
                else transform.eulerAngles = new Vector3(0, angulonovo / 2 + transform.eulerAngles.y / 2, 0);
                angulotempo = spdangular;
            }
            else transform.eulerAngles = new Vector3(0, angulonovo, 0);
        }
        else
        {
            if (anguloanimationtime <= 0)
            {
                if (Mathf.Abs(transform.eulerAngles.y - angulonovo) > 180)
                {
                    transform.eulerAngles = new Vector3(0, angulonovo / 2 + transform.eulerAngles.y / 2 + 180, 0);
                }
                else transform.eulerAngles = new Vector3(0, angulonovo / 2 + transform.eulerAngles.y / 2, 0);
                anguloanimationtime = spdangular;
            }
            anguloanimationtime -= Time.deltaTime*100;
            
        }
        
        //pulo


    }
    void FixedUpdate()
    {
        //calculo final de movimento
        gameObject.transform.position = new Vector3(transform.position.x + (vsp * spd * Time.deltaTime), transform.position.y, transform.position.z + (hsp * spd * Time.deltaTime));
        //calculo final de pulo
        rb.AddForce(Vector3.up * (jumph * jump * Time.deltaTime), ForceMode.Impulse);
    }
}
