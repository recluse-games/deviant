// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: EntityStateNames.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Deviant {

  /// <summary>Holder for reflection information generated from EntityStateNames.proto</summary>
  public static partial class EntityStateNamesReflection {

    #region Descriptor
    /// <summary>File descriptor for EntityStateNames.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static EntityStateNamesReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChZFbnRpdHlTdGF0ZU5hbWVzLnByb3RvEgdEZXZpYW50KngKEEVudGl0eVN0",
            "YXRlTmFtZXMSCAoESURMRRAAEgoKBk1PVklORxABEg0KCUFUVEFDS0lORxAC",
            "EgsKB0NBU1RJTkcQAxIOCgpSRUNPVkVSSU5HEAQSDQoJUkVDT0lMSU5HEAUS",
            "CQoFRFlJTkcQBhIICgRERUFEEAdCCVoHZGV2aWFudGIGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(new[] {typeof(global::Deviant.EntityStateNames), }, null, null));
    }
    #endregion

  }
  #region Enums
  public enum EntityStateNames {
    [pbr::OriginalName("IDLE")] Idle = 0,
    [pbr::OriginalName("MOVING")] Moving = 1,
    [pbr::OriginalName("ATTACKING")] Attacking = 2,
    [pbr::OriginalName("CASTING")] Casting = 3,
    [pbr::OriginalName("RECOVERING")] Recovering = 4,
    [pbr::OriginalName("RECOILING")] Recoiling = 5,
    [pbr::OriginalName("DYING")] Dying = 6,
    [pbr::OriginalName("DEAD")] Dead = 7,
  }

  #endregion

}

#endregion Designer generated code
