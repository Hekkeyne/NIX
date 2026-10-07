extends CharacterBody2D

var speed: float = 300.0

func _physics_process(delta: float) -> void:
	var direction := Input.get_vector("left","right","up","down")
	if direction:
		velocity.x = direction.x * speed
		velocity.y=direction.y*speed
		if velocity.x<0:
			$Sprite2D.flip_h=true
		elif velocity.x>0:
			$Sprite2D.flip_h=false
	else:
		velocity.x=move_toward(velocity.x,0,speed)
		velocity.y=move_toward(velocity.y,0,speed)
	velocity=velocity.normalized()*speed
	move_and_slide()
