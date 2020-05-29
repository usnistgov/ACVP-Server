import { Dependency } from '../../dependency/Dependency';

export class OperatingEnvironmentCreatePayload {
  id: number;
  url: string;
  type: string;
  name: string;
  description: string;

  dependencyUrls: string[];
  existingDependencies: Dependency[];

  // Just as a result of the incoming naming convention, the "new" deps are here, and the existing ones are in
  // an array above called "existingDependencies"
  dependencies: Dependency[];
}
