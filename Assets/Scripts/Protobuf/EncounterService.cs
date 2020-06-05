// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: EncounterService.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Deviant {

  /// <summary>Holder for reflection information generated from EncounterService.proto</summary>
  public static partial class EncounterServiceReflection {

    #region Descriptor
    /// <summary>File descriptor for EncounterService.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static EncounterServiceReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChZFbmNvdW50ZXJTZXJ2aWNlLnByb3RvEgdEZXZpYW50Gg9FbmNvdW50ZXIu",
            "cHJvdG8aF0VudGl0eUFjdGlvbk5hbWVzLnByb3RvGhZFbnRpdHlNb3ZlQWN0",
            "aW9uLnByb3RvGhZFbnRpdHlQbGF5QWN0aW9uLnByb3RvGhhFbnRpdHlUYXJn",
            "ZXRBY3Rpb24ucHJvdG8aF0VudGl0eVN0YXRlQWN0aW9uLnByb3RvGhVFbnRp",
            "dHlHZXRBY3Rpb24ucHJvdG8aG0VuY291bnRlckNyZWF0ZUFjdGlvbi5wcm90",
            "bxoYRW50aXR5Um90YXRlQWN0aW9uLnByb3RvIoYEChBFbmNvdW50ZXJSZXF1",
            "ZXN0EhAKCHBsYXllcklkGAEgASgJEiUKCWVuY291bnRlchgCIAEoCzISLkRl",
            "dmlhbnQuRW5jb3VudGVyEjQKEGVudGl0eUFjdGlvbk5hbWUYAyABKA4yGi5E",
            "ZXZpYW50LkVudGl0eUFjdGlvbk5hbWVzEjMKEGVudGl0eU1vdmVBY3Rpb24Y",
            "BCABKAsyGS5EZXZpYW50LkVudGl0eU1vdmVBY3Rpb24SMwoQZW50aXR5UGxh",
            "eUFjdGlvbhgFIAEoCzIZLkRldmlhbnQuRW50aXR5UGxheUFjdGlvbhI3ChJl",
            "bnRpdHlUYXJnZXRBY3Rpb24YBiABKAsyGy5EZXZpYW50LkVudGl0eVRhcmdl",
            "dEFjdGlvbhI1ChFlbnRpdHlTdGF0ZUFjdGlvbhgHIAEoCzIaLkRldmlhbnQu",
            "RW50aXR5U3RhdGVBY3Rpb24SMQoPZW50aXR5R2V0QWN0aW9uGAggASgLMhgu",
            "RGV2aWFudC5FbnRpdHlHZXRBY3Rpb24SPQoVZW5jb3VudGVyQ3JlYXRlQWN0",
            "aW9uGAkgASgLMh4uRGV2aWFudC5FbmNvdW50ZXJDcmVhdGVBY3Rpb24SNwoS",
            "ZW50aXR5Um90YXRlQWN0aW9uGAogASgLMhsuRGV2aWFudC5FbnRpdHlSb3Rh",
            "dGVBY3Rpb24iTAoRRW5jb3VudGVyUmVzcG9uc2USEAoIcGxheWVySWQYASAB",
            "KAkSJQoJZW5jb3VudGVyGAIgASgLMhIuRGV2aWFudC5FbmNvdW50ZXIyYAoQ",
            "RW5jb3VudGVyU2VydmljZRJMCg9VcGRhdGVFbmNvdW50ZXISGS5EZXZpYW50",
            "LkVuY291bnRlclJlcXVlc3QaGi5EZXZpYW50LkVuY291bnRlclJlc3BvbnNl",
            "KAEwAUIJWgdkZXZpYW50YgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Deviant.EncounterReflection.Descriptor, global::Deviant.EntityActionNamesReflection.Descriptor, global::Deviant.EntityMoveActionReflection.Descriptor, global::Deviant.EntityPlayActionReflection.Descriptor, global::Deviant.EntityTargetActionReflection.Descriptor, global::Deviant.EntityStateActionReflection.Descriptor, global::Deviant.EntityGetActionReflection.Descriptor, global::Deviant.EncounterCreateActionReflection.Descriptor, global::Deviant.EntityRotateActionReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Deviant.EncounterRequest), global::Deviant.EncounterRequest.Parser, new[]{ "PlayerId", "Encounter", "EntityActionName", "EntityMoveAction", "EntityPlayAction", "EntityTargetAction", "EntityStateAction", "EntityGetAction", "EncounterCreateAction", "EntityRotateAction" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Deviant.EncounterResponse), global::Deviant.EncounterResponse.Parser, new[]{ "PlayerId", "Encounter" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  /// The request message containing the user's name.
  /// </summary>
  public sealed partial class EncounterRequest : pb::IMessage<EncounterRequest> {
    private static readonly pb::MessageParser<EncounterRequest> _parser = new pb::MessageParser<EncounterRequest>(() => new EncounterRequest());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<EncounterRequest> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Deviant.EncounterServiceReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public EncounterRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public EncounterRequest(EncounterRequest other) : this() {
      playerId_ = other.playerId_;
      encounter_ = other.encounter_ != null ? other.encounter_.Clone() : null;
      entityActionName_ = other.entityActionName_;
      entityMoveAction_ = other.entityMoveAction_ != null ? other.entityMoveAction_.Clone() : null;
      entityPlayAction_ = other.entityPlayAction_ != null ? other.entityPlayAction_.Clone() : null;
      entityTargetAction_ = other.entityTargetAction_ != null ? other.entityTargetAction_.Clone() : null;
      entityStateAction_ = other.entityStateAction_ != null ? other.entityStateAction_.Clone() : null;
      entityGetAction_ = other.entityGetAction_ != null ? other.entityGetAction_.Clone() : null;
      encounterCreateAction_ = other.encounterCreateAction_ != null ? other.encounterCreateAction_.Clone() : null;
      entityRotateAction_ = other.entityRotateAction_ != null ? other.entityRotateAction_.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public EncounterRequest Clone() {
      return new EncounterRequest(this);
    }

    /// <summary>Field number for the "playerId" field.</summary>
    public const int PlayerIdFieldNumber = 1;
    private string playerId_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string PlayerId {
      get { return playerId_; }
      set {
        playerId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "encounter" field.</summary>
    public const int EncounterFieldNumber = 2;
    private global::Deviant.Encounter encounter_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Deviant.Encounter Encounter {
      get { return encounter_; }
      set {
        encounter_ = value;
      }
    }

    /// <summary>Field number for the "entityActionName" field.</summary>
    public const int EntityActionNameFieldNumber = 3;
    private global::Deviant.EntityActionNames entityActionName_ = global::Deviant.EntityActionNames.Play;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Deviant.EntityActionNames EntityActionName {
      get { return entityActionName_; }
      set {
        entityActionName_ = value;
      }
    }

    /// <summary>Field number for the "entityMoveAction" field.</summary>
    public const int EntityMoveActionFieldNumber = 4;
    private global::Deviant.EntityMoveAction entityMoveAction_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Deviant.EntityMoveAction EntityMoveAction {
      get { return entityMoveAction_; }
      set {
        entityMoveAction_ = value;
      }
    }

    /// <summary>Field number for the "entityPlayAction" field.</summary>
    public const int EntityPlayActionFieldNumber = 5;
    private global::Deviant.EntityPlayAction entityPlayAction_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Deviant.EntityPlayAction EntityPlayAction {
      get { return entityPlayAction_; }
      set {
        entityPlayAction_ = value;
      }
    }

    /// <summary>Field number for the "entityTargetAction" field.</summary>
    public const int EntityTargetActionFieldNumber = 6;
    private global::Deviant.EntityTargetAction entityTargetAction_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Deviant.EntityTargetAction EntityTargetAction {
      get { return entityTargetAction_; }
      set {
        entityTargetAction_ = value;
      }
    }

    /// <summary>Field number for the "entityStateAction" field.</summary>
    public const int EntityStateActionFieldNumber = 7;
    private global::Deviant.EntityStateAction entityStateAction_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Deviant.EntityStateAction EntityStateAction {
      get { return entityStateAction_; }
      set {
        entityStateAction_ = value;
      }
    }

    /// <summary>Field number for the "entityGetAction" field.</summary>
    public const int EntityGetActionFieldNumber = 8;
    private global::Deviant.EntityGetAction entityGetAction_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Deviant.EntityGetAction EntityGetAction {
      get { return entityGetAction_; }
      set {
        entityGetAction_ = value;
      }
    }

    /// <summary>Field number for the "encounterCreateAction" field.</summary>
    public const int EncounterCreateActionFieldNumber = 9;
    private global::Deviant.EncounterCreateAction encounterCreateAction_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Deviant.EncounterCreateAction EncounterCreateAction {
      get { return encounterCreateAction_; }
      set {
        encounterCreateAction_ = value;
      }
    }

    /// <summary>Field number for the "entityRotateAction" field.</summary>
    public const int EntityRotateActionFieldNumber = 10;
    private global::Deviant.EntityRotateAction entityRotateAction_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Deviant.EntityRotateAction EntityRotateAction {
      get { return entityRotateAction_; }
      set {
        entityRotateAction_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as EncounterRequest);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(EncounterRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (PlayerId != other.PlayerId) return false;
      if (!object.Equals(Encounter, other.Encounter)) return false;
      if (EntityActionName != other.EntityActionName) return false;
      if (!object.Equals(EntityMoveAction, other.EntityMoveAction)) return false;
      if (!object.Equals(EntityPlayAction, other.EntityPlayAction)) return false;
      if (!object.Equals(EntityTargetAction, other.EntityTargetAction)) return false;
      if (!object.Equals(EntityStateAction, other.EntityStateAction)) return false;
      if (!object.Equals(EntityGetAction, other.EntityGetAction)) return false;
      if (!object.Equals(EncounterCreateAction, other.EncounterCreateAction)) return false;
      if (!object.Equals(EntityRotateAction, other.EntityRotateAction)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (PlayerId.Length != 0) hash ^= PlayerId.GetHashCode();
      if (encounter_ != null) hash ^= Encounter.GetHashCode();
      if (EntityActionName != global::Deviant.EntityActionNames.Play) hash ^= EntityActionName.GetHashCode();
      if (entityMoveAction_ != null) hash ^= EntityMoveAction.GetHashCode();
      if (entityPlayAction_ != null) hash ^= EntityPlayAction.GetHashCode();
      if (entityTargetAction_ != null) hash ^= EntityTargetAction.GetHashCode();
      if (entityStateAction_ != null) hash ^= EntityStateAction.GetHashCode();
      if (entityGetAction_ != null) hash ^= EntityGetAction.GetHashCode();
      if (encounterCreateAction_ != null) hash ^= EncounterCreateAction.GetHashCode();
      if (entityRotateAction_ != null) hash ^= EntityRotateAction.GetHashCode();
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
      if (encounter_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(Encounter);
      }
      if (EntityActionName != global::Deviant.EntityActionNames.Play) {
        output.WriteRawTag(24);
        output.WriteEnum((int) EntityActionName);
      }
      if (entityMoveAction_ != null) {
        output.WriteRawTag(34);
        output.WriteMessage(EntityMoveAction);
      }
      if (entityPlayAction_ != null) {
        output.WriteRawTag(42);
        output.WriteMessage(EntityPlayAction);
      }
      if (entityTargetAction_ != null) {
        output.WriteRawTag(50);
        output.WriteMessage(EntityTargetAction);
      }
      if (entityStateAction_ != null) {
        output.WriteRawTag(58);
        output.WriteMessage(EntityStateAction);
      }
      if (entityGetAction_ != null) {
        output.WriteRawTag(66);
        output.WriteMessage(EntityGetAction);
      }
      if (encounterCreateAction_ != null) {
        output.WriteRawTag(74);
        output.WriteMessage(EncounterCreateAction);
      }
      if (entityRotateAction_ != null) {
        output.WriteRawTag(82);
        output.WriteMessage(EntityRotateAction);
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
      if (encounter_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Encounter);
      }
      if (EntityActionName != global::Deviant.EntityActionNames.Play) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) EntityActionName);
      }
      if (entityMoveAction_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(EntityMoveAction);
      }
      if (entityPlayAction_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(EntityPlayAction);
      }
      if (entityTargetAction_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(EntityTargetAction);
      }
      if (entityStateAction_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(EntityStateAction);
      }
      if (entityGetAction_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(EntityGetAction);
      }
      if (encounterCreateAction_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(EncounterCreateAction);
      }
      if (entityRotateAction_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(EntityRotateAction);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(EncounterRequest other) {
      if (other == null) {
        return;
      }
      if (other.PlayerId.Length != 0) {
        PlayerId = other.PlayerId;
      }
      if (other.encounter_ != null) {
        if (encounter_ == null) {
          Encounter = new global::Deviant.Encounter();
        }
        Encounter.MergeFrom(other.Encounter);
      }
      if (other.EntityActionName != global::Deviant.EntityActionNames.Play) {
        EntityActionName = other.EntityActionName;
      }
      if (other.entityMoveAction_ != null) {
        if (entityMoveAction_ == null) {
          EntityMoveAction = new global::Deviant.EntityMoveAction();
        }
        EntityMoveAction.MergeFrom(other.EntityMoveAction);
      }
      if (other.entityPlayAction_ != null) {
        if (entityPlayAction_ == null) {
          EntityPlayAction = new global::Deviant.EntityPlayAction();
        }
        EntityPlayAction.MergeFrom(other.EntityPlayAction);
      }
      if (other.entityTargetAction_ != null) {
        if (entityTargetAction_ == null) {
          EntityTargetAction = new global::Deviant.EntityTargetAction();
        }
        EntityTargetAction.MergeFrom(other.EntityTargetAction);
      }
      if (other.entityStateAction_ != null) {
        if (entityStateAction_ == null) {
          EntityStateAction = new global::Deviant.EntityStateAction();
        }
        EntityStateAction.MergeFrom(other.EntityStateAction);
      }
      if (other.entityGetAction_ != null) {
        if (entityGetAction_ == null) {
          EntityGetAction = new global::Deviant.EntityGetAction();
        }
        EntityGetAction.MergeFrom(other.EntityGetAction);
      }
      if (other.encounterCreateAction_ != null) {
        if (encounterCreateAction_ == null) {
          EncounterCreateAction = new global::Deviant.EncounterCreateAction();
        }
        EncounterCreateAction.MergeFrom(other.EncounterCreateAction);
      }
      if (other.entityRotateAction_ != null) {
        if (entityRotateAction_ == null) {
          EntityRotateAction = new global::Deviant.EntityRotateAction();
        }
        EntityRotateAction.MergeFrom(other.EntityRotateAction);
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
          case 18: {
            if (encounter_ == null) {
              Encounter = new global::Deviant.Encounter();
            }
            input.ReadMessage(Encounter);
            break;
          }
          case 24: {
            EntityActionName = (global::Deviant.EntityActionNames) input.ReadEnum();
            break;
          }
          case 34: {
            if (entityMoveAction_ == null) {
              EntityMoveAction = new global::Deviant.EntityMoveAction();
            }
            input.ReadMessage(EntityMoveAction);
            break;
          }
          case 42: {
            if (entityPlayAction_ == null) {
              EntityPlayAction = new global::Deviant.EntityPlayAction();
            }
            input.ReadMessage(EntityPlayAction);
            break;
          }
          case 50: {
            if (entityTargetAction_ == null) {
              EntityTargetAction = new global::Deviant.EntityTargetAction();
            }
            input.ReadMessage(EntityTargetAction);
            break;
          }
          case 58: {
            if (entityStateAction_ == null) {
              EntityStateAction = new global::Deviant.EntityStateAction();
            }
            input.ReadMessage(EntityStateAction);
            break;
          }
          case 66: {
            if (entityGetAction_ == null) {
              EntityGetAction = new global::Deviant.EntityGetAction();
            }
            input.ReadMessage(EntityGetAction);
            break;
          }
          case 74: {
            if (encounterCreateAction_ == null) {
              EncounterCreateAction = new global::Deviant.EncounterCreateAction();
            }
            input.ReadMessage(EncounterCreateAction);
            break;
          }
          case 82: {
            if (entityRotateAction_ == null) {
              EntityRotateAction = new global::Deviant.EntityRotateAction();
            }
            input.ReadMessage(EntityRotateAction);
            break;
          }
        }
      }
    }

  }

  /// <summary>
  /// The response message containing the greetings
  /// </summary>
  public sealed partial class EncounterResponse : pb::IMessage<EncounterResponse> {
    private static readonly pb::MessageParser<EncounterResponse> _parser = new pb::MessageParser<EncounterResponse>(() => new EncounterResponse());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<EncounterResponse> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Deviant.EncounterServiceReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public EncounterResponse() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public EncounterResponse(EncounterResponse other) : this() {
      playerId_ = other.playerId_;
      encounter_ = other.encounter_ != null ? other.encounter_.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public EncounterResponse Clone() {
      return new EncounterResponse(this);
    }

    /// <summary>Field number for the "playerId" field.</summary>
    public const int PlayerIdFieldNumber = 1;
    private string playerId_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string PlayerId {
      get { return playerId_; }
      set {
        playerId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "encounter" field.</summary>
    public const int EncounterFieldNumber = 2;
    private global::Deviant.Encounter encounter_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Deviant.Encounter Encounter {
      get { return encounter_; }
      set {
        encounter_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as EncounterResponse);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(EncounterResponse other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (PlayerId != other.PlayerId) return false;
      if (!object.Equals(Encounter, other.Encounter)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (PlayerId.Length != 0) hash ^= PlayerId.GetHashCode();
      if (encounter_ != null) hash ^= Encounter.GetHashCode();
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
      if (encounter_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(Encounter);
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
      if (encounter_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Encounter);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(EncounterResponse other) {
      if (other == null) {
        return;
      }
      if (other.PlayerId.Length != 0) {
        PlayerId = other.PlayerId;
      }
      if (other.encounter_ != null) {
        if (encounter_ == null) {
          Encounter = new global::Deviant.Encounter();
        }
        Encounter.MergeFrom(other.Encounter);
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
          case 18: {
            if (encounter_ == null) {
              Encounter = new global::Deviant.Encounter();
            }
            input.ReadMessage(Encounter);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
