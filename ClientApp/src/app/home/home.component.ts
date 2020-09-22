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
  public weather: weatherData[];
  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.http = http;
    this.baseUrl = baseUrl;
    this.getWeather();
  }
  
  public apriCancello() {
    this.http.get<boolean>(this.baseUrl + 'fermezza/apriCancello').subscribe(result => {
      if (result) {
        this.currentCount++;
      }
    }, error => console.error(error));
  }

  public getWeather() {
    this.http.get<weatherData[]>(this.baseUrl + 'fermezza/getWeather').subscribe(result => {
      if (result) {
        this.weather = result;
      }
    }, error => console.error(error));
  }
}



interface weatherData {
  title: string;
  value: string;
  lastUpdate: Date;
}

