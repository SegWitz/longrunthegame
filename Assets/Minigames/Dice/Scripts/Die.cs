/**
 * Copyright (c) 2010-2015, WyrmTale Games and Game Components
 * All rights reserved.
 * http://www.wyrmtale.com
 *
 * THIS SOFTWARE IS PROVIDED BY WYRMTALE GAMES AND GAME COMPONENTS 'AS IS' AND ANY
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL WYRMTALE GAMES AND GAME COMPONENTS BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR 
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */
using UnityEngine;

/// <summary>
/// Die base class to determine if a die is rolling and to calculate it's current value
/// </summary>
public class Die : MonoBehaviour
{
	Rigidbody RigidbodyReference;

	float[] Results = new float[6];

	// true if die is still rolling
	public bool rolling
	{
		get
		{
			return !(RigidbodyReference.velocity.sqrMagnitude < .1F && RigidbodyReference.angularVelocity.sqrMagnitude < .1F);
		}
	}

	// current value, 0 is undetermined (die is rolling) or invalid.
	public int value { get; private set; }

	void Awake()
	{
		RigidbodyReference = GetComponent<Rigidbody>();
	}

	void Update()
	{
		// determine the value is the die is not rolling
		if (!rolling) GetValue();
	}

	// calculate this die's value
	void GetValue()
	{
		for (int f = 0; f < 6; f++)
		{
			Results[f] = Vector3.Dot(transform.TransformDirection(HitVector(f + 1)), Vector3.up);
		}

		int UpFaceIndex = 0;
		float TempValue = -1;

		for (int f = 0; f < 6; f++)
		{
			if (Results[f] > TempValue)
			{
				UpFaceIndex = f + 1;
				TempValue = Results[f];
			}
		}

		value = UpFaceIndex;
	}

	// virtual  method that to get a die side hitVector.
	// this has to be overridden in the dieType specific subclass
	protected virtual Vector3 HitVector(int side)
	{
		return Vector3.zero;
	}
}