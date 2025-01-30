using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CPlayerController : MonoBehaviour
{
    Rigidbody rb;
    public float force = 10.0f;           // Normalna prędkość ruchu
    public float jumpForce = 2.0f;        // Siła skoku

    public float sprintMultiplier = 1.1f; // Mnożnik prędkości przy sprincie
    public float crouchMultiplier = 0.5f; // Mnożnik prędkości przy kucaniu
    public float crawlMultiplier = 0.3f;  // Mnożnik prędkości przy czołganiu
    public float dashForce = 3.0f;       // Siła dasha
    public float dashDuration = 0.2f;     // Czas trwania dasha
    public float doubleTapTime = 0.3f;    // Maksymalny czas między dwoma naciśnięciami W dla dasha

    private bool isGrounded = true;       // Czy postać dotyka podłoża
    private bool isCrouching = false;     // Czy postać jest w pozycji kucającej
    private bool isCrawling = false;      // Czy postać jest w pozycji czołgającej się
    private int jumpCount = 0;            // Licznik skoków dla double jump

    private float lastWPressTime = -1f;   // Czas ostatniego naciśnięcia W
    private Vector3 originalScale;        // Oryginalna skala postaci
    public Vector3 crouchScale = new Vector3(1, 0.5f, 1);  // Skala przy kucaniu
    public Vector3 crawlScale = new Vector3(1, 0.3f, 1);   // Skala przy czołganiu

    public GameObject ItemBomb;    // Prefab bomby, który gracz będzie mógł położyć
    public Item ItemBombPickup;
    public float bombDropHeight = 1.0f; // Wysokość na jaką bomba będzie umieszczona nad ziemią

    private GameObject currentBomb;  // Zmienna przechowująca obiekt bomby, jeśli został już położony

    public Animator ani; // Animator postaci

    public PlayerStats playerStats; // Referencja do statystyk gracza
    public Camera mainCamera; // Kamera gracza

    private Vector3 moveDirection = Vector3.zero; // Kierunek ruchu

    public float attackRange = 1.5f; // Zasięg ataku miecza
    public int attackDamage = 20; // Obrażenia zadawane przez miecz
    public LayerMask enemyLayer; // Warstwa przeciwników


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalScale = transform.localScale; // Zapisz oryginalną skalę postaci
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Pobierz główną kamerę, jeśli nie została przypisana
        }
    }
    private void FixedUpdate()
    {
        // Ustawienie aktualnej prędkości na podstawie sprintu, kucania i czołgania
        float currentForce = force;

        if (Input.GetButton("Sprint") && !isCrouching && !isCrawling)
        {
            ani.SetBool("Run", true);
            currentForce *= sprintMultiplier;
        }
        else
        {
            ani.SetBool("Run", false);
        }

        if (Input.GetButtonDown("Crouch"))
        {
            isCrouching = true;
            isCrawling = false;
            currentForce *= crouchMultiplier;
            transform.localScale = crouchScale;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            isCrouching = false;
            if (!isCrawling)
            {
                transform.localScale = originalScale;
            }
        }
        if (Input.GetKeyDown(KeyCode.W) && Input.GetAxisRaw("Vertical") > 0) // Tylko jeśli wciśnięto W i gracz porusza się do przodu
        {
            if (Time.time - lastWPressTime < doubleTapTime)  // Sprawdzenie, czy czas między naciśnięciami "W" jest mniejszy niż dozwolony czas
            {
                ani.SetTrigger("Roll");  // Aktywacja animacji roll (przewrotu)
                StartCoroutine(Dash(rb));  // Uruchomienie funkcji Dash
            }
            lastWPressTime = Time.time;  // Zaktualizowanie czasu ostatniego naciśnięcia
        }


        if (Input.GetButtonDown("Crawl"))
        {
            isCrawling = !isCrawling;
            isCrouching = false;
            currentForce *= crawlMultiplier;

            transform.localScale = isCrawling ? crawlScale : originalScale;
        }

        // Pobieranie wejść od użytkownika
        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");

        // Obliczanie kierunku kamery
        Vector3 forward = mainCamera.transform.forward;
        forward.y = 0; // Wykluczamy zmiany w pionie
        forward.Normalize();

        Vector3 right = mainCamera.transform.right;
        right.y = 0; // Wykluczamy zmiany w pionie
        right.Normalize();

        // Tworzymy wektor ruchu w zależności od wejść gracza
        moveDirection = forward * vertical + right * horizontal;

        // Sprawdzenie, czy gracz się porusza, i czy upłynęła odpowiednia ilość czasu
        if (moveDirection.magnitude > 0.1f && Time.time - lastWPressTime > 0.3f)  // moveDirection.magnitude sprawdza, czy ruch jest wystarczająco duży (czy gracz się porusza) oraz sprawdza, czy od ostatniego naciśnięcia "W" minęła co najmniej sekunda
        {
            ani.SetBool("Move", true);  // Jeśli gracz się porusza, ustaw animację ruchu
        }
        else
        {
            ani.SetBool("Move", false);  // Jeśli gracz nie porusza się, ustaw animację bez ruchu
        }
        // Dodajemy siłę do rigidbody w odpowiednim kierunku
        rb.AddForce(moveDirection.normalized * currentForce);

    }

    void Update()
    {
        float currentForce = force;

        if (playerStats.dead == false)
        {
            ani.SetBool("Dead", false);

            if (Input.GetButton("Use"))
            {
                Inventory playerInventory = GetComponent<Inventory>();
                if (playerInventory.GetItemCount(ItemBombPickup) != 0)
                {
                    DropBomb();
                    playerInventory.RemoveItem(ItemBombPickup);
                }
            }
        }
        
        else
        {
            ani.SetBool("Dead", true);
        }

        if (Input.GetButtonDown("Jump") && (isGrounded || jumpCount < 2))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            ani.SetTrigger(jumpCount == 0 ? "Jump" : "DoubleJump");
            jumpCount++;
        }

        if (Input.GetButtonDown("Fire1")) // "Fire1" to domyślna akcja dla lewego przycisku myszy
        {
            ani.SetTrigger("Attack"); // Odtwórz animację ataku
            // Sprawdź, czy w zasięgu ataku znajduje się jakiś przeciwnik
            Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);
            foreach (Collider enemy in hitEnemies)
            {
                PerformAttack(enemy);
            }
        }

    }
    private void PerformAttack(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Dragon enemyDragon = other.GetComponent<Dragon>();
            if (enemyDragon != null)
            {
                enemyDragon.TakeDamage(attackDamage); // Zadaj obrażenia przeciwnikowi
            }
        }
    }

    private IEnumerator Dash(Rigidbody body)
    {
        // Pobierz kierunek, w którym gracz patrzy
        Vector3 forward = mainCamera.transform.forward;
        forward.y = 0; // Wykluczamy zmiany w pionie
        forward.Normalize();

        Vector3 right = mainCamera.transform.right;
        right.y = 0; // Wykluczamy zmiany w pionie
        right.Normalize();

        // Kierunek dashowania
        Vector3 dashDirection = forward * Input.GetAxisRaw("Vertical") + right * Input.GetAxisRaw("Horizontal");
        if (dashDirection.magnitude > 0.1f)
        {
            // Przypisz siłę dashowania do rigidbody
            body.velocity = dashDirection.normalized * dashForce;
        }

        yield return new WaitForSeconds(dashDuration);  // Czas trwania dasha

        body.velocity = Vector3.zero;  // Zatrzymaj ruch po dashu
    }



    private void DropBomb()
    {
        if (ItemBomb != null && isGrounded)
        {
            Vector3 bombPosition = transform.position + new Vector3(0, bombDropHeight, 0);
            currentBomb = Instantiate(ItemBomb, bombPosition, Quaternion.identity);
            currentBomb.gameObject.SetActive(true);
            StartCoroutine(DestroyBombOnExplosion(currentBomb));
        }
    }

    private IEnumerator DestroyBombOnExplosion(GameObject bomb)
    {
        if (bomb != null)
        {
            yield return new WaitForSeconds(bomb.GetComponent<Bomb>().explosionDelay);
            if (bomb != null)
            {
                Destroy(bomb);
            }
            currentBomb = null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (!isGrounded)
            {
                ani.SetTrigger("Land");
            }
            isGrounded = true;
            jumpCount = 0;
        }
    }
}