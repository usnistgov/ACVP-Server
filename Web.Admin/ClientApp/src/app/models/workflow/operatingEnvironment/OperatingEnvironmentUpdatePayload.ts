import { Dependency } from '../../dependency/Dependency';

export class OperatingEnvironmentUpdatePayload {
  id: number;
  url: string;
  type: string;
  name: string;
  description: string;
  dependenciesToCreate: Dependency[];
  dependencies: Dependency[];
}