using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models
{
	public class WorkflowItemPayloadConverter : JsonConverter<IWorkflowItemPayload>
	{
		public override IWorkflowItemPayload Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			throw new NotImplementedException();
		}

		public override void Write(Utf8JsonWriter writer, IWorkflowItemPayload value, JsonSerializerOptions options)
		{
			switch (value)
			{
				case CertifyTestSessionPayload p: JsonSerializer.Serialize(writer, p, typeof(CertifyTestSessionPayload), options); break;
				case DeletePayload p: JsonSerializer.Serialize(writer, p, typeof(DeletePayload), options); break;
				case DependencyCreatePayload p: JsonSerializer.Serialize(writer, p, typeof(DependencyCreatePayload), options); break;
				case DependencyUpdatePayload p: JsonSerializer.Serialize(writer, p, typeof(DependencyUpdatePayload), options); break;
				case ImplementationCreatePayload p: JsonSerializer.Serialize(writer, p, typeof(ImplementationCreatePayload), options); break;
				case ImplementationUpdatePayload p: JsonSerializer.Serialize(writer, p, typeof(ImplementationUpdatePayload), options); break;
				case OECreatePayload p: JsonSerializer.Serialize(writer, p, typeof(OECreatePayload), options); break;
				case OEUpdatePayload p: JsonSerializer.Serialize(writer, p, typeof(OEUpdatePayload), options); break;
				case OrganizationCreatePayload p: JsonSerializer.Serialize(writer, p, typeof(OrganizationCreatePayload), options); break;
				case OrganizationUpdatePayload p: JsonSerializer.Serialize(writer, p, typeof(OrganizationUpdatePayload), options); break;
				case PersonCreatePayload p: JsonSerializer.Serialize(writer, p, typeof(PersonCreatePayload), options); break;
				case PersonUpdatePayload p: JsonSerializer.Serialize(writer, p, typeof(PersonUpdatePayload), options); break;
			}
		}
	}
}
