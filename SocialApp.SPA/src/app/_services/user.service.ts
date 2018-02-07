import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Http, RequestOptions, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import {User} from '../_models/User';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import { AuthHttp } from 'angular2-jwt';


@Injectable()
export class UserService {
    baseUrl = environment.apiUrl;

    constructor(private authHttp: AuthHttp) { }

    getUsers(): Observable<User[]> {
        return this.authHttp
            .get(this.baseUrl + 'users')
            .map(response => <User[]>response.json())
            .catch(this.errorHandler);
    }

    getUser(id: number): Observable<User> {
        return this.authHttp
            .get(this.baseUrl + 'user/' + id)
            .map(response => <User>response.json())
            .catch(this.errorHandler);
    }

    private errorHandler(error: any) {
        const appError = error.headers.get('Application-error');

        if (appError) {
            return Observable.throw(appError);
        }

        if (!error) {
            return Observable.throw('Unknown server error');
        }

        const serverError = error.json();
        let modelStateErrors = '';

        if (serverError) {
            for (const key in serverError) {
                if (serverError[key]) {
                    modelStateErrors += serverError[key] + '\n';
                }
            }
        }

        return Observable.throw(modelStateErrors || 'Server error');
    }
}
