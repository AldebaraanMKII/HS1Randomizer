using UnityEngine;

public class MonoBehaviourMessage : MonoBehaviour
{
	public virtual void Awake()
	{
		MonoBehaviour.print("Awake : " + base.name);
	}

	public virtual void Start()
	{
		MonoBehaviour.print("Start : " + base.name);
	}

	public virtual void OnEnable()
	{
		MonoBehaviour.print("OnEnable : " + base.name);
	}

	public virtual void OnDisable()
	{
		MonoBehaviour.print("OnDisable : " + base.name);
	}

	public virtual void OnDestroy()
	{
		MonoBehaviour.print("OnDestroy : " + base.name);
	}

	public virtual void OnApplicationFocus(bool isFocus)
	{
		MonoBehaviour.print("OnApplicationFocus : " + base.name);
		MonoBehaviour.print("isFocus : " + isFocus);
	}

	public virtual void OnApplicationPause(bool isPause)
	{
		MonoBehaviour.print("OnApplicationPause : " + base.name);
		MonoBehaviour.print("isPause : " + isPause);
	}

	public virtual void OnApplicationQuit()
	{
		MonoBehaviour.print("OnApplicationQuit : " + base.name);
	}

	public virtual void OnLevelWasLoaded()
	{
		MonoBehaviour.print("OnLevelWasLoaded : " + base.name);
	}

	public virtual void OnTransformChildrenChanged()
	{
		MonoBehaviour.print("OnTransformChildrenChanged : " + base.name);
	}

	public virtual void OnTransformParentChanged()
	{
		MonoBehaviour.print("OnTransformParentChanged : " + base.name);
	}

	public virtual void OnValidate()
	{
		MonoBehaviour.print("OnValidate : " + base.name);
	}

	public virtual void Reset()
	{
		MonoBehaviour.print("Reset : " + base.name);
	}

	public virtual void OnAnimatorIK()
	{
		MonoBehaviour.print("OnAnimatorIK : " + base.name);
	}

	public virtual void OnAnimatorMove()
	{
		MonoBehaviour.print("OnAnimatorMove : " + base.name);
	}

	public virtual void OnAudioFilterRead(float[] data, int channels)
	{
		MonoBehaviour.print("OnAudioFilterRead : " + base.name);
	}

	public virtual void OnJointBreak()
	{
		MonoBehaviour.print("OnJointBreak : " + base.name);
	}

	public virtual void OnParticleCollision()
	{
		MonoBehaviour.print("OnParticleCollision : " + base.name);
	}

	public virtual void FixedUpdate()
	{
		MonoBehaviour.print("FixedUpdate : " + base.name);
	}

	public virtual void Update()
	{
		MonoBehaviour.print("Update : " + base.name);
	}

	public virtual void LateUpdate()
	{
		MonoBehaviour.print("LateUpdate : " + base.name);
	}

	public virtual void OnConnectedToServer()
	{
		MonoBehaviour.print("OnConnectedToServer : " + base.name);
	}

	public virtual void OnDisconnectedFromServer()
	{
		MonoBehaviour.print("OnDisconnectedFromServer : " + base.name);
	}

	public virtual void OnFailedToConnect()
	{
		MonoBehaviour.print("OnFailedToConnect : " + base.name);
	}

	public virtual void OnFailedToConnectToMasterServer()
	{
		MonoBehaviour.print("OnFailedToConnectToMasterServer : " + base.name);
	}

	public virtual void OnMasterServerEvent()
	{
		MonoBehaviour.print("OnMasterServerEvent : " + base.name);
	}

	public virtual void OnNetworkInstantiate(NetworkMessageInfo info)
	{
		MonoBehaviour.print("OnNetworkInstantiate : " + base.name);
		MonoBehaviour.print("NetworkMessageInfo : " + info);
	}

	public virtual void OnPlayerConnected()
	{
		MonoBehaviour.print("OnPlayerConnected : " + base.name);
	}

	public virtual void OnPlayerDisconnected()
	{
		MonoBehaviour.print("OnPlayerDisconnected : " + base.name);
	}

