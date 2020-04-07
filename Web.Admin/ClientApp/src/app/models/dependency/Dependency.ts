import { Attribute } from './Attribute';

export class Dependency {

  id: number;
  name: string;
  type: string;
  description: string;
  attributes: Attribute[];

  // Field only used in OE-Update/OE-Crate workflow items, but it makes no sense
  // to have a whole extra model class, so here we are - RLS 3/9/20
  isInlineCreate: boolean;

  public constructor(id: number,
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
