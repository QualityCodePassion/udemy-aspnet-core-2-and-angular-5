import { PaginationResult } from './../_models/pagination';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Http, RequestOptions, Headers, Response } from '@angular/http';
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

    getUsers(page?: number, itemsPerPage?: number): Observable<PaginationResult<User[]>> {
        const paginatedResult: PaginationResult<User[]> = new PaginationResult<User[]>();
        let queryString = '?';

        if (page && itemsPerPage) {
            queryString += 'pageNumber=' + page + '&pageSize=' + itemsPerPage;
        }

        return this.authHttp
            .get(this.baseUrl + 'users' + queryString)
            .map((response: Response) => {
                paginatedResult.result = response.json();

                if (response.headers.get('pagination')) {
                    paginatedResult.pagination = JSON.parse(response.headers.get('pagination'));
                }

                return paginatedResult;
            })
            .catch(this.errorHandler);
    }

    getUser(id: number): Observable<User> {
        return this.authHttp
            .get(this.baseUrl + 'users/' + id)
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
