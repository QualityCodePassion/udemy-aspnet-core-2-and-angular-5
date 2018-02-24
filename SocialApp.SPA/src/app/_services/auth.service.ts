import { environment } from './../../environments/environment';
import { AuthUser } from './../_models/authUser';
import { Injectable } from '@angular/core';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import { Observable } from 'rxjs/Observable';
import { JwtHelperService } from '@auth0/angular-jwt';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable()
export class AuthService {
    baseUrl = environment.apiUrl;
    userToken: any;
    decodedToken: any;

    constructor(private http: HttpClient, private jwtHelperService: JwtHelperService) { }

    login(model: any) {
        return this.http.post<AuthUser>(this.baseUrl + 'auth/login', model,
            { headers: new HttpHeaders().set('content-type', 'application/json')}).map(
            user => {
                if (user && user.tokenString) {
                    localStorage.setItem('token', user.tokenString);
                    this.decodedToken = this.jwtHelperService.decodeToken(user.tokenString);
                    console.log(this.decodedToken);
                    this.userToken = user.tokenString;
                }
            });
    }

    register(model: any) {
        return this.http.post(
            this.baseUrl + 'auth/register', model,
            { headers: new HttpHeaders().set('content-type', 'application/json')});
    }

    loggedIn () {
        const token = this.jwtHelperService.tokenGetter();

        if (!token) {
            return false;
        }

        return !this.jwtHelperService.isTokenExpired(token);
    }

    getUsername () {
        return this.decodedToken.unique_name;
    }

}
