﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{

    public float speed = 30;
    private Rigidbody2D rigidBody;

    private AudioSource audioSource;
    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.velocity = Vector2.right * speed;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
		if((col.gameObject.name ==CommonEnum.ObjectNames.LeftPaddle.ToString())||
		col.gameObject.name ==CommonEnum.ObjectNames.RightPaddle.ToString()){
			HandlePaddleHit(col);
		}

		if((col.gameObject.name ==CommonEnum.ObjectNames.WallTop.ToString())||
		col.gameObject.name ==CommonEnum.ObjectNames.WallBottom.ToString()){
			SoundManager.Instance.PlayOneShot(SoundManager.Instance.wallBloop);
		}

		if((col.gameObject.name ==CommonEnum.ObjectNames.LeftGoal.ToString())||
		col.gameObject.name ==CommonEnum.ObjectNames.RightGoal.ToString()){
			SoundManager.Instance.PlayOneShot(SoundManager.Instance.goalBloop);
			if(col.gameObject.name ==CommonEnum.ObjectNames.LeftGoal.ToString())
				IncreaseTextUIScore("RightScoreUI");
			else
				IncreaseTextUIScore("LeftScoreUI");
			transform.position = new Vector2(0,0);
		}
    }

	void HandlePaddleHit(Collision2D col){
		float y = BallHitPaddleWhere(transform.position, 
			col.transform.position,
			col.collider.bounds.size.y);
		Vector2 dir= new Vector2();
		if(col.gameObject.name==CommonEnum.ObjectNames.LeftPaddle.ToString()){
			dir = new Vector2(1,y).normalized;
		}
		if(col.gameObject.name==CommonEnum.ObjectNames.RightPaddle.ToString()){
			dir = new Vector2(-1,y).normalized;
		}
		rigidBody.velocity = dir * speed;
		SoundManager.Instance.PlayOneShot(SoundManager.Instance.hitPaddleBloop);
	}

	float BallHitPaddleWhere(Vector2 ball, Vector2 paddle, float paddleHeight){
		return (ball.y - paddle.y)/paddleHeight;
	}

	void IncreaseTextUIScore(string textUIName){
		var textUIComp = GameObject.Find(textUIName).GetComponent<Text>();

		int score = int.Parse(textUIComp.text);
		score++;
		textUIComp.text = score.ToString();
	}
}
