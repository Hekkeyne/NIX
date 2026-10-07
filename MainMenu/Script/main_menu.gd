extends Node2D

func _process(delta: float) -> void:
	$Label.text="Speed/Скорость: %s \nPosition/Позиция: %s" % [$FatherCharacter.velocity,$FatherCharacter.global_position]
