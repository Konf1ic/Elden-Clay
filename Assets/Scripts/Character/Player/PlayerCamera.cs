using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kf {
    public class PlayerCamera : MonoBehaviour {
        public static PlayerCamera instance;
        public PlayerManager player;
        public Camera cameraObject;
        [SerializeField] Transform cameraPrivotTransform;


        //  CHANGE THESE TO TWEAK CAMERA
        [Header("Camera Settings")]
        private float cameraSmoothSpeed = 1;    //  BIGGER THE NUMBER, THE LONGER THE CAMERA REACH IT POSITION DURING MOVEMENT
        [SerializeField] float leftAndRightLookSpeed = 220;
        [SerializeField] float upAndDownLookSpeed = 220;
        [SerializeField] float minimumPivot = -30;  //  THE LOWEST POINT THE CAMERA CAN LOOK DOWN
        [SerializeField] float maximumPivot = 60;   //  THE HIGHEST POINT THE CAMERA CAN LOOK UP
        [SerializeField] float cameraCollisionRadius = 0.2F;
        [SerializeField] LayerMask collideWithLayers;

        [Header("Camera Valuess")]
        private Vector3 cameraVerlocity;
        private Vector3 cameraObjectPosition;   //  USE FOR CAMERA COLLISION (MOVE THE CAMERA TO THIS POSITION UPON COLLIDING)
        [SerializeField] float leftAndRightLookAngle;
        [SerializeField] float upAndDownLookAngle;
        private float cameraZPosition;    //  VALUE USE FOR CAMERA COLLISION
        private float targetCameraZPosition;     //  VALUE USE FOR CAMERA COLLISION

        private void Awake() {
            if (instance == null) {
                instance = this;
            } else {
                Destroy(gameObject);
            }
        }
        private void Start() {
            DontDestroyOnLoad(gameObject);
            cameraZPosition = cameraObject.transform.localPosition.z;
        }

        public void HandleAllCameraAction() {
            if (player != null) {
                HandleFollowTarget();
                HandleRotation();
                HandleCollision();
            }
        }

        public void HandleFollowTarget() {
            Vector3 targetcameraPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVerlocity, cameraSmoothSpeed * Time.deltaTime);
            transform.position = targetcameraPosition;
        }

        public void HandleRotation() {
            //  IF LOCL ON, FOCUS ROTATION TOWARD TARGE
            //  ELSE ROTATE NOMALLY

            //  ROTATE LEFT AND RIGHT BASE ON HORIZONTAL MOVEMENT ON THE RIGHT JOYSTICK
            leftAndRightLookAngle += (PlayerInputManager.instance.cameraHorizontalInput * leftAndRightLookSpeed) * Time.deltaTime;
            //  ROTATE UP AND DOWN BASE ON VERTICAL MOVEMENT ON THE RIGHT JOYSTICK
            upAndDownLookAngle -= (PlayerInputManager.instance.cameraVerticalInput * upAndDownLookSpeed) * Time.deltaTime;
            //  CLAMP THE UP AND DOWN LOOK ANGLE BETWEEN A MIN AND MAX VALUE
            upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);


            Vector3 cameraRotation = Vector3.zero;
            Quaternion targetRotation;

            //  ROTATE THIS GAME OBJ LEFT AND RIGHT
            cameraRotation.y = leftAndRightLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            transform.rotation = targetRotation;

            //  ROTATE THE PIVOT GAME OBJ UP AND DOWN
            cameraRotation = Vector3.zero;
            cameraRotation.x = upAndDownLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            cameraPrivotTransform.localRotation = targetRotation;
        }

        public void HandleCollision() {
            targetCameraZPosition = cameraZPosition;

            RaycastHit hit;
            //  DIRECTION FOR COLLISION CHECK
            Vector3 direction = cameraPrivotTransform.position - cameraObject.transform.position;
            direction.Normalize();

            //  WE CHECK IF THE CAMERA IS IN FRONT OF OUR WANTED DIRICTION ^ (ABOVE)
            if (Physics.SphereCast(cameraPrivotTransform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetCameraZPosition), 0)){
                //  IF THERE IS, WE CHECK OUR DISTANCE FROM IT
                float distanceFromHitObject = Vector3.Distance(cameraPrivotTransform.position, hit.point);
                //  WE THEN EQUATE OUR TARGET Z POSITION TO THE FOLLOWING
                targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);
            }

            //  IF OUR TARGET POSITION IS LESS THAN OUR COLLISION RADIUS, WE SUPTRACT COLLISION RADIUS (MAKE IS SNAP BACK)
            if ( Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius) {
                targetCameraZPosition = -cameraCollisionRadius;
            }

            //  WE THEN APPLY OUR FINAL POSITION USING A LERP OVER A TIME OF 0.2F
            cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, 0.2f);
            cameraObject.transform.localPosition = cameraObjectPosition;
        }
    }
}