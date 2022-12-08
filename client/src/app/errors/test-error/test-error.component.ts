import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-test-error',
  templateUrl: './test-error.component.html',
  styleUrls: ['./test-error.component.css']
})
export class TestErrorComponent implements OnInit {

  baseUrl = 'https://localhost:5001/api/'
  validationErrors : string[] = [];

  constructor(private httpClient : HttpClient) { }

  ngOnInit(): void {
  }

  get404Error()
  {
    this.httpClient.get(this.baseUrl + 'buggy/not-found').subscribe({
      next : response => console.log(response),
      error: err => console.log(err)
    });

  }


  get400Error()
  {
    this.httpClient.get(this.baseUrl + 'buggy/bad-request').subscribe({
      next : response => console.log(response),
      error: err => console.log(err)
    });
  }

  get500Error()
  {
    this.httpClient.get(this.baseUrl + 'buggy/server-error').subscribe({
      next : response => console.log(response),
      error: err => console.log(err)
    });
  }

  get401Error()
  {
    this.httpClient.get(this.baseUrl + 'buggy/auth').subscribe({
      next : response => console.log(response),
      error: err => console.log(err)
    });
  }

  get400ValidationError()
  {
    this.httpClient.post(this.baseUrl + 'account/register', {}).subscribe({
      next : response => console.log(response),
      error: err => 
      {
        this.validationErrors = err;
        console.log(err);
      }
    });
  }
}
