using UnityEngine;
using System.Collections;

public class Tweener
{
	public delegate void CallBackDelegate();
	private CallBackDelegate _Callback = null;

	public delegate float TweenDelegate(  float t, float b, float c, float d );
	private TweenDelegate _Easing = null;

	private Vector3 _From			= Vector3.zero;
	private Vector3 _To				= Vector3.zero;

	private float _ProgressPct	= 0.0f;

	public float progressPct
	{
		get
		{
			return _ProgressPct;
		}
	}

	private bool _Animating			= false;
	private float _TimeElapsed		= 0.0f;
	private float _Duration			= 1.0f;
	private Vector3 _Progression	= Vector3.zero;
	
	public Tweener (){}

	/// <summary>
	/// Eases from value to value.
	/// </summary>
	/// <param name="from">From.</param>
	/// <param name="to">To.</param>
	/// <param name="duration">Duration.</param>
	/// <param name="easing">Easing.</param>
	public void easeFromTo( Vector3 from,  Vector3 to, float duration=1f, TweenDelegate easing=null, CallBackDelegate callback=null )
	{
		if(easing==null)
		{
			easing = Easing.Linear;
		}

		_Easing 	= easing;
		_Callback	= callback;

		_From 	= from;
		_To		= to;

		_Duration 		= duration;
		_TimeElapsed 	= 0f;
		_ProgressPct	= 0f;

		_Animating = true;
	}

	public void update (bool callCallBack = true)
	{
		if( _Animating )
		{
			if( _TimeElapsed<_Duration )
			{
				if( _Easing!=null )
				{
					_Progression.x = _Easing.Invoke( _TimeElapsed, _From.x, (_To.x - _From.x), _Duration );
					_Progression.y = _Easing.Invoke( _TimeElapsed, _From.y, (_To.y - _From.y), _Duration );
					_Progression.z = _Easing.Invoke( _TimeElapsed, _From.z, (_To.z - _From.z), _Duration );

					_ProgressPct = _TimeElapsed / _Duration;

					_TimeElapsed+=Time.deltaTime;
				}
			}
			else
			{
				_Progression = _To;

				_Animating = false;
				_TimeElapsed = 0f;
				_ProgressPct = 1f;

				if (callCallBack && _Callback != null)
				{
					_Callback.Invoke();
				}
			}
		}
	}

	public void update(ref Vector2 whatToTween)
	{
		bool wasAnimating = _Animating;
		update(false);
		whatToTween = _Progression;

		if (wasAnimating && !_Animating && _Callback != null)
		{
			_Callback.Invoke();
		}

	}

	public bool animating
	{
		get
		{
			return _Animating;
		}
	}

	public Vector3 progression
	{
		get
		{
			return _Progression;
		}
	}

	public Vector3 from
	{
		get
		{
			return _From;
		}
		set
		{
			_From = value;
		}
	}

	public Vector3 to
	{
		get
		{
			return _To;
		}
		set
		{
			_To = value;
		}
	}
}
