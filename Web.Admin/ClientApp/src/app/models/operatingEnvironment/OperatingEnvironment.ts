import { DependencyLite } from '../dependency/Dependency-lite';

export class OperatingEnvironment {

  id: number;
  name: string;
  dependencies: DependencyLite[];

  public OperatingEnvironment(id: number,
    name: string,
    dependencies: DependencyLite[]) {
    this.id = id;
    this.name = name;
    this.dependencies = dependencies;
  };

}
