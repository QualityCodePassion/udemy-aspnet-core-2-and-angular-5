import { PaginationResult } from './../_models/pagination';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import {User} from '../_models/User';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';


@Injectable()
export class UserService {
    baseUrl = environment.apiUrl;

    constructor(private authHttp: HttpClient) { }

    getUsers(page?: number, itemsPerPage?: number, userParams?: any): Observable<PaginationResult<User[]>> {
        const paginatedResult: PaginationResult<User[]> = new PaginationResult<User[]>();
        let params = new HttpParams();

        if (page && itemsPerPage) {
            params = params.append('pageNumber', page.toString());
            params = params.append('pageSize', itemsPerPage.toString());
        }

        // Apply the filtering if provided in the userParams
        if (userParams) {
            if (userParams.gender) {
                params = params.append('gender', userParams.gender);
            }
            if (userParams.minAge) {
                params = params.append('minAge', userParams.minAge);
            }
            if (userParams.maxAge) {
                params = params.append('maxAge', userParams.maxAge);
            }
            if (userParams.orderBy) {
                params = params.append('orderBy', userParams.orderBy);
            }
        }
        return this.authHttp
            .get<User[]>(this.baseUrl + 'users', {observe: 'response', params })
            .map(response => {
                paginatedResult.result = response.body;

                if (response.headers.get('pagination')) {
                    paginatedResult.pagination = JSON.parse(response.headers.get('pagination'));
                }

                return paginatedResult;
            });
    }

    getUser(id: number): Observable<User> {
        return this.authHttp
            .get<User>(this.baseUrl + 'users/' + id);
    }

    // This has now been moved to the "ErrorInterceptor" class (_services/error.interceptor.ts)
    // private errorHandler(error: any) {
    // }
}
