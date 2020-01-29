using UnityEngine;
using System.Collections;

// this class steers the arrow and its behaviour

namespace BowAndArrow
{
	public class Arrow : MonoBehaviour
	{
		[SerializeField]
		GameObject arrowHead = null;
		[SerializeField]
		GameObject risingText = null;
		[SerializeField]
		AudioClip targetHit = null;
		[SerializeField]
		Transform collisionPointTransform = null;

		// register collision
		bool collisionOccurred;

		bowAndArrow bow;
		SpriteRenderer Renderer;
		Rigidbody2D _rigidBody;
		public Rigidbody2D rigidBody
		{
			get
			{
				if (_rigidBody == null) _rigidBody = GetComponent<Rigidbody2D>();
				return _rigidBody;
			}
		}

		// the vars realize the fading out of the arrow when target is hit
		float alpha;
		float life_loss;

		void Awake()
		{
			Renderer = GetComponent<SpriteRenderer>();
			//rigidBody = GetComponent<Rigidbody>();
		}

		// Use this for initialization
		void Start()
		{
			// set the initialization values for fading out
			float duration = 2f;
			life_loss = 1f / duration;
			alpha = 1f;
		}

		// Update is called once per frame
		void Update()
		{
			//this part of update is only executed, if a rigidbody is present
			// the rigidbody is added when the arrow is shot (released from the bowstring)
			if (rigidBody != null)
			{
				// do we fly actually?
				if (rigidBody.velocity != Vector2.zero)
				{
					// get the actual velocity
					Vector2 vel = rigidBody.velocity;

					// rotate the arrow according to the trajectory
					transform.right = vel;
				}
			}

			// if the arrow hit something...
			if (collisionOccurred)
			{
				// fade the arrow out
				alpha -= Time.deltaTime * life_loss;
				Renderer.color = new Color(1f, 1f, 1f, alpha);

				// if completely faded out, die:
				if (alpha <= 0f)
				{
					// create new arrow
					bow.createArrow(true);
					// and destroy the current one
					Destroy(gameObject);
				}
			}
		}


		//
		// void OnCollisionEnter(Collision other)
		//
		// other: the other object the arrow collided with
		//

		void OnCollisionEnter2D(Collision2D other)
		{
			// we have to determine a score
			int actScore = 0;

			//so, did a collision occur already?
			if (collisionOccurred)
			{
				// fix the arrow and let it not move anymore
				transform.position = new Vector3(other.transform.position.x, transform.position.y, transform.position.z);
				// the rest of the method hasn't to be calculated
				return;
			}

			// I installed cubes as border collider outside the screen
			// If the arrow hits these objects, the player lost an arrow
			if (other.transform.name == "Border Collisions")
			{
				bow.createArrow(false);
				Destroy(gameObject);
				return;
			}

			// Ok - 
			// we hit the target
			if (other.transform.name == "target")
			{
				// play the audio file ("trrrrr")
				GetComponent<AudioSource>().PlayOneShot(targetHit);
				// set velocity to zero
				rigidBody.velocity = Vector3.zero;
				// disable the rigidbody
				rigidBody.isKinematic = true;
				rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
				// and a collision occurred
				collisionOccurred = true;
				// disable the arrow head to create optical illusion
				// that arrow hit the target
				arrowHead.SetActive(false);

				// y is the absolute coordinate on the screen, not on the collider, 
				// so we subtract the collider's position
				float y = collisionPointTransform.position.y;
				y = y - other.transform.position.y;

				// we hit at least white...
				if (y < 1.48557f && y > -1.48691f)
					actScore = 10;
				// ... it could be black, too ...
				if (y < 1.36906f && y > -1.45483f)
					actScore = 20;
				// ... even blue is possible ...
				if (y < 0.9470826f && y > -1.021649f)
					actScore = 30;
				// ... or red ...
				if (y < 0.6095f && y > -0.760f)
					actScore = 40;
				// ... or gold !!!
				if (y < 0.34f && y > -0.53f)
					actScore = 50;

				// create a rising text for score display
				GameObject rt = Instantiate(risingText, new Vector3(0, 0, 0), Quaternion.identity);
				rt.transform.position = other.transform.position + new Vector3(-1, 1, 0);
				rt.transform.name = "rt";
				rt.GetComponent<TextMesh>().text = "+" + actScore;
				// inform the master script about the score
				bow.setPoints(actScore);
			}
		}

		//
		// public void setBow
		//
		// set a reference to the main game object 

		public void setBow(bowAndArrow bow)
		{
			this.bow = bow;
		}
	}
}