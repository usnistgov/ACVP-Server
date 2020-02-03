import { Attribute } from './attribute';

export class Dependency {

  id: number;
  name: string;
  type: string;
  description: string;
  attributes: Attribute[];

  public DependencyLite(id: number,
    name: string,
    type: string,
    description: string,
    attributes: Attribute[]) {
    this.id = id;
    this.name = name;
    this.type = type;
    this.description = description;
    this.attributes = attributes;
  };

}
