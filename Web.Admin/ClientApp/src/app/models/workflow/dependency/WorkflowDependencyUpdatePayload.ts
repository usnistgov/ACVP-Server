export class WorkflowDependencyUpdatePayload {

  id: number;
  url: string;

  type: string;
  name: string;
  description: string

  typeUpdated: boolean;
  nameUpdated: boolean;
  descriptionUpdated: boolean;
  attributesUpdated: boolean;

}
