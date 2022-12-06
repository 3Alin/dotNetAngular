import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-person-form',
  templateUrl: './person-form.component.html',
  styleUrls: ['./person-form.component.css']
})
export class PersonFormComponent implements OnInit {

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
  }

  onSubmit(person: { firstName: string, lastName: string}) {

    if (person.firstName == "" || person.lastName == "")
      return;

    const options = {
      headers: new HttpHeaders().append('Content-type', 'application/json')
    };

    this.http.post("/weatherforecast/postRequest", JSON.stringify(person.firstName + " " + person.lastName), options)
      .subscribe((result) => { console.log(result) });
  }
}
