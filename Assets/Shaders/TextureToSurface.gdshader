// taken from https://godotshaders.com/shader/cast-texture-to-surface/
shader_type spatial;

uniform sampler2D tex : filter_linear_mipmap, repeat_enable; 
//texture to be casted. Change to repeat_disable if needed
uniform vec2 tex_offset = vec2(0.0);
//texture image offset
uniform vec2 tex_scale = vec2(1.0);
//texture image scale
uniform vec3 tex_color_shift : source_color = vec3(1.0);
//color to be mixed with texture image

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
	//rotate UV correctly (change if image is facing wrong way)
	uv = scale(uv, tex_scale.x, tex_scale.y) + (tex_offset * 0.5); 
	//apply scale and offset
	vec3 tex_uv = texture(tex,  uv).xyz; 
	//extract color from texture on uv cordinates
	ALBEDO = tex_uv * tex_color_shift;
	//apply mix of extracted color and tex_color_shift color
}
