shader_type spatial;

uniform float speed = 1.0; // Speed of the arrow texture movement
uniform vec2 tex_scale = vec2(1.0);
uniform vec2 tex_offset = vec2(0.0);
uniform sampler2D TEXTURE : filter_linear_mipmap, repeat_enable;

uniform vec4 emission : source_color;
uniform float emission_energy = 1.;
uniform float emission_energy_variance: hint_range(0., 16) = 0.5;
uniform float fluctuating_speed: hint_range(1., 30.) = 1;

vec2 scale(vec2 uv, float x, float y)
{
	//scale function taken from godotshader.com snippets page
	//https://godotshaders.com/snippet/scale/
	
	mat2 scale = mat2(vec2(x, 0.0), vec2(0.0, y));
	
	uv -= 0.5;
	uv = uv * scale;
	uv += 0.5;
	return uv;
}

void fragment() {
	vec2 uv = vec2(1.0 - UV.y, UV.x);
	uv = scale(uv, tex_scale.x, tex_scale.y) + (tex_offset * 0.5); 
    vec2 uv_shift = vec2(-TIME * speed, 0.0);
    vec2 shifted_uv = uv + uv_shift;
	vec3 tex_uv = texture(TEXTURE,  uv).xyz; 
    ALBEDO = texture(TEXTURE, shifted_uv).rgb;
	
	vec3 emission_tex = texture(TEXTURE, shifted_uv).rgb;
	float current_energy = emission_energy + emission_energy_variance * sin(fluctuating_speed * TIME);
	EMISSION = (emission.rgb+emission_tex)*current_energy;
}
