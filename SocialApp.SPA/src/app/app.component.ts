import { JwtHelper } from 'angular2-jwt';
import { AuthService } from './_services/auth.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'app';
  jwtHelper: JwtHelper = new JwtHelper();

  constructor(private authService: AuthService) {}

  ngOnInit() {
    const token = localStorage.getItem('token');

    if (token) {
      this.authService.decodedToken = this.jwtHelper.decodeToken(token);
    }
  }

}
