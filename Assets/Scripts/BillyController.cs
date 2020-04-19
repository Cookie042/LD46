using System;
using System.Collections.Generic;
using SaveBilly;
using UnityEngine;
using UnityEngine.AI;
using NavMeshBuilder = UnityEditor.AI.NavMeshBuilder;

public class BillyController : MonoBehaviour
{
    public static BillyController instance;
    
    private NavMeshAgent _agent;

    [SerializeField] private LayerMask _foodMask;

    public List<FoodBase> activelyEating = new List<FoodBase>();

    private Transform targetObject;

    public Transform mouthTransform;

    private Animator anim;

    public float maxValues= 100;
    
    public float health = 100;
    public float sugar = 50;
    public float hunger = 50;
    public float thirst = 50;
    
    
    public UIFillBar healthBar;
    public UIFillBar sugarBar;
    public UIFillBar hungerBar;
    public UIFillBar thirstBar;
    
    public float foodConsumeRate = 1;
    private static readonly int Speed_AnimParam = Animator.StringToHash("Speed");

    private void OnEnable()
    {
        instance = this;
    }

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        var colliders = Physics.OverlapSphere(transform.position, 20, _foodMask.value);

        // Debug.Log(string.Join(" - ", colliders.ToList()));
        
        float nearDistance = 10000;
        Collider nearestCollider = null;
        
        foreach (Collider collider in colliders)
        {
            var distance = Vector3.Distance(collider.transform.position, transform.position);

            if (nearDistance > distance && distance > .1f)
            {
                nearDistance = distance;
                nearestCollider = collider;
            }
        }

        if (nearestCollider != null)
        {
            _agent.SetDestination(nearestCollider.transform.position);
        }

        for (int i = 0; i < activelyEating.Count; i++)
        {
            var food = activelyEating[i];
            if (food == null) continue;
            food.EatFood(foodConsumeRate);

            switch (food.foodType)
            {
                case FoodType.GoodFood:
                    hunger -= foodConsumeRate * 3 * Time.deltaTime;
                    break;
                
                case FoodType.GoodDrink:
                    thirst -= foodConsumeRate * 3 * Time.deltaTime;
                    if (thirst < 0) thirst = 0;
                    break;
                
                case FoodType.BadFood:
                    hunger -= foodConsumeRate * 2 * Time.deltaTime;
                    sugar += foodConsumeRate * 2 * Time.deltaTime;
                    break;
                
                case FoodType.BadDrink:
                    thirst -= foodConsumeRate * 2 * Time.deltaTime;
                    sugar += foodConsumeRate * 2 * Time.deltaTime;
                    if (thirst < 0) thirst = 0;
                    break;
            }
        }
        
        thirst += Time.deltaTime * foodConsumeRate * .5f;
        hunger += Time.deltaTime * foodConsumeRate * .5f;
            
        if (thirst > 90 || hunger > 90 || sugar > 90)
        {
            health -= Time.deltaTime * 10f;
        }
        sugar -= Time.deltaTime * foodConsumeRate * .25f;

        sugar = Mathf.Clamp(sugar, 0, 100);
        thirst = Mathf.Clamp(thirst, 0, 100);
        hunger = Mathf.Clamp(hunger, 0, 100);
        
        healthBar.SetValue(health);
        sugarBar.SetValue(sugar);
        hungerBar.SetValue(hunger);
        thirstBar.SetValue(thirst);
        
        anim.SetFloat(Speed_AnimParam, _agent.velocity.magnitude);
    }

    private void OnTriggerEnter(Collider other)
    {
        var food = other.gameObject.GetComponentInParent<FoodBase>();
        if (food != null)
        {
            activelyEating.Add(food);
            //_agent.isStopped = true;
            food.OnFinishedHandler += () =>
            {
                activelyEating.Remove(food);
                Destroy(food.gameObject);
                //_agent.isStopped = false;
            };

        }
    }   
    private void OnTriggerExit(Collider other)
    {
        var food = other.gameObject.GetComponentInParent<FoodBase>();
        if (food != null) activelyEating.Remove(food);

    }

    public void PlayHopSound()
    {
        var audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }
}
