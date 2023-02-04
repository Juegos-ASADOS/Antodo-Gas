// Copyright (c) coherence ApS.
// For all coherence generated code, the coherence SDK license terms apply. See the license file in the coherence Package root folder for more information.

// <auto-generated>
// Generated file. DO NOT EDIT!
// </auto-generated>
namespace Coherence.Generated
{
	using Coherence.ProtocolDef;
	using Coherence.Serializer;
	using Coherence.SimulationFrame;
	using Coherence.Entity;
	using Coherence.Utils;
	using Coherence.Brook;
	using Coherence.Toolkit;
	using UnityEngine;

	public struct LocalUser : ICoherenceComponentData
	{
		public int localIndex;

		public override string ToString()
		{
			return $"LocalUser(localIndex: {localIndex})";
		}

		public uint GetComponentType() => Definition.InternalLocalUser;

		public const int order = 0;

		public int GetComponentOrder() => order;

		public AbsoluteSimulationFrame Frame;
	
		private static readonly int _localIndex_Min = -2147483648;
		private static readonly int _localIndex_Max = 2147483647;

		public void SetSimulationFrame(AbsoluteSimulationFrame frame)
		{
			Frame = frame;
		}

		public AbsoluteSimulationFrame GetSimulationFrame() => Frame;

		public ICoherenceComponentData MergeWith(ICoherenceComponentData data, uint mask)
		{
			var other = (LocalUser)data;
			if ((mask & 0x01) != 0)
			{
				Frame = other.Frame;
				localIndex = other.localIndex;
			}
			mask >>= 1;
			return this;
		}

		public static void Serialize(LocalUser data, uint mask, IOutProtocolBitStream bitStream)
		{
			if (bitStream.WriteMask((mask & 0x01) != 0))
			{
				Coherence.Utils.Bounds.Check(data.localIndex, _localIndex_Min, _localIndex_Max, "LocalUser.localIndex");
				data.localIndex = Coherence.Utils.Bounds.Clamp(data.localIndex, _localIndex_Min, _localIndex_Max);
				bitStream.WriteIntegerRange(data.localIndex, 32, -2147483648);
			}
			mask >>= 1;
		}

		public static (LocalUser, uint, uint?) Deserialize(InProtocolBitStream bitStream)
		{
			var mask = (uint)0;
			var val = new LocalUser();
	
			if (bitStream.ReadMask())
			{
				val.localIndex = bitStream.ReadIntegerRange(32, -2147483648);
				mask |= 0b00000000000000000000000000000001;
			}
			return (val, mask, null);
		}

		/// <summary>
		/// Resets byte array references to the local array instance that is kept in the lastSentData.
		/// If the array content has changed but remains of same length, the new content is copied into the local array instance.
		/// If the array length has changed, the array is cloned and overwrites the local instance.
		/// If the array has not changed, the reference is reset to the local array instance.
		/// Otherwise, changes to other fields on the component might cause the local array instance reference to become permanently lost.
		/// </summary>
		public void ResetByteArrays(ICoherenceComponentData lastSent, uint mask)
		{
			var last = lastSent as LocalUser?;
	
		}
	}
}