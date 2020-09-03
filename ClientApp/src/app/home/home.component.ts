import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  public currentCount = 0;
  public http: HttpClient;
  public baseUrl: string;
  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.http = http;
    this.baseUrl = baseUrl;
  }

  public apriCancello() {
    this.http.get<boolean>(this.baseUrl + 'fermezza/apriCancello').subscribe(result => {
      if (result) {
        this.currentCount++;;
      }
    }, error => console.error(error));
  }
}

