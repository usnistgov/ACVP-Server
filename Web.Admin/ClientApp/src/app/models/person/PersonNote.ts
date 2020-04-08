export class PersonNote {

  id: number;
  subject: string;
  body: string;
  time: string;

  public PersonNote(id: number,
      subject: string,
      body: string,
      time: string) {
    this.id = id;
    this.subject = subject;
    this.body = body;
    this.time = time;
  };

}
