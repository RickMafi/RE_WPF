

// === Shader Info === 

// Shader file: gfx/FX/pdxmesh.shader
// Effect: snap_to_terrain_atlasShadow_mapobject



// === Defines === 

#define PDX_DIRECTX_11
#define VERTEX_SHADER
#define PDX_HLSL
#define VENDOR_NVIDIA
#define JOMINI_MAP_OBJECT
#define PDX_MESH_UV1
#define PDX_MESH_SNAP_VERTICES_TO_TERRAIN
#define PDX_MAX_HEIGHTMAP_COMPRESS_LEVELS 5
#define JOMINI_REFRACTION_ENABLED
#define PDX_WINDOWS


// === HLSL Macros === 

#define PDX_POSITION SV_Position
#define PDX_COLOR SV_Target
#define PDX_COLOR0 SV_Target0
#define PDX_COLOR0_SRC1 SV_Target1 // Use this when doing dual source blending, currently only supports that for render target #0
#define PDX_COLOR1 SV_Target1
#define PDX_COLOR2 SV_Target2
#define PDX_COLOR3 SV_Target3
#define PDX_COLOR4 SV_Target4
#define PDX_COLOR5 SV_Target5
#define PDX_COLOR6 SV_Target6
#define PDX_COLOR7 SV_Target7
#define PDX_VertexID SV_VertexID
#define PDX_InstanceID SV_InstanceID
#define PDX_DispatchThreadID SV_DispatchThreadID
#define PDX_GroupThreadID SV_GroupThreadID
#define PDX_GroupID SV_GroupID
#define PDX_GroupIndex SV_GroupIndex
#define PDX_TessFactor SV_TessFactor
#define PDX_InsideTessFactor SV_InsideTessFactor
#define PDX_OutputControlPointID SV_OutputControlPointID
#define PDX_DomainLocation SV_DomainLocation
#define PDX_RenderTargetArrayIndex SV_RenderTargetArrayIndex
#define PDX_ViewportArrayIndex SV_ViewportArrayIndex

#define PdxDomainTypeTriangle "tri"
#define PdxDomainTypeQuad "quad"
#define PdxDomainTypeIsoline "isoline"

#define PdxPartitioningModeInteger "integer"
#define PdxPartitioningModeFractionalEven "fractional_even"
#define PdxPartitioningModeFractionalOdd "fractional_odd"

#define PdxPrimitiveTypePoint point
#define PdxPrimitiveTypeLine line
#define PdxPrimitiveTypeTriangle triangle
#define PdxPrimitiveTypeLineAdjacency lineadj
#define PdxPrimitiveTypeTriangleAdjacency triangleadj

#define PdxTessellatorOutputTopologyPoint "point"
#define PdxTessellatorOutputTopologyLine "line"
#define PdxTessellatorOutputTopologyTriangleCw "triangle_cw"
#define PdxTessellatorOutputTopologyTriangleCcw "triangle_ccw"

#define PdxMeshShaderOutputTopologyLine "line"
#define PdxMeshShaderOutputTopologyTriangle "triangle"

#define mod( X, Y ) ( (X) % (Y) )

float2x2 Create2x2( in float2 x, in float2 y )
{
	return transpose( float2x2( x, y ) );
}
// TODO, Create3x3 should be transposed in hlsl, and not in glsl, and then the mul() arguments should be reversed
#define Create3x3 float3x3
float4x4 Create4x4( in float4 x, in float4 y, in float4 z, in float4 w )
{
	return transpose( float4x4( x, y, z, w ) );
}

#define GetMatrixData( Matrix, row, col ) ( Matrix [ row ] [ col ] )

float3x3 CastTo3x3( in float4x4 M )
{
	return (float3x3)M;
}

#define lessThan( a, b ) ( (a) < (b) )

float2 vec2(float vValue) { return float2(vValue, vValue); }
float3 vec3(float vValue) { return float3(vValue, vValue, vValue); }
float4 vec4(float vValue) { return float4(vValue, vValue, vValue, vValue); }


struct PdxTextureSampler2D
{
    Texture2D 		_Texture;
    SamplerState 	_Sampler;
};
struct PdxTextureSampler2DMS
{
    Texture2DMS<float4>		_Texture;
};

struct PdxTextureSampler2DArray
{
    Texture2DArray	_Texture;
    SamplerState 	_Sampler;
};

struct PdxTextureSampler3D
{
    Texture3D 		_Texture;
    SamplerState 	_Sampler;
};

struct PdxTextureSamplerCube
{
    TextureCube 	_Texture;
    SamplerState 	_Sampler;
};

struct PdxTextureSampler2DCmp
{
    Texture2D 				_Texture;
    SamplerComparisonState 	_Sampler;
};

// These ar