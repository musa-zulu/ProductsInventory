import { HttpClient, HttpEventType } from '@angular/common/http';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.css']
})
export class FileUploadComponent implements OnInit {
  public progress: number;
  public message: string;  
  @Output() public onUploadFinished = new EventEmitter();
  constructor(private _http: HttpClient) { }
  ngOnInit() {
  }
  public uploadFile = (files) => {
    if (files.length === 0) {
      return;
    }
    const fileToUpload =  files[0] as File;
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    this._http.post('https://localhost:5001/api/v1/products/Upload', formData, {reportProgress: true, observe: 'events'})
      .subscribe(event => { 
        if (event.type === HttpEventType.UploadProgress) {
          this.progress = Math.round(100 * event.loaded / event.total);
        } else if (event.type === HttpEventType.Response) {
          this.message = 'Upload success.';
          localStorage.removeItem('imagePath');
          localStorage.setItem('imagePath', JSON.stringify(event.body));
          this.onUploadFinished.emit(event.body);
        } 
      });
  }
}
