import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-output',
  templateUrl: './output.component.html',
  styleUrls: ['./output.component.css']
})
export class OutputComponent {

  public output?: outputInfo; 

  constructor(http: HttpClient) {

    http.get<outputInfo>('/weatherforecast/output').subscribe(result => {

      this.output = result;
    }, error => console.error(error));
  }
}

interface outputInfo {
  file1: string;
  file2: string;
  result: string;
}
