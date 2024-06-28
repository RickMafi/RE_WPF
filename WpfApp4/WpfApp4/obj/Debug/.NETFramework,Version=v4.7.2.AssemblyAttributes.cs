loat UnpackAndGetMapObjectOpacity( in uint InstanceIndex24_Opacity8 )
{
	const float OpacityScale = 1.0f / float(0x0000007f);
	float Opacity = float(uint(InstanceIndex24_Opacity8 & uint(0x0000007f))) * OpacityScale