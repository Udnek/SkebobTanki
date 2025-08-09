using Tank.Parts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Test
{
    public class Track : MonoBehaviour
    {
        private Rigidbody tankRigidbody;
        private Controls controls;
        [SerializeField] private bool isLeftTrack;

        [SerializeField] private LayerMask groundLayer;
        private readonly float FIRST_TRACK_STATUS = 1f;
        private readonly float SECOND_TRACK_STATUS = 0.1f; 
        private float steeringForceMultiplier = 1f;
        [FormerlySerializedAs("tankTrackTriggerCollider")] [SerializeField] private BoxCollider hitbox;
        
        private float speedRight;
        private float speedLeft;
        
        //private Vector3 trackPositionLocal;
        //private Vector3 trackPositionWorld;

        private void Awake()
        {
            controls = new Controls();
            controls.Player.Move.Enable();
        }

        private void Start()
        {
            tankRigidbody = GetComponent<Part>().tank.GetComponent<Rigidbody>();
            DefineTrackSide();
            if (groundLayer.value == 0)
                groundLayer = LayerMask.GetMask("Default");
        }
        
        private void DefineTrackSide()
        {
            // Vector3 centerInTankSpace = GetComponent<Part>().tank.transform.position;
            // isLeftTrack = centerInTankSpace.x < 0;
            
            if (isLeftTrack)
            {
                speedRight = FIRST_TRACK_STATUS;
                speedLeft = SECOND_TRACK_STATUS;
            }
            else
            {
                speedRight = SECOND_TRACK_STATUS;
                speedLeft = FIRST_TRACK_STATUS;
            }
        }

        private void FixedUpdate()
        {
            Vector2 input = controls.Player.Move.ReadValue<Vector2>();
            if (CheckTrackGrounded(hitbox)) 
                TrackMovement(input);
        }
        
        private bool CheckTrackGrounded(BoxCollider trackCollider)
        {
            if (trackCollider == null) return false;
        
            Vector3 center = trackCollider.transform.TransformPoint(trackCollider.center);
            Vector3 halfExtents = trackCollider.size * 0.5f;
            Quaternion rotation = trackCollider.transform.rotation;

            var isGround = Physics.CheckBox(center, halfExtents, rotation);
            //Debug.Log(isGround);
            return isGround;
        }

        private void TrackMovement(Vector2 input)
        {
            //Debug.Log(readMoveVector);
            //Debug.Log(readTurnVector);
            float forward = input.y;
            float rightward = input.x;
            Vector3 trackForce;
            if (forward != 0)
            {
                trackForce = transform.forward * forward;
                if (rightward != 0) steeringForceMultiplier = rightward > 0 ? speedRight : speedLeft;
                else steeringForceMultiplier = 1;
                trackForce *= steeringForceMultiplier * 10;
            }
            else if (rightward != 0)
            {
                trackForce = transform.forward * ((isLeftTrack ? 1 : -1) * 20);
                //Debug.Log(trackForce);
            }
            else trackForce = new();
            
            tankRigidbody.AddForceAtPosition(trackForce, transform.TransformPoint(hitbox.center));
        }
    }
}