// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: card_action_status_types.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Deviant {

  /// <summary>Holder for reflection information generated from card_action_status_types.proto</summary>
  public static partial class CardActionStatusTypesReflection {

    #region Descriptor
    /// <summary>File descriptor for card_action_status_types.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static CardActionStatusTypesReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Ch5jYXJkX2FjdGlvbl9zdGF0dXNfdHlwZXMucHJvdG8SB0RldmlhbnQqRwoV",
            "Q2FyZEFjdGlvblN0YXR1c1R5cGVzEgkKBUVNUFRZEAASDQoJVU5CTE9DS0VE",
            "EAESCwoHQkxPQ0tFRBACEgcKA0hJVBADQgtaCS47ZGV2aWFudGIGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(new[] {typeof(global::Deviant.CardActionStatusTypes), }, null, null));
    }
    #endregion

  }
  #region Enums
  public enum CardActionStatusTypes {
    [pbr::OriginalName("EMPTY")] Empty = 0,
    [pbr::OriginalName("UNBLOCKED")] Unblocked = 1,
    [pbr::OriginalName("BLOCKED")] Blocked = 2,
    [pbr::OriginalName("HIT")] Hit = 3,
  }

  #endregion

}

#endregion Designer generated code
