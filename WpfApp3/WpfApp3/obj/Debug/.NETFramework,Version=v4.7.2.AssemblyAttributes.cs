 BoneIndex = Input.BoneIndex[i];
			uint OffsetIndex = BoneIndex + JointsInstanceIndex;

			float4x4 VertexMatrix = PdxMeshGetJointVertexMatrix( OffsetIndex );

			SkinnedPosition += mul( VertexMatrix, Position ) *