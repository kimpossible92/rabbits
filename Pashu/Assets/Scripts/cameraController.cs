using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class cameraController : MonoBehaviour
{
    private AnimalController animalController;
    public void SetAnimal(AnimalController animalControll)
    {
        animalController = animalControll;
    }
    public AnimalController AnimalControll { get => animalController; set => animalController = value; }
    public bool SetBomb { get => setBomb; set => setBomb = value; }

    public static cameraController instance;
    public Transform followTransform;
    public Transform CameraTransform;

    public float moveSpeed;

    public float moveTime;
    public float rotationAmount;
    public Vector3 zoomAmount;

    private Vector3 newPos;
    public Vector3 newZoom;
    private Quaternion newRot;
    private List<AnimalController> animalControllers_ = new List<AnimalController>();
    public void AddAnimal(AnimalController animal)
    {
        animalControllers_.Add(animal);
    }
    public void DeleteAnimal(AnimalController animal)
    {
        animalControllers_.Remove(animal);
    }
    //mouse
    private Vector3 dragStart;
    private Vector3 dragEnd;
    [HideInInspector] private bool setBomb = false;
    public void setBoolBomb(bool bomb)
    {
        setBomb = bomb;
    }
    private Vector3 RotateStart;
    private Vector3 RotateEnd;
    private int numerics = 0;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        newPos = transform.position;
        newRot = transform.rotation;
        newZoom = CameraTransform.localPosition;
        animalControllers_.Add(FindObjectOfType<AnimalController>());
        numerics = 0;
        if (animalControllers_[numerics] != null)
        { 
            animalController = animalControllers_[0]; 
        }
    }
    public void nextNumeric()
    {
        //animalControllers_ = FindObjectsOfType<AnimalController>();
        if (animalControllers_.Count > numerics + 1)
        {
            numerics += 1;
            animalController = animalControllers_[numerics];
        }
    }
    public void backNumeric()
    {
        //animalControllers_ = FindObjectsOfType<AnimalController>();
        if (numerics!=0)
        {
            numerics -= 1;
            animalController = animalControllers_[numerics];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (setBomb)
        {
            PointBomb();
        }
        else
        {

        }
        if (followTransform != null)
        {
            transform.position = followTransform.position;
        }

        HandleInput();
        MouseInput();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            followTransform = null;
            animalController = null;
        }
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out raycastHit))
            {
                Transform objectHit = raycastHit.transform;
                animalController.GetComponent<NavMeshAgent>().SetDestination(raycastHit.point);
                if (animalController != null) 
                {
                    if (setBomb) { animalController.SetTheBomb(raycastHit.point); }
                    else { animalController.SetTarget(raycastHit.point); }
                }
            }
            
        }
    }

    GameObject preViewBomb_;
    public void PointBomb()
    {

        RaycastHit raycastHit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out raycastHit))
        {
            preViewBomb_.transform.position = raycastHit.point;
        }
    }
    public void SetTheBomb(GameObject previewBomb)
    {
        RaycastHit raycastHit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out raycastHit))
        {
            var prev = Instantiate(previewBomb, raycastHit.point, Quaternion.identity);
            preViewBomb_ = prev;
            setBomb = true;
            //if (animalController != null)
            //{
            //    animalController.SetTheBomb(raycastHit.point);
            //}
        }
        
    }

    void HandleInput()
    {
        newZoom.y = Mathf.Clamp(newZoom.y, 10, 150);
        newZoom.z = Mathf.Clamp(newZoom.z, -150, -10);
        if (followTransform == null)
        {
            if (Mathf.Abs(Input.GetAxis("Vertical")) > 0)
            {
                newPos += transform.forward * moveSpeed * Input.GetAxis("Vertical");
            }
            if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0)
            {
                newPos += transform.right * moveSpeed * Input.GetAxis("Horizontal");
            }
            if (Input.GetKey(KeyCode.Q))
            {
                newRot *= Quaternion.Euler(Vector3.up * rotationAmount);
            }
            if (Input.GetKey(KeyCode.E))
            {
                newRot *= Quaternion.Euler(Vector3.up * -rotationAmount);
            }

            transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * moveTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, newRot, Time.deltaTime * moveTime);
        }

        CameraTransform.localPosition = Vector3.Lerp(CameraTransform.localPosition, newZoom, Time.deltaTime * moveTime);
    }

    void MouseInput()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {


            if (Input.mouseScrollDelta.y != 0)
            {
                newZoom += Input.mouseScrollDelta.y * zoomAmount;
            }
            if (followTransform == null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Plane plane = new Plane(Vector3.up, Vector3.zero);
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    float start;
                    if (plane.Raycast(ray, out start))
                    {
                        dragStart = ray.GetPoint(start);
                    }
                }

                if (Input.GetMouseButton(0))
                {
                    Plane plane = new Plane(Vector3.up, Vector3.zero);
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    float point;
                    if (plane.Raycast(ray, out point))
                    {
                        dragEnd = ray.GetPoint(point);
                        newPos = transform.position + (dragStart - dragEnd);
                    }
                }

                if (Input.GetMouseButtonDown(2))
                {
                    RotateStart = Input.mousePosition;
                }

                if (Input.GetMouseButton(2))
                {
                    RotateEnd = Input.mousePosition;
                    Vector3 difference = RotateStart - RotateEnd;
                    RotateStart = RotateEnd;

                    //newRot *= Quaternion.Euler(Vector3.right * (-difference.y / 5f)); //this is for up-down rotation
                    newRot *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));


                }
            }
        }
    }
}