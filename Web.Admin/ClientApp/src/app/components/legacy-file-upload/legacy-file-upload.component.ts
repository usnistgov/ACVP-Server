import { Component, OnInit, Output, EventEmitter } from '@angular/core';
//import { NgxFileDropEntry, FileSystemFileEntry, FileSystemDirectoryEntry } from 'ngx-file-drop';
import { HttpEventType, HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-legacy-file-upload',
  templateUrl: './legacy-file-upload.component.html',
  styleUrls: ['./legacy-file-upload.component.scss']
})
export class LegacyFileUploadComponent implements OnInit {

  public progress: number;
  public message: string;
  @Output() public onUploadFinished = new EventEmitter();

  constructor(private http: HttpClient) { }

  //public files: NgxFileDropEntry[] = [];

  //public dropped(files: NgxFileDropEntry[]) {
  //  this.files = files;
  //  for (const droppedFile of files) {

  //    // Is it a file?
  //    if (droppedFile.fileEntry.isFile) {
  //      const fileEntry = droppedFile.fileEntry as FileSystemFileEntry;
  //      fileEntry.file((file: File) => {

  //        // Here you can access the real file
  //        console.log(droppedFile.relativePath, file);

  //        /*
  //        // You could upload it like this:
  //        const formData = new FormData()
  //        formData.append('logo', file, relativePath)
 
  //        // Headers
  //        const headers = new HttpHeaders({
  //          'security-token': 'mytoken'
  //        })
 
  //        this.http.post('https://mybackend.com/api/upload/sanitize-and-save-logo', formData, { headers: headers, responseType: 'blob' })
  //        .subscribe(data => {
  //          // Sanitized logo returned from backend
  //          // Note 2020/01/09
  //          // Look toward the app.component.ts section of this page to see an example of creating a bootstrap-based file upload progress bar 
  //          // using the HTTPClient default responses.  Looks pretty simple, really.
  //          // https://www.positronx.io/angular-file-upload-with-progress-bar-tutorial/
  //        })
  //        */

  //      });
  //    } else {
  //      // It was a directory (empty directories are added, otherwise only files)
  //      const fileEntry = droppedFile.fileEntry as FileSystemDirectoryEntry;
  //      console.log(droppedFile.relativePath, fileEntry);
  //    }
  //  }
  //}

  //public fileOver(event) {
  //  console.log(event);
  //}

  //public fileLeave(event) {
  //  console.log(event);
  //}

  ngOnInit() {
  }

  public uploadFile = (files) => {
    if (files.length === 0) {
      return;
    }

    let fileToUpload = <File>files[0];
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);

    this.http.post('/api/Legacy/Upload', formData, { reportProgress: true, observe: 'events' })
      .subscribe(event => {
        if (event.type === HttpEventType.UploadProgress)
          this.progress = Math.round(100 * event.loaded / event.total);
        else if (event.type === HttpEventType.Response) {
          this.message = 'Upload success.';
          this.onUploadFinished.emit(event.body);
        }
      });
  } 

}
