shader_type canvas_item;

uniform float rotation_speed = 8;
uniform float time_multiplier = 2;
uniform float resize_speed = 4;
uniform float min_scale = 0.8;
uniform float max_scale = 1.1;

void fragment() {
    float angle = TIME * rotation_speed * time_multiplier;
    mat2 rotation_matrix = mat2(vec2(cos(angle), -sin(angle)), vec2(sin(angle), cos(angle)));
    vec2 centered_uv = UV - vec2(0.5, 0.5);

    float scale_factor = mix(min_scale, max_scale, (sin(TIME * resize_speed) + 1.0) * 0.5);
    vec2 scaled_uv = centered_uv / scale_factor;
    scaled_uv += vec2(0.5, 0.5);

	vec2 scaled_center = scaled_uv - vec2(0.5, 0.5);
	vec2 rotated_uv = rotation_matrix * scaled_center;
	rotated_uv += vec2(0.5, 0.5);

    COLOR = texture(TEXTURE, rotated_uv);
}
