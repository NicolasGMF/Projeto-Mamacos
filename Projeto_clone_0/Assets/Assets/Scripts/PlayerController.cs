using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{

    private float spd = 7f; //velocidade de movimento
    private float spdangular = 7;
    private float hsp;
    private float use;
    private float vsp;
    private float shift;
    private bool segurando = false;
    private int timesegurar = 0;
    private int idletime = 0;
    private int itemperto = 0;
    private Collider itemspec;
    float angulotempo = 0;
    float angulonovo;
    float anguloanimationtime = 60;
    private Animator animator; //objeto animator
    private Rigidbody rb; //objeto rigidbody
    public GameObject hand;


    public ParticleSystem ps;

    public PhotonView myPhotonView;
    public Rigidbody rbody;

    public float speed = 2;
    public float myHue = 0;

    public GameObject bazuka;
    public Transform bulletSpawnPoint;

    float inputRotation;
    float inputSpeed;

    //--------------------------------------------------------
    void Start()
    {
        ps.Stop();
        animator = GetComponentInChildren<Animator>(); //pegar o objeto animator do filho
        rb = GetComponent<Rigidbody>(); //pegar o objeto rigidbody
        angulotempo = 0;

        transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);

        
        if (myPhotonView.Owner == null)
        {
            Destroy(gameObject);
        }
    }

    //--------------------------------------------------------
    void Update()
    {
        if (myPhotonView.IsMine)
        {
            
            //pegar botoes movimento
            hsp = Input.GetAxis("Player1_Horizontal");
            vsp = Input.GetAxis("Player1_Vertical");
            use = Input.GetAxis("Player1_Use");
            shift = Input.GetAxis("Player1_Shift");


            //segurar objetos
            if (timesegurar >0) timesegurar--;
            Debug.Log(timesegurar);
            if (use > 0)
            {
                if(itemperto==1)
                {
                    if(timesegurar<=0)
                    {
                        if (!segurando)
                        {
                            //segurar
                            itemspec.gameObject.transform.position = hand.transform.position;
                            //itemspec.isTrigger = true;
                            var kinechan = itemspec.gameObject.GetComponent<Rigidbody>();
                            kinechan.isKinematic = true;
                            itemspec.gameObject.transform.parent = hand.transform;
                            segurando = true;
                            timesegurar = 160;

                        }
                    }
                }
                if(segurando)
                {
                    if (timesegurar <= 0)
                    {
                         segurando = false;

                         itemspec.gameObject.transform.parent = null;
                         var kinechan = itemspec.gameObject.GetComponent<Rigidbody>();
                         kinechan.isKinematic = false;
                         timesegurar = 160;
                        //soltar
                    }
                }
            }



            //correr
            if (shift > 0)
            {
                if(segurando)
                {
                    shift = 0;
                }
                else
                {
                    vsp = vsp * 2;
                    hsp = hsp * 2;
                }
                //animator.SetFloat("velocidade", 2);
            }
            else
            {
                //animator.SetFloat("velocidade", 1);
            }
            //controlar velocidade na diagonal
            if (hsp != 0 && vsp != 0)
            {
                hsp = hsp / 1.3f;
                vsp = vsp / 1.3f;
            }
            //angulo do personagem
            if (angulotempo > 0) angulotempo -= Time.deltaTime * 100;

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
                    if (Mathf.Abs(transform.eulerAngles.y - angulonovo) > 180)
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
                anguloanimationtime -= Time.deltaTime * 100;

            }


            inputRotation = Input.GetAxis("Horizontal");
            inputSpeed = Input.GetAxis("Vertical");

            if (Input.GetKeyDown(KeyCode.C))
            {
                myHue = Random.Range(0.0f, 1.0f);
                SendMyColor();
            }



           


        }
    }
    //colisao items
    void OnTriggerEnter(Collider other)
    {
        if(itemperto==0)
        {
            if (other.gameObject.CompareTag("Item"))
            {
                itemperto = 1;
                itemspec = other;
                var nameScript = other.gameObject.GetComponent<Outline>();
                nameScript.ChangeWidth(2f);

                // if (other.contacts[0].otherCollider.transform.gameObject.name == "HeadShot")
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(itemperto==1)
        {
            if (other.gameObject.CompareTag("Item"))
            {
                if(other = itemspec)
                {
                    itemperto = 0;
                    var nameScript = other.gameObject.GetComponent<Outline>();
                    nameScript.ChangeWidth(0f);
                }

                // if (other.contacts[0].otherCollider.transform.gameObject.name == "HeadShot")
            }
        }
    }

    //--------------------------------------------------------
    public void SendMyColor()
    {
        myPhotonView.RPC(nameof(ChangeColor), RpcTarget.All, myHue);
    }
    
    [PunRPC]
    void ChangeColor(float hue, PhotonMessageInfo info)
    {
        myHue = hue;
        Color newColor = Color.HSVToRGB(myHue, 1, 1);
        GetComponent<MeshRenderer>().material.color = newColor;
    }

    //--------------------------------------------------------
    public void SendChatMessage(string message)
    { 
        myPhotonView.RPC(nameof(ChatMessageReceived), RpcTarget.All, message);
    }

    [PunRPC]
    void ChatMessageReceived(string message, PhotonMessageInfo info)
    {
        FindObjectOfType<ChatOnline>().AddChatText(message, info.Sender.NickName);
    }

    

    //--------------------------------------------------------
    private void FixedUpdate()
    {
        if (myPhotonView.IsMine)
        {
            // Quaternion rot = rbody.rotation * Quaternion.Euler(0, inputRotation * Time.deltaTime * 60, 0);
            //rbody.MoveRotation(rot);

            if ((vsp != 0) || (hsp != 0))
            {
                if(idletime>3000)
                {
                    animator.SetBool("idleani1", false);
                }
                idletime = 0;
                animator.SetBool("andando", true);
                if (segurando)
                {
                    animator.SetBool("segurando", true);
                }
                else animator.SetBool("segurando", false);
                if (shift > 0)
                {
                    animator.SetBool("correndo", true);
                    if (!ps.isPlaying)
                    {
                        ps.Play();
                    }
                }
                else
                {
                    animator.SetBool("correndo", false);
                    ps.Stop();
                }
            }
            else 
            {
                idletime++;
                if(idletime>3000)
                {
                    animator.SetBool("idleani1", true);
                    idletime = 4000;
                }
                animator.SetBool("andando", false);
            }



            gameObject.transform.position = new Vector3(transform.position.x + (vsp * spd * Time.deltaTime ), transform.position.y, transform.position.z + (hsp * spd * Time.deltaTime));
            //Vector3 force = rot * Vector3.forward * inputSpeed * 1000 * Time.deltaTime;
            //rbody.AddForce(force);


            if (rbody.velocity.magnitude > 5)
            {
                rbody.velocity = rbody.velocity.normalized * 2;
            }
        }
    }
}