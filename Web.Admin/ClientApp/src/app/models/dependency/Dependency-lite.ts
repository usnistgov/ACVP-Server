export class DependencyLite {

  id: number;
  name: string;
  type: string;
  description: string

  public DependencyLite(id: number,
    name: string,
    type: string,
    description: string) {
    this.id = id;
    this.name = name;
    this.type = type;
    this.description = description;
  };

}
