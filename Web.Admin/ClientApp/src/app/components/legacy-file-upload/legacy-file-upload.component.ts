import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { HttpEventType, HttpClient } from '@angular/common/http';
import { LegacyResult } from '../../models/Legacy/legacyResult';

@Component({
  selector: 'app-legacy-file-upload',
  templateUrl: './legacy-file-upload.component.html',
  styleUrls: ['./legacy-file-upload.component.scss']
})
export class LegacyFileUploadComponent implements OnInit {

  public progress: number;
  public message: string;
  public result: LegacyResult;
  public fileName: string;
  public uploadEnabled: boolean = true;
  @Output() public onUploadFinished = new EventEmitter();

  constructor(private http: HttpClient) { }

  ngOnInit() {
  }

  public uploadFile = (files) => {
    this.uploadEnabled = false;
    this.message = null;
    this.progress = 0;
    this.result = null;

    if (files.length === 0) {
      this.message = "No file selected";
      this.uploadEnabled = true;
      return;
    }

    if (files.length > 1) {
      this.message = "Only a single file may be uploaded";
      this.uploadEnabled = true;
      return;
    }

    let fileToUpload = <File>files[0];
    this.fileName = fileToUpload.name;

    if (!this.fileName.endsWith(".zip")) {
      this.message = "File must be a zip file";
      this.uploadEnabled = true;
      return;
    }

    if (fileToUpload.size > 536870912) {
      this.message = "Files larger than 500MB cannot be uploaded";
      this.uploadEnabled = true;
      return;
    }


    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);

    this.http.post<LegacyResult>('/api/Legacy/Upload', formData, { reportProgress: true, observe: 'events' })
      .subscribe(event => {
        if (event.type === HttpEventType.UploadProgress)
          this.progress = Math.round(100 * event.loaded / event.total);
        else if (event.type === HttpEventType.Response) {
          this.result = event.body;
          this.uploadEnabled = true;
        }
      });
  } 

}