	public virtual void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		MonoBehaviour.print("OnSerializeNetworkView : " + base.name);
		MonoBehaviour.print("BitStream : " + stream);
		MonoBehaviour.print("NetworkMessageInfo : " + info);
	}

	public virtual void OnServerInitialized()
	{
		MonoBehaviour.print("OnServerInitialized : " + base.name);
	}

	public virtual void OnMouseDown()
	{
		MonoBehaviour.print("OnMouseDown : " + base.name);
	}

	public virtual void OnMouseUp()
	{
		MonoBehaviour.print("OnMouseUp : " + base.name);
	}

	public virtual void OnMouseUpAsButton()
	{
		MonoBehaviour.print("OnMouseUpAsButton : " + base.name);
	}

	public virtual void OnMouseDrag()
	{
		MonoBehaviour.print("OnMouseDrag : " + base.name);
	}

	public virtual void OnMouseEnter()
	{
		MonoBehaviour.print("OnMouseEnter : " + base.name);
	}

	public virtual void OnMouseExit()
	{
		MonoBehaviour.print("OnMouseExit : " + base.name);
	}

	public virtual void OnMouseOver()
	{
		MonoBehaviour.print("OnMouseOver : " + base.name);
	}

	public virtual void OnControllerColliderHit(ControllerColliderHit hit)
	{
		MonoBehaviour.print("OnControllerColliderHit : " + hit);
	}

	public virtual void OnTriggerEnter(Collider col)
	{
		MonoBehaviour.print("OnTriggerEnter : " + col);
	}

	public virtual void OnTriggerExit(Collider col)
	{
		MonoBehaviour.print("OnTriggerExit : " + col);
	}

	public virtual void OnTriggerStay(Collider col)
	{
		MonoBehaviour.print("OnTriggerStay : " + col);
	}

	public virtual void OnCollisionEnter(Collision col)
	{
		MonoBehaviour.print("OnCollisionEnter : " + col.gameObject);
	}

	public virtual void OnCollisionExit(Collision col)
	{
		MonoBehaviour.print("OnCollisionExit : " + col.gameObject);
	}

	public virtual void OnCollisionStay(Collision col)
	{
		MonoBehaviour.print("OnCollisionStay : " + col.gameObject);
	}

	public virtual void OnTriggerEnter2D(Collider2D col)
	{
		MonoBehaviour.print("OnTriggerEnter2D : " + col);
	}

	public virtual void OnTriggerExit2D(Collider2D col)
	{
		MonoBehaviour.print("OnTriggerExit2D : " + col);
	}

	public virtual void OnTriggerStay2D(Collider2D col)
	{
		MonoBehaviour.print("OnTriggerStay2D : " + col);
	}

	public virtual void OnCollisionEnter2D(Collision2D col)
	{
		MonoBehaviour.print("OnCollisionEnter2D : " + col.gameObject);
	}

	public virtual void OnCollisionExit2D(Collision2D col)
	{
		MonoBehaviour.print("OnCollisionExit2D : " + col.gameObject);
	}

	public virtual void OnCollisionStay2D(Collision2D col)
	{
		MonoBehaviour.print("OnCollisionStay2D : " + col.gameObject);
	}

	public virtual void OnPreCull()
	{
		MonoBehaviour.print("OnPreCull : " + base.name);
	}

	public virtual void OnBecameVisible()
	{
		MonoBehaviour.print("OnBecameVisible : " + base.name);
	}

	public virtual void OnBecameInvisible()
	{
		MonoBehaviour.print("OnBecameInvisible : " + base.name);
	}

	public virtual void OnWillRenderObject()
	{
		MonoBehaviour.print("OnWillRenderObject : " + base.name);
	}

	public virtual void OnPreRender()
	{
		MonoBehaviour.print("OnPreRender : " + base.name);
	}

	public virtual void OnRenderObject()
	{
		MonoBehaviour.print("OnRenderObject : " + base.name);
	}

	public virtual void OnPostRender()
	{
		MonoBehaviour.print("OnPostRender : " + base.name);
	}

	public virtual void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		MonoBehaviour.print("OnRenderImage : " + base.name);
		MonoBehaviour.print("src : " + src);
		MonoBehaviour.print("dest : " + dest);
	}

	public virtual void OnGUI()
	{
		MonoBehaviour.print("OnGUI : " + base.name);
	}

	public virtual void OnDrawGizmos()
	{
		MonoBehaviour.print("OnDrawGizmos : " + base.name);
	}

	public virtual void OnDrawGizmosSelected()
	{
		MonoBehaviour.print("OnDrawGizmosSelected : " + base.name);
	}
}
