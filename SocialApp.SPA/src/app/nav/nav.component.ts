import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};

  constructor(private authservice: AuthService) { }

  ngOnInit() {
  }

  login() {
    this.authservice.login(this.model).subscribe( data => {
      console.log('Logged in Successfully');
    }, error => {
      console.log('Failed to login');
    });
  }

  logout() {
    this.authservice.userToken = null;
    localStorage.removeItem('token');
    console.log('logged out');
  }

  loggedIn() {
    // TODO must validate the token!
    const token = localStorage.getItem('token');
    return !!token;
  }

}
