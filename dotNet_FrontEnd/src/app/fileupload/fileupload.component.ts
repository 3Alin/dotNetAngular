import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-fileupload',
  templateUrl: './fileupload.component.html',
  styleUrls: ['./fileupload.component.css']
})
export class FileuploadComponent implements OnInit {

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
  }

  selectedFile: File = null;
  templateFile: File = null;

  onFileSelected(event: any) {
    this.selectedFile = <File>event.target.files[0];
  }

  onTemplateFileSelected(event: any) {
    this.templateFile = <File>event.target.files[0];
  }

  onUpload() {
    
    var filedata = new FormData();
    filedata.append(this.selectedFile.name, this.selectedFile);

    this.http.post("/weatherforecast/uploadRequest", filedata)
      .subscribe((result) => { console.log(result) });

    filedata = new FormData();
    filedata.append(this.templateFile.name, this.templateFile);

    this.http.post("/weatherforecast/uploadRequest", filedata)
      .subscribe((result) => { console.log(result) });
  }
}
