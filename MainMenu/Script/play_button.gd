extends TextureButton



func _on_mouse_entered() -> void:
	$Label.self_modulate=Color.from_rgba8(255,255,255)


func _on_mouse_exited() -> void:
	$Label.self_modulate=Color.from_rgba8(150,150,150)
