using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof (CharacterController))]
    [RequireComponent(typeof (AudioSource))]
    public class FirstPersonController : MonoBehaviour
    {
        #pragma warning disable CS0649
        [SerializeField] private bool m_IsWalking;
        [SerializeField] private float m_WalkSpeed;
        [SerializeField] private float m_RunSpeed;
        [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
        [SerializeField] private float m_JumpSpeed;
        [SerializeField] private float m_StickToGroundForce;
        [SerializeField] private float m_GravityMultiplier;
        [SerializeField] private MouseLook m_MouseLook = new MouseLook();
        [SerializeField] private bool m_UseFovKick;
        [SerializeField] private FOVKick m_FovKick = new FOVKick();
        [SerializeField] private bool m_UseHeadBob;
        [SerializeField] private CurveControlledBob m_HeadBob = new CurveControlledBob();
        [SerializeField] private LerpControlledBob m_JumpBob = new LerpControlledBob();
        [SerializeField] private float m_StepInterval;
        [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
        [SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
        [SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.
        #pragma warning restore CS0649

        private Camera m_Camera;
        private bool rotateCamera;
        private bool m_Jump;
        private float m_YRotation;
        private Vector2 m_Input;
        private Vector3 m_MoveDir = Vector3.zero;
        private CharacterController m_CharacterController;
        private CollisionFlags m_CollisionFlags;
        private bool m_PreviouslyGrounded;
        private Vector3 m_OriginalCameraPosition;
        private float m_StepCycle;
        private float m_NextStep;
        private bool m_Jumping;
        private AudioSource m_AudioSource;
        private Vector3 speedbeforeJump;
        private float jump_time;
        private float default_jump_duration;
        private float default_jump_amount;
        private player_status playerStatus;
        private float m_jumpAngleLimit;
        // Use this for initialization
        // Private debug

        private void Start()
        {
            m_CharacterController = GetComponent<CharacterController>();
            m_jumpAngleLimit = 90 - m_CharacterController.slopeLimit;
            m_Camera = Camera.main;
            m_OriginalCameraPosition = m_Camera.transform.localPosition;
            m_FovKick.Setup(m_Camera);
            m_HeadBob.Setup(m_Camera, m_StepInterval);
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle/2f;
            m_Jumping = false;
            m_AudioSource = GetComponent<AudioSource>();
			m_MouseLook.Init(transform , m_Camera.transform);
            default_jump_duration = m_JumpBob.BobDuration;
            default_jump_amount = m_JumpBob.BobAmount;
            rotateCamera = true;
            playerStatus = GetComponent<player_status>();
        }


        // Update is called once per frame
        private void Update()
        {
            // rotate the camera
            if (rotateCamera)
            {
                RotateView();
            }
            // the jump state needs to read here to make sure it is not missed
            // learn if player choosed to jump
            if (!m_Jump)
            {
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
                // Check the normal of the ground and see if it is allowed to jump
                RaycastHit hitInfo;
                if (Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                                  m_CharacterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                {
                    Vector3 hitNormal = hitInfo.normal;
                    float angle = (float)(Math.Atan(hitNormal.y / Vector3.Distance(new Vector3(0, 0, 0), new Vector3(hitNormal.x, 0, hitNormal.z))) / Math.PI * 180);
                    if (angle < m_jumpAngleLimit)
                    {
                        m_Jump = false;
                    }
                }
            }
            // transition from jumping to ground
            if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
            {
                //print(jump_time - Time.time);
                if (Time.time - jump_time < 0.7)
                {
                    m_JumpBob.setBob(default_jump_duration, default_jump_amount);
                } else
                {
                    m_JumpBob.setBob(default_jump_duration + (Time.time - jump_time - (float)0.7) * (float)0.2, default_jump_amount + (Time.time - jump_time - (float)0.7) * (float)1);
                }
                StartCoroutine(m_JumpBob.DoBobCycle());
                PlayLandingSound();
                m_MoveDir.y = 0f;
                m_Jumping = false;
                m_Jump = false;
            }
            if (m_PreviouslyGrounded && !m_CharacterController.isGrounded) {
                jump_time = Time.time;
            }
            // transition from ground to jump, but have not captured by the fix update
            if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
            {
                m_MoveDir.y = 0f;
            }

            m_PreviouslyGrounded = m_CharacterController.isGrounded;
        }


        private void PlayLandingSound()
        {
            m_AudioSource.clip = m_LandSound;
            m_AudioSource.Play();
            m_NextStep = m_StepCycle + .5f;
        }


        private void FixedUpdate()
        {
            float speed;
            
            GetInput(out speed);
            
            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = transform.forward* (float)Math.Pow(Input.GetAxis("Vertical"),7) + transform.right* (float)Math.Pow(Input.GetAxis("Horizontal"),7);
            
            if (!playerStatus.Scree_free())
            {
                desiredMove = new Vector3(0, 0, 0);
            }

            // This is the correction for slope. The player are forced to slide down the slope if it is too steep
            Vector3 slopeAdded = new Vector3(0, 0, 0);    

            // get a normal for the surface that is being touched to move along it
            RaycastHit hitInfo;
            if (Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                              m_CharacterController.height/2f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                Vector3 hitNormal = hitInfo.normal;
                // if the angle exceed the limit
                float angle = (float)(Math.Atan(hitNormal.y / Vector3.Distance(new Vector3(0, 0, 0), new Vector3(hitNormal.x, 0, hitNormal.z))) / Math.PI * 180);
                if (angle < m_jumpAngleLimit)
                {
                    Vector3 downVector = new Vector3(0, -m_StickToGroundForce, 0);
                    slopeAdded = Vector3.ProjectOnPlane(downVector, hitInfo.normal);
                    slopeAdded.y = 0;
                }

            }
            
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;
            // print(desiredMove);
            m_MoveDir.x = desiredMove.x*speed;
            m_MoveDir.z = desiredMove.z*speed;
            m_MoveDir.x *= 2;
            m_MoveDir.z *= 2;
                       

            if (m_CharacterController.isGrounded)
            {
               
                m_MoveDir.y = -m_StickToGroundForce;
                speedbeforeJump = m_MoveDir;
                if (m_Jump)
                {
                    m_MoveDir.y = m_JumpSpeed;
                    PlayJumpSound();
                    
                    m_Jump = false;
                    m_Jumping = true;
                }
                // if the player is on the ground, apply this correction
                m_MoveDir += slopeAdded;
            }
            else
            {
                m_Jump = false;
                if (m_MoveDir.x != 0 || m_MoveDir.z != 0)
                {
                    speedbeforeJump = m_MoveDir;
                } else
                {
                    m_MoveDir.x = speedbeforeJump.x;
                    m_MoveDir.z = speedbeforeJump.z;
                }
                m_MoveDir.x *= (float)0.85;
                m_MoveDir.z *= (float)0.85;
                m_MoveDir += Physics.gravity*m_GravityMultiplier*Time.fixedDeltaTime;
                //print(m_MoveDir);
            }
            //print(m_MoveDir);
            
            m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);
            ProgressStepCycle(speed);
            UpdateCameraPosition(speed);

            m_MouseLook.UpdateCursorLock();
        }

        private void OnDrawGizmosSelected()
        {
            // Gizmos.color = Color.red;
            // Debug.DrawLine(transform.position, transform.position + Vector3.down * hitDistance);
            // Gizmos.DrawWireSphere(transform.position + Vector3.down * hitDistance, (float)0.5);
        }

        private void PlayJumpSound()
        {
            m_AudioSource.clip = m_JumpSound;
            m_AudioSource.Play();
        }


        private void ProgressStepCycle(float speed)
        {
            if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
            {
                m_StepCycle += (m_CharacterController.velocity.magnitude + (speed*(m_IsWalking ? 1f : m_RunstepLenghten)))*
                             Time.fixedDeltaTime;
            }

            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;

            PlayFootStepAudio();
        }


        private void PlayFootStepAudio()
        {
            if (!m_CharacterController.isGrounded)
            {
                return;
            }
            // pick & play a random footstep sound from the array,
            // excluding sound at index 0
            int n = Random.Range(1, m_FootstepSounds.Length);
            m_AudioSource.clip = m_FootstepSounds[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            m_FootstepSounds[n] = m_FootstepSounds[0];
            m_FootstepSounds[0] = m_AudioSource.clip;
        }


        private void UpdateCameraPosition(float speed)
        {
            Vector3 newCameraPosition;
            if (!m_UseHeadBob)
            {
                return;
            }
            if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
            {
                m_Camera.transform.localPosition =
                    m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
                                      (speed*(m_IsWalking ? 1f : m_RunstepLenghten)));
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
            }
            else
            {
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
            }
            m_Camera.transform.localPosition = newCameraPosition;
        }


        private void GetInput(out float speed)
        {
            // Read input
            float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
            float vertical = CrossPlatformInputManager.GetAxis("Vertical");

            bool waswalking = m_IsWalking;

#if !MOBILE_INPUT
            // On standalone builds, walk/run speed is modified by a key press.
            // keep track of whether or not the character is walking or running
            m_IsWalking = !Input.GetKey(KeyCode.LeftShift);
#endif
            // set the desired speed to be walking or running
            speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;

            //if ((horizontal != 0 && vertical <= 0) || vertical < 0)
            //{
            //    speed = m_WalkSpeed*3/4;
              
            //}

            m_Input = new Vector2(horizontal, vertical);


            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1)
            {
                m_Input.Normalize();
            }

            // handle speed change to give an fov kick
            // only if the player is going to a run, is running and the fovkick is to be used
            if (m_IsWalking != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0)
            {
                StopAllCoroutines();
                StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
            }
        }

        ///rotate the camera based on the mouse
        private void RotateView()
        {
            m_MouseLook.LookRotation (transform, m_Camera.transform);
        }


        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            //Rigidbody body = hit.collider.attachedRigidbody;
            ////dont move the rigidbody if the character is on top of it
            //if (m_CollisionFlags == CollisionFlags.Below)
            //{
            //    return;
            //}

            //if (body == null || body.isKinematic)
            //{
            //    return;
            //}
            //body.AddForceAtPosition(m_CharacterController.velocity*0.1f, hit.point, ForceMode.Impulse);
        }

        public void ToggleRotation(bool allowRotation)
        {
            this.rotateCamera = allowRotation;
        }
    }
}
