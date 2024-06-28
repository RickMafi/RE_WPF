hapeDataInstanced[ VectorIndex ][ ComponentIndex ];
}

uint GetUintAt( uint LookupIndex )
{
	return uint( GetFloatAt( LookupIndex ) );
}

uint CalcLinearBlendBufferIndex( uint VertexIndex, uint VertexDataIndex )
{
	return ( VertexDataIndex * BlendShapeVertexCount + VertexIndex );
}

float3 ReadBlendBufferTextureFloat3( uint AtVectorIndex )
{
	int AtFloat = int( AtVectorIndex ) * 3;
	float X = PdxReadBuffer( BlendShapeDataBuffer, AtFloat );
	float Y = PdxReadBuffer( BlendShapeDataBuffer, AtFloat + 1 );
	float Z = PdxReadBuffer( BlendShapeDataBuffer, AtFloat + 2 );
	
	return float3( X, Y, Z );
}

void ApplyBlendShapes( inout float3 PositionOut, inout float3 NormalOut, inout float3 TangentOut, in uint BlendShapeInstanceIndex, in uint ObjectInstanceIndex, in uint VertexID )
{
	uint VertexIndex = VertexID + BlendShapesVertexOffset;
			
	uint VectorIndex = 0;
	uint VectorElement = 0;

	uint ActiveBlendShapes = GetActiveBlendShapes( ObjectInstanceIndex );

	uint IndicesOffset = BlendShapeInstanceIndex;
	uint WeightsOffset = IndicesOffset + ActiveBlendShapes;

	for (uint CurrentBlendShapeIndex = 0; CurrentBlendShapeIndex < ActiveBlendShapes; ++CurrentBlendShapeIndex) 
	{
		float Weight = GetFloatAt( WeightsOffset + CurrentBlendShapeIndex );
		uint BlendShapeOffsetIndex = GetUintAt( IndicesOffset + CurrentBlendShapeIndex );

		uint VertexDataIndex = BlendShapeOffsetIndex * 3;
			
		PositionOut += ReadBlendBufferTextureFloat3( CalcLinearBlendBufferIndex( VertexIndex, VertexDataIndex ) ).xyz * Weight;
		++VertexDataIndex;
		NormalOut += ReadBlendBufferTextureFloat3( CalcLinearBlendBufferIndex( VertexIndex, VertexDataIndex ) ).xyz * Weight;
		++VertexDataIndex;
		TangentOut += ReadBlendBufferTextureFloat3( CalcLinearBlendBufferIndex( VertexIndex, VertexDataIndex ) ).xyz * Weight;
		++VertexDataIndex;
		++VectorElement;
		if (VectorElement == 4)
		{
			VectorElement = 0;
			++VectorIndex;
		}
	}

	NormalOut = normalize( NormalOut );
	TangentOut = normalize( TangentOut );
}

void ApplyBlendShapesPositionOnly( inout float3 PositionOut, in uint BlendShapeInstanceIndex, in uint ObjectInstanceIndex, in uint VertexID )
{
	uint VertexIndex = VertexID + BlendShapesVertexOffset;
	uint VectorIndex = 0; 
	uint VectorElement = 0;

	uint ActiveBlendShapes = GetActiveBlendShapes( ObjectInstanceIndex );

	uint IndicesOffset = BlendShapeInstanceIndex;
	uint WeightsOffset = IndicesOffset + ActiveBlendShapes;

	for ( uint CurrentBlendShapeIndex = 0; CurrentBlendShapeIndex < ActiveBlendShapes; ++CurrentBlendShapeIndex )
	{
		float Weight = GetFloatAt( WeightsOffset + CurrentBlendShapeIndex );
		uint BlendShapeOffsetIndex = GetUintAt( IndicesOffset + CurrentBlendShapeIndex );

		uint VertexDataIndex = BlendShapeOffsetIndex * 3;

		PositionOut += ReadBlendBufferTextureFloat3( CalcLinearBlendBufferIndex( VertexIndex, VertexDataIndex ) ).xyz * Weight;
		++VectorElement;
		if ( VectorElement == 4 )
		{
			VectorElement = 0;
			++VectorIndex;
		}
	}
}

	struct VS_OUTPUT_PDXMESH
	{
		float4 Position;
		float3 WorldSpacePos;
		float3 Normal;
		float3 Tangent;
		float3 Bitangent;
		float2 UV0;
		float2 UV1;
		float2 UV2;
	};
	
	struct VS_INPUT_PDXMESH
	{
		float3 Position;
		float3 Normal;
		float4 Tangent;
		float2 UV0;
	#ifdef PDX_MESH_UV1
		float2 UV1;
	#endif
	#ifdef PDX_MESH_UV2
		float2 UV2;
	#endif
	#ifdef PDX_MESH_SKINNED
		uint4 BoneIndex;
		float3 BoneWeight;
	#endif
	#ifdef PDX_MESH_BLENDSHAPES
		uint ObjectInstanceIndex;
		uint BlendShapeInstanceIndex;
		uint VertexID;
	#endif
	};
	
	VS_INPU