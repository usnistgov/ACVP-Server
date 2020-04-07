import { Dependency } from '../../dependency/dependency';

export class OperatingEnvironmentCreatePayload {
  id: number;
  url: string;
  type: string;
  name: string;
  description: string;
  dependenciesToCreate: Dependency[];
  dependencies: Dependency[];
}
