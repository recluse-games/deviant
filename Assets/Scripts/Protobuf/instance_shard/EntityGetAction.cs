// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: EntityGetAction.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Deviant {

  /// <summary>Holder for reflection information generated from EntityGetAction.proto</summary>
  public static partial class EntityGetActionReflection {

    #region Descriptor
    /// <summary>File descriptor for EntityGetAction.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static EntityGetActionReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChVFbnRpdHlHZXRBY3Rpb24ucHJvdG8SB0RldmlhbnQiIwoPRW50aXR5R2V0",
            "QWN0aW9uEhAKCFBsYXllcklkGAEgASgJQgtaCS47ZGV2aWFudGIGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Deviant.EntityGetAction), global::Deviant.EntityGetAction.Parser, new[]{ "PlayerId" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class EntityGetAction : pb::IMessage<EntityGetAction> {
    private static readonly pb::MessageParser<EntityGetAction> _parser = new pb::MessageParser<EntityGetAction>(() => new EntityGetAction());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<EntityGetAction> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Deviant.EntityGetActionReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public EntityGetAction() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public EntityGetAction(EntityGetAction other) : this() {
      playerId_ = other.playerId_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public EntityGetAction Clone() {
      return new EntityGetAction(this);
    }

    /// <summary>Field number for the "PlayerId" field.</summary>
    public const int PlayerIdFieldNumber = 1;
    private string playerId_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string PlayerId {
      get { return playerId_; }
      set {
        playerId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as EntityGetAction);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(EntityGetAction other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (PlayerId != other.PlayerId) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (PlayerId.Length != 0) hash ^= PlayerId.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (PlayerId.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(PlayerId);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (PlayerId.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(PlayerId);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(EntityGetAction other) {
      if (other == null) {
        return;
      }
      if (other.PlayerId.Length != 0) {
        PlayerId = other.PlayerId;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            PlayerId = input.ReadString();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
