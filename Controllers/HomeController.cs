using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using UnityTutorialSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityTutorialSite.Controllers
{
    public class HomeController : Controller
    {
        private const string CorrectPassword = "unity2024";
        private const string AuthKey = "IsAuthenticated";

        public IActionResult Index()
        {
            if (IsAuthenticated())
            {
                return RedirectToAction("Dashboard");
            }

            ViewData["Title"] = "Unity Tutorials Pro - Вход";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string password)
        {
            try
            {
                if (string.IsNullOrEmpty(password))
                {
                    TempData["Error"] = "Введите пароль";
                    return View("Index");
                }

                if (password == CorrectPassword)
                {
                    HttpContext.Session.SetString(AuthKey, "true");
                    HttpContext.Session.SetString("LoginTime", DateTime.Now.ToString());
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    TempData["Error"] = "Неверный пароль. Попробуйте: unity2024";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Ошибка системы: {ex.Message}";
            }

            return View("Index");
        }

        public IActionResult Dashboard()
        {
            if (!IsAuthenticated())
            {
                return RedirectToAction("Index");
            }

            var tutorials = GetTutorials();
            ViewData["Title"] = "Unity Tutorials Pro - Все курсы";
            ViewData["Welcome"] = "Добро пожаловать в панель обучения!";
            return View(tutorials);
        }

        public IActionResult Tutorial(string id)
        {
            if (!IsAuthenticated())
            {
                return RedirectToAction("Index");
            }

            var tutorial = GetTutorialById(id);
            if (tutorial == null)
            {
                TempData["Error"] = "Туториал не найден";
                return RedirectToAction("Dashboard");
            }

            ViewData["Title"] = $"{tutorial.Title} - Unity Tutorials Pro";
            return View(tutorial);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove(AuthKey);
            HttpContext.Session.Remove("LoginTime");
            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            ViewData["Title"] = "Ошибка";
            return View();
        }

        // API для мобильных приложений
        [HttpGet]
        public IActionResult ApiTutorials()
        {
            if (!IsAuthenticated())
            {
                return Json(new { error = "Not authenticated" });
            }

            var tutorials = GetTutorials();
            return Json(tutorials);
        }

        [HttpGet]
        public IActionResult ApiTutorial(string id)
        {
            if (!IsAuthenticated())
            {
                return Json(new { error = "Not authenticated" });
            }

            var tutorial = GetTutorialById(id);
            return Json(tutorial);
        }

        private bool IsAuthenticated()
        {
            return HttpContext.Session.GetString(AuthKey) == "true";
        }

        private List<Tutorial> GetTutorials()
        {
            return new List<Tutorial>
            {
                new Tutorial
                {
                    Id = "3d-movement",
                    Title = "🎮 3D Управление персонажем",
                    Description = "Полное руководство по созданию плавного управления в 3D с использованием Rigidbody и CharacterController",
                    Category = "3D",
                    Difficulty = "Начальный",
                    Duration = "30 минут",
                    Icon = "🎮",
                    LastUpdated = "2024-01-15"
                },
                new Tutorial
                {
                    Id = "2d-movement",
                    Title = "👾 2D Управление персонажем",
                    Description = "Создание классического 2D управления для платформеров с анимациями и физикой",
                    Category = "2D",
                    Difficulty = "Начальный",
                    Duration = "25 минут",
                    Icon = "👾",
                    LastUpdated = "2024-01-10"
                },
                new Tutorial
                {
                    Id = "ui-system",
                    Title = "🖥️ UI Система",
                    Description = "Создание интерактивного пользовательского интерфейса с кнопками, слайдерами и меню",
                    Category = "UI",
                    Difficulty = "Средний",
                    Duration = "45 минут",
                    Icon = "🖥️",
                    LastUpdated = "2024-01-08"
                },
                new Tutorial
                {
                    Id = "animation-basics",
                    Title = "🎭 Основы анимации",
                    Description = "Работа с Animator Controller и создание плавных анимаций персонажей",
                    Category = "Анимация",
                    Difficulty = "Начальный",
                    Duration = "35 минут",
                    Icon = "🎭",
                    LastUpdated = "2024-01-05"
                },
                new Tutorial
                {
                    Id = "physics-system",
                    Title = "⚡ Физика и коллизии",
                    Description = "Изучение физических взаимодействий, коллайдеров и триггеров",
                    Category = "Физика",
                    Difficulty = "Средний",
                    Duration = "40 минут",
                    Icon = "⚡",
                    LastUpdated = "2024-01-03"
                },
                new Tutorial
                {
                    Id = "audio-system",
                    Title = "🔊 Аудио система",
                    Description = "Работа со звуками, музыкой и аудио микшером в Unity",
                    Category = "Аудио",
                    Difficulty = "Начальный",
                    Duration = "20 минут",
                    Icon = "🔊",
                    LastUpdated = "2024-01-01"
                }
            };
        }

        private Tutorial GetTutorialById(string id)
        {
            var tutorials = GetTutorials();
            var tutorial = tutorials.FirstOrDefault(t => t.Id == id);

            if (tutorial != null)
            {
                tutorial.Instructions = GetInstructions(id);
                tutorial.Code = GetTutorialCode(id);
                tutorial.Steps = GetTutorialSteps(id);
                tutorial.VideoUrl = GetVideoUrl(id);
            }

            return tutorial;
        }

        private string GetInstructions(string id)
        {
            return id switch
            {
                "3d-movement" => @"<h3>🎯 Цели обучения:</h3>
<ul>
<li>Создать персонажа с физическим управлением</li>
<li>Реализовать плавное перемещение и повороты</li>
<li>Добавить систему прыжков</li>
<li>Настроить проверку нахождения на земле</li>
</ul>

<h3>📝 Пошаговая инструкция:</h3>

<strong>Шаг 1: Подготовка проекта</strong>
1. Создайте новый 3D проект в Unity
2. Настройте сцену: добавьте плоскость (Plane) как землю
3. Добавьте 3D объект (Cube или Capsule) как персонажа

<strong>Шаг 2: Настройка физики</strong>
1. Выберите вашего персонажа в иерархии
2. Добавьте компонент <code>Rigidbody</code>
3. В инспекторе установите:
   - Mass: 1
   - Drag: 0
   - Angular Drag: 0.05
   - Freeze Rotation: X, Y, Z

<strong>Шаг 3: Создание скрипта</strong>
1. В папке Scripts создайте новый C# скрипт
2. Назовите его <code>PlayerController3D</code>
3. Скопируйте код из раздела 'Код реализации'

<strong>Шаг 4: Настройка персонажа</strong>
1. Перетащите скрипт на вашего персонажа
2. В инспекторе настройте параметры:
   - Move Speed: 5-8
   - Jump Force: 7-10
   - Ground Layer: создайте слой 'Ground'

<strong>Шаг 5: Тестирование</strong>
1. Запустите сцену
2. Проверьте движение с помощью WASD/Стрелок
3. Протестируйте прыжок пробелом
4. Убедитесь, что персонаж не проходит сквозь землю",

                "2d-movement" => @"<h3>🎯 Цели обучения:</h3>
<ul>
<li>Создать 2D персонажа с физикой</li>
<li>Реализовать горизонтальное движение</li>
<li>Добавить систему прыжков</li>
<li>Настроить автоматический поворот спрайта</li>
</ul>

<h3>📝 Пошаговая инструкция:</h3>

<strong>Шаг 1: Подготовка 2D проекта</strong>
1. Создайте новый 2D проект в Unity
2. Импортируйте спрайты персонажа и платформ
3. Настройте сортировку слоев

<strong>Шаг 2: Создание персонажа</strong>
1. Добавьте спрайт персонажа на сцену
2. Добавьте компоненты:
   - <code>Rigidbody2D</code>
   - <code>BoxCollider2D</code>
3. Настройте Rigidbody2D:
   - Body Type: Dynamic
   - Gravity Scale: 3

<strong>Шаг 3: Настройка земли</strong>
1. Создайте объект для земли
2. Добавьте <code>BoxCollider2D</code>
3. Назначьте слой 'Ground'

<strong>Шаг 4: Создание скрипта управления</strong>
1. Создайте скрипт <code>PlayerController2D</code>
2. Скопируйте предоставленный код
3. Настройте ссылки в инспекторе

<strong>Шаг 5: Тестирование</strong>
1. Запустите игру
2. Проверьте движение влево/вправо
3. Убедитесь, что прыжок работает корректно
4. Проверьте поворот спрайта",

                _ => @"<h3>📚 Стандартная инструкция:</h3>
<ol>
<li>Создайте новый проект в Unity</li>
<li>Подготовьте необходимые ассеты</li>
<li>Создайте и настройте игровые объекты</li>
<li>Добавьте и настройте скрипты</li>
<li>Протестируйте функциональность</li>
<li>Оптимизируйте и доработайте</li>
</ol>"
            };
        }

        private string GetTutorialCode(string id)
        {
            return id switch
            {
                "3d-movement" => @"using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController3D : MonoBehaviour
{
    [Header(""Movement Settings"")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private float rotationSpeed = 10f;
    
    [Header(""Physics Settings"")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float airControl = 0.5f;
    
    private Rigidbody rb;
    private bool isGrounded;
    private float horizontalInput;
    private float verticalInput;
    private bool jumpInput;
    
    // Public properties for other scripts
    public bool IsGrounded => isGrounded;
    public Vector3 Velocity => rb.velocity;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetupRigidbody();
    }
    
    void Update()
    {
        GetInput();
        CheckGrounded();
        HandleJumpInput();
    }
    
    void FixedUpdate()
    {
        HandleMovement();
        HandleRotation();
    }
    
    void GetInput()
    {
        horizontalInput = Input.GetAxisRaw(""Horizontal"");
        verticalInput = Input.GetAxisRaw(""Vertical"");
        jumpInput = Input.GetButtonDown(""Jump"");
    }
    
    void HandleMovement()
    {
        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput).normalized;
        
        // Apply movement
        if (movement.magnitude >= 0.1f)
        {
            float currentSpeed = moveSpeed * (isGrounded ? 1f : airControl);
            Vector3 targetVelocity = movement * currentSpeed;
            targetVelocity.y = rb.velocity.y;
            
            rb.velocity = targetVelocity;
        }
        else if (isGrounded)
        {
            // Apply friction when grounded and not moving
            Vector3 velocity = rb.velocity;
            velocity.x *= 0.9f;
            velocity.z *= 0.9f;
            rb.velocity = velocity;
        }
    }
    
    void HandleRotation()
    {
        if (rb.velocity.magnitude > 0.1f)
        {
            Vector3 direction = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, 
                rotationSpeed * Time.fixedDeltaTime);
        }
    }
    
    void HandleJumpInput()
    {
        if (jumpInput && isGrounded)
        {
            Jump();
        }
    }
    
    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
    }
    
    void CheckGrounded()
    {
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 
            out hit, groundCheckDistance, groundLayer);
        
        // Visual debug
        Debug.DrawRay(transform.position, Vector3.down * groundCheckDistance, 
            isGrounded ? Color.green : Color.red);
    }
    
    void SetupRigidbody()
    {
        rb.drag = 0f;
        rb.angularDrag = 0.05f;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }
    
    // Public method for external control
    public void AddForce(Vector3 force, ForceMode mode = ForceMode.Force)
    {
        rb.AddForce(force, mode);
    }
    
    // Reset player position
    public void ResetPosition(Vector3 newPosition)
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = newPosition;
    }
}",

                "2d-movement" => @"using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    [Header(""Movement Settings"")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private float airControlFactor = 0.8f;
    
    [Header(""Visual Settings"")]
    [SerializeField] private Transform graphics;
    [SerializeField] private float flipSpeed = 5f;
    
    [Header(""Physics Settings"")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheckPoint;
    
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool facingRight = true;
    private float horizontalInput;
    private bool jumpInput;
    private bool canJump = true;
    
    // Animation references
    private Animator animator;
    
    // Public properties
    public bool IsGrounded => isGrounded;
    public bool IsFacingRight => facingRight;
    public Vector2 Velocity => rb.velocity;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        SetupRigidbody();
    }
    
    void Update()
    {
        GetInput();
        CheckGrounded();
        HandleJumpInput();
        UpdateAnimations();
    }
    
    void FixedUpdate()
    {
        HandleMovement();
        HandleFlip();
    }
    
    void GetInput()
    {
        horizontalInput = Input.GetAxisRaw(""Horizontal"");
        jumpInput = Input.GetButtonDown(""Jump"");
    }
    
    void HandleMovement()
    {
        float currentSpeed = moveSpeed * (isGrounded ? 1f : airControlFactor);
        Vector2 velocity = rb.velocity;
        velocity.x = horizontalInput * currentSpeed;
        rb.velocity = velocity;
    }
    
    void HandleFlip()
    {
        if (horizontalInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (horizontalInput < 0 && facingRight)
        {
            Flip();
        }
    }
    
    void Flip()
    {
        facingRight = !facingRight;
        
        if (graphics != null)
        {
            Vector3 scale = graphics.localScale;
            scale.x = Mathf.Abs(scale.x) * (facingRight ? 1 : -1);
            graphics.localScale = Vector3.Lerp(graphics.localScale, scale, 
                flipSpeed * Time.fixedDeltaTime);
        }
        else
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (facingRight ? 1 : -1);
            transform.localScale = scale;
        }
    }
    
    void HandleJumpInput()
    {
        if (jumpInput && isGrounded && canJump)
        {
            Jump();
        }
    }
    
    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        canJump = false;
        Invoke(nameof(ResetJump), 0.2f);
    }
    
    void ResetJump()
    {
        canJump = true;
    }
    
    void CheckGrounded()
    {
        if (groundCheckPoint != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, 
                groundCheckRadius, groundLayer);
        }
    }
    
    void UpdateAnimations()
    {
        if (animator != null)
        {
            animator.SetFloat(""Speed"", Mathf.Abs(horizontalInput));
            animator.SetBool(""IsGrounded"", isGrounded);
            animator.SetFloat(""VerticalVelocity"", rb.velocity.y);
        }
    }
    
    void SetupRigidbody()
    {
        rb.gravityScale = 3f;
        rb.freezeRotation = true;
    }
    
    // Public methods
    public void AddForce(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
    }
    
    public void Teleport(Vector2 newPosition)
    {
        rb.velocity = Vector2.zero;
        transform.position = newPosition;
    }
    
    // Visual debug
    void OnDrawGizmosSelected()
    {
        if (groundCheckPoint != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        }
    }
}",

                _ => @"using UnityEngine;

public class TutorialTemplate : MonoBehaviour
{
    [Header(""Basic Settings"")]
    [SerializeField] private string tutorialName = ""New Tutorial"";
    
    void Start()
    {
        Debug.Log($""🚀 Starting tutorial: {tutorialName}"");
        Initialize();
    }
    
    void Update()
    {
        // Update logic here
    }
    
    void Initialize()
    {
        // Initialization code here
        Debug.Log(""✅ Tutorial initialized successfully!"");
    }
    
    // Public method example
    public void ExampleMethod()
    {
        Debug.Log(""📝 This is an example method"");
    }
}"
            };
        }

        private List<string> GetTutorialSteps(string id)
        {
            return new List<string>
            {
                "Подготовка проекта и настройка сцены",
                "Создание и конфигурация игровых объектов",
                "Написание и настройка скриптов",
                "Тестирование и отладка функциональности",
                "Оптимизация и финальная полировка"
            };
        }

        private string GetVideoUrl(string id)
        {
            return $"https://example.com/videos/{id}";
        }
    }
}
