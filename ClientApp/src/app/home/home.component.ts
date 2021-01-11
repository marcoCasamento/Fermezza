import { AuthorizeService } from '../../api-authorization/authorize.service';
import { Observable } from 'rxjs';
import { Component, Inject, OnInit  } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, tap } from 'rxjs/operators';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  public currentCount = 0;
  public http: HttpClient;
  public baseUrl: string;
  public weather: weatherData[];
  public isAuthenticated: Observable<boolean>;
  public userName: Observable<string>;
  constructor(private authorizeService: AuthorizeService, http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.http = http;
    this.baseUrl = baseUrl;
    
  }

  ngOnInit() {
    this.isAuthenticated = this.authorizeService.isAuthenticated();
    this.userName = this.authorizeService.getUser().pipe(map(u => u && u.name));
    if (this.isAuthenticated)
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

